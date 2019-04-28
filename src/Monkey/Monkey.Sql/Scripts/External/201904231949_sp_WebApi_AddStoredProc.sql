create procedure sp_WebApi_AddStoredProc @procName sysname,
	@dbName sysname, 
	@schema sysname = 'dbo', 
	@name nvarchar(255) = @procName, 
	@commandName nvarchar(255) = null,
	@queryName nvarchar(255) = null, 
	@resultName nvarchar(255) = null,
	@isReadOnly bit = null
as
begin
SEt NOCOUNT ON;

-- defaults
if @commandName is null
	set @commandName = @name + 'Command';
else set @isReadOnly = 0;

if @queryName is null
	set @queryName = @name + 'Query';
else set @isReadOnly = 1;

if @resultName is null
	set @resultName = @name + 'Result';

if @isReadOnly is null
	begin
		if @name like 'Get%'
			set @isReadOnly = 1;
		else
			set @isReadOnly = 0;
	end

-- validation
if exists (select 1 from Monkey.dbo.ProcedureDescriptors p where p.ConnectionName = @dbName and p.[Schema] = @schema and p.[Name] = @procName)
	throw 51000, 'Records already exists', 1; 

-- body
begin try
	begin tran WebApi_AddStoredProc

	declare @ids bigint = (next value for Monkey.dbo.HiLo)
	insert into Monkey.dbo.ProcedureDescriptors([Id], ConnectionName, [Schema], [Name])
	values (@ids, @dbName, @schema, @procName);
	declare @procId bigint = @ids;

	insert into Monkey.dbo.ProcedureParameterDescriptors(Id, [Name], ProcedureId, [Type],[Order])
	select p.[Order] + @ids, p.ParamName, @ids, p.[Type], p.[Order]
	from dbo.udf_StoredProcParameters(@dbName, @schema, @procName) p;
	
	declare @dfr table(is_hidden bit, column_ordinal int, [name] nvarchar(255), is_nullable bit, system_type_id int, system_type_name nvarchar(255), max_lenght int, precision int, scale int, collation_name nvarchar(255), 
	user_type_id int, user_type_database nvarchar(255), user_type_schema nvarchar(255), user_type_name nvarchar(255), assembly_qulified_type_name nvarchar(512), xml_collection_id int, xml_collection_database nvarchar(255), xml_collection_schema nvarchar(255),xml_collection_name nvarchar(255),
	is_xml_document bit, is_case_sensitive bit, is_fixed_length_clr_type bit, source_server nvarchar(255), source_database nvarchar(255), source_schema nvarchar(255), source_table nvarchar(255), soure_column nvarchar(255), is_identity_column bit, is_part_of_unique_key bit, is_updatable bit, is_computed_column bit,
	is_sparse_column_set bit, ordinal_in_order_by_list bit, order_by_is_descending bit, order_by_list_length bit, tds_type_id int, tds_length int, tds_collation_id int, tds_collation_sort_id int);

	declare @procExec nvarchar(255) = N'exec ' + @procName;
	insert into @dfr
	exec sp_describe_first_result_set @procExec

	-- Creating procedure metadata
	insert into Monkey.dbo.ProcedureResultDescriptors(Id, [Name], [Type], ProcedureId, [Order])
	select column_ordinal+@ids, 
		[name], 
		[Monkey].dbo.fn_WebApi_NormalizeSqlTypeName([system_type_name]), @ids, column_ordinal
	from @dfr

	-- Creating command
	declare @requestId bigint = @ids ;
	insert into [Monkey].[dbo].[ObjectTypes](Id, IsDynamic, IsPrimitive, IsVoid, [Name], [Namespace], Usage)
	values (@requestId, 1, 0, 0, case when @isReadOnly = 1 then @queryName else @commandName end, @schema, case when @isReadOnly = 1 then 'Query' else 'Command' end)

	-- Filling Properties of Command or Query
	insert into [Monkey].[dbo].ObjectProperties(Id, [Name], [PropertyTypeId], IsCollection, DeclaringTypeId)
	select @ids + c.[Order], [Monkey].dbo.fn_WebApi_MakePropertyName(c.[Name]), s.ObjectTypeId, 0, @requestId
	from [Monkey].dbo.ProcedureParameterDescriptors c
	inner join [Monkey].dbo.SqlObjectTypeMappings s on s.SqlType = c.[Type]
	where s.IsNullable = 1 and c.ProcedureId = @ids

	declare @c int = @@ROWCOUNT;
	if @c != (select count(*) from[Monkey].dbo.ProcedureParameterDescriptors c where c.ProcedureId = @ids)
		throw 51000, 'One parameter for request cannot be matched', 0; 

	-- Binding command/query Properties with parameters
	insert into [Monkey].dbo.ProcedureParameterBindings(PropertyId, ParameterId)
	select @ids + c.[Order], c.Id
	from [Monkey].dbo.ProcedureParameterDescriptors c
	inner join [Monkey].dbo.SqlObjectTypeMappings s on s.SqlType = c.[Type]
	where s.IsNullable = 1 and c.ProcedureId = @ids

	-- Creating or retriving Result object
	declare @resultId bigint = (select Id from [Monkey].dbo.[ObjectTypes] where [Name] = @resultName and [Namespace]=@schema);
	if @resultId is null
	begin
		insert into [Monkey].[dbo].[ObjectTypes](Id, IsDynamic, IsPrimitive, IsVoid, [Name], [Namespace], Usage)
		values (@ids + 1, 1, 0, 0, @resultName, @schema, 'Result')
		set @resultId = @ids + 1;

		-- creating Properties of Result
		-- need to understand from where to start id
		-- we assume that procedures wont have longer list of arguments thatn 50.
		
		insert into [Monkey].[dbo].ObjectProperties(Id, [Name], [PropertyTypeId], IsCollection, DeclaringTypeId)
			select @ids + c.[Order] + 50, [Monkey].dbo.fn_WebApi_MakePropertyName(c.[Name]), s.ObjectTypeId, 0, @resultId
			from [Monkey].dbo.ProcedureResultDescriptors c
			inner join [Monkey].dbo.SqlObjectTypeMappings s on s.SqlType = c.[Type]
			where s.IsNullable = 1 and c.ProcedureId = @ids

		-- binding Result Properties with resultset columns
		insert into [Monkey].dbo.ProcedureResultColumnBindings(PropertyId, ResultColumnId)
			select @ids + c.[Order] + 50, c.Id
			from [Monkey].dbo.ProcedureResultDescriptors c
			inner join [Monkey].dbo.SqlObjectTypeMappings s on s.SqlType = c.[Type]
			where s.IsNullable = 1 and c.ProcedureId = @ids

		declare @c2 int = @@ROWCOUNT;
		if @c2 != (select count(*) from[Monkey].dbo.ProcedureResultDescriptors c where c.ProcedureId = @ids)
		begin
			declare @missing nvarchar(max) = '';
			select @missing =  ''''+ c.[Name] + ''' parameter expects ''' + c.[Type] + ''' but this is not supported.'+ CHAR(13)+CHAR(10) + @missing
			from [Monkey].dbo.ProcedureResultDescriptors c
			left join [Monkey].dbo.SqlObjectTypeMappings s on s.SqlType = c.[Type]
			where s.IsNullable is null and c.ProcedureId = @ids;
			set @missing = 'One or more parameters for result cannot be matched: ' + @missing;
			throw 51000, @missing, 0; 
		end
	end
	insert into [Monkey].dbo.ProcedureBindings(ProcedureId, RequestId, ResultId, [Name], [Mode], IsResultCollection)
	values (@procId, @requestId, @resultId, @name, case when @isReadOnly = 1 then 4 else 2 end, @isReadOnly)

	commit tran WebApi_AddStoredProc
	print 'Stored procedure added to API'
end try
begin catch
	
	IF (@@TRANCOUNT > 0)
    BEGIN
      rollback tran 
      PRINT 'Error detected, all changes reversed'
    END 
    SELECT
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() AS ErrorState,
        ERROR_PROCEDURE() AS ErrorProcedure,
        ERROR_LINE() AS ErrorLine,
        ERROR_MESSAGE() AS ErrorMessage;
	throw;

end catch
end
