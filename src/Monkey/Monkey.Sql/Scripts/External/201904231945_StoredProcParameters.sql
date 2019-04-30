CREATE FUNCTION webapi_StoredProcParameters (
    @dbName sysname,
	@schema sysname,
	@procName sysname
)
RETURNS TABLE
AS
RETURN
	SELECT  
		name as [ParamName],  
		type_name(user_type_id) as [Type],
		parameter_id as [Order]
	FROM sys.parameters 
	WHERE object_id = object_id(@dbName + '.'  +@schema + '.' + @procName)
