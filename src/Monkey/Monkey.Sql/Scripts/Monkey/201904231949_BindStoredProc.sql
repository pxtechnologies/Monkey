create procedure BindStoredProc @procName sysname,
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
if exists (select 1 from dbo.ProcedureDescriptors p where p.ConnectionName = @dbName and p.[Schema] = @schema and p.[Name] = @procName)
	throw 51000, 'Records already exists', 1; 

if not exists (select 1 from #params) and not exists (select 1 from #resultSet)
	throw 51000, 'You have forgotten to fill temporary tables: #params or #resultSet with ([Order],[Name],[Type])',16;
-- body
begin try
	begin tran BindStoredProc

	declare @ids bigint = (next value for dbo.HiLo)
	insert into dbo.ProcedureDescriptors([Id], ConnectionName, [Schema], [Name])
	values (@ids, @dbName, @schema, @procName);
	declare @procId bigint = @ids;

	insert into dbo.ProcedureParameterDescriptors(Id, [Name], ProcedureId, [Type],[Order])
	select p.[Order] + @ids, p.[Name], @ids, p.[Type], p.[Order]
	from #params p;

	-- Creating procedure metadata
	insert into dbo.ProcedureResultDescriptors(Id, [Name], [Type], ProcedureId, [Order])
	select [Order]+@ids, 
		[Name], 
		[Type], 
		@ids, 
		[Order]
	from #resultSet

	-- Creating command
	declare @requestId bigint = @ids ;
	insert into [dbo].[ObjectTypes](Id, IsDynamic, IsPrimitive, IsVoid, [Name], [Namespace], Usage)
	values (@requestId, 1, 0, 0, case when @isReadOnly = 1 then @queryName else @commandName end, @schema, case when @isReadOnly = 1 then 'Query' else 'Command' end)

	-- Filling Properties of Command or Query
	insert into [dbo].ObjectProperties(Id, [Name], [PropertyTypeId], IsCollection, DeclaringTypeId)
	select @ids + c.[Order], dbo.MakePropertyName(c.[Name]), s.ObjectTypeId, 0, @requestId
	from dbo.ProcedureParameterDescriptors c
	inner join dbo.SqlObjectTypeMappings s on s.SqlType = c.[Type]
	where s.IsNullable = 1 and c.ProcedureId = @ids

	declare @c int = @@ROWCOUNT;
	if @c != (select count(*) from dbo.ProcedureParameterDescriptors c where c.ProcedureId = @ids)
		throw 51000, 'One parameter for request cannot be matched', 0; 

	-- Binding command/query Properties with parameters
	insert into dbo.ProcedureParameterBindings(PropertyId, ParameterId)
	select @ids + c.[Order], c.Id
	from dbo.ProcedureParameterDescriptors c
	inner join dbo.SqlObjectTypeMappings s on s.SqlType = c.[Type]
	where s.IsNullable = 1 and c.ProcedureId = @ids

	-- Creating or retriving Result object
	declare @resultId bigint = (select Id from dbo.[ObjectTypes] where [Name] = @resultName and [Namespace]=@schema);
	if @resultId is null
	begin
		insert into [dbo].[ObjectTypes](Id, IsDynamic, IsPrimitive, IsVoid, [Name], [Namespace], Usage)
		values (@ids + 1, 1, 0, 0, @resultName, @schema, 'Result')
		set @resultId = @ids + 1;

		-- creating Properties of Result
		-- need to understand from where to start id
		-- we assume that procedures wont have longer list of arguments thatn 50.
		
		insert into [dbo].ObjectProperties(Id, [Name], [PropertyTypeId], IsCollection, DeclaringTypeId)
			select @ids + c.[Order] + 50, dbo.MakePropertyName(c.[Name]), s.ObjectTypeId, 0, @resultId
			from dbo.ProcedureResultDescriptors c
			inner join dbo.SqlObjectTypeMappings s on s.SqlType = c.[Type]
			where s.IsNullable = 1 and c.ProcedureId = @ids

		-- binding Result Properties with resultset columns
		insert into dbo.ProcedureResultColumnBindings(PropertyId, ResultColumnId)
			select @ids + c.[Order] + 50, c.Id
			from dbo.ProcedureResultDescriptors c
			inner join dbo.SqlObjectTypeMappings s on s.SqlType = c.[Type]
			where s.IsNullable = 1 and c.ProcedureId = @ids

		declare @c2 int = @@ROWCOUNT;
		if @c2 != (select count(*) from dbo.ProcedureResultDescriptors c where c.ProcedureId = @ids)
		begin
			declare @missing nvarchar(max) = '';
			select @missing =  ''''+ c.[Name] + ''' parameter expects ''' + c.[Type] + ''' but this is not supported.'+ CHAR(13)+CHAR(10) + @missing
			from dbo.ProcedureResultDescriptors c
			left join dbo.SqlObjectTypeMappings s on s.SqlType = c.[Type]
			where s.IsNullable is null and c.ProcedureId = @ids;
			set @missing = 'One or more parameters for result cannot be matched: ' + @missing;
			throw 51000, @missing, 0; 
		end
	end
	insert into dbo.ProcedureBindings(ProcedureId, RequestId, ResultId, [Name], [Mode], IsResultCollection)
	values (@procId, @requestId, @resultId, @name, case when @isReadOnly = 1 then 4 else 2 end, @isReadOnly)

	commit tran BindStoredProc
	print 'Stored procedure binded in API, you need to publish your changes.'
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
