create procedure webapi_BindStoredProc @procName sysname,
	@connectionName sysname, 
	@schema sysname = 'dbo', 
	@name nvarchar(255) = @procName, 
	@commandName nvarchar(255) = null,
	@queryName nvarchar(255) = null, 
	@resultName nvarchar(255) = null,
	@isReadOnly bit = null
as
begin
SET NOCOUNT ON;
	declare @dbName sysname = db_name();
	select p.[Order], p.ParamName [Name],  p.[Type] 
	into #params
	from dbo.webapi_StoredProcParameters(@dbName, @schema, @procName) p;
	
	declare @dfr table(is_hidden bit, column_ordinal int, [name] nvarchar(255), is_nullable bit, system_type_id int, system_type_name nvarchar(255), max_lenght int, precision int, scale int, collation_name nvarchar(255), 
	user_type_id int, user_type_database nvarchar(255), user_type_schema nvarchar(255), user_type_name nvarchar(255), assembly_qulified_type_name nvarchar(512), xml_collection_id int, xml_collection_database nvarchar(255), xml_collection_schema nvarchar(255),xml_collection_name nvarchar(255),
	is_xml_document bit, is_case_sensitive bit, is_fixed_length_clr_type bit, source_server nvarchar(255), source_database nvarchar(255), source_schema nvarchar(255), source_table nvarchar(255), soure_column nvarchar(255), is_identity_column bit, is_part_of_unique_key bit, is_updatable bit, is_computed_column bit,
	is_sparse_column_set bit, ordinal_in_order_by_list bit, order_by_is_descending bit, order_by_list_length bit, tds_type_id int, tds_length int, tds_collation_id int, tds_collation_sort_id int);

	declare @procExec nvarchar(255) = N'exec ' + @procName;
	insert into @dfr
	exec sp_describe_first_result_set @procExec

	select column_ordinal [Order], 
		[name] as [Name], 
		[Monkey].dbo.NormalizeSqlTypeName([system_type_name]) as [Type]
	into #resultSet
	from @dfr

	exec [Monkey].dbo.BindStoredProc @procName, @connectionName, @schema, @name, @commandName, @queryName, @resultName, @isReadOnly;

	drop table #params;
	drop table #resultSet;
end
