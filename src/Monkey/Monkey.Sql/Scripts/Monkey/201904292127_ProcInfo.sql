create proc ProcInfo @procName sysname, @dbConnectionName sysname, @schema sysname = 'dbo'
as
begin
	select * from vw_ProcBindings pb where pb.ConnectionName = @dbConnectionName and pb.ProcedureName = @procName and pb.[Schema] = @schema;
	select * from dbo.webapi_RequestInfo(@procName, @dbConnectionName, @schema);
	select * from dbo.webapi_ResultInfo(@procName, @dbConnectionName, @schema);
end