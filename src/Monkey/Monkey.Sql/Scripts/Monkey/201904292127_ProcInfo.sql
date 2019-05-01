create proc ProcInfo @procName sysname, @dbConnectionName sysname, @schema sysname = 'dbo'
as
begin
	select * from ProcBindings pb where pb.ConnectionName = @dbConnectionName and pb.ProcedureName = @procName and pb.[Schema] = @schema;
	select * from dbo.RequestInfo(@procName, @dbConnectionName, @schema);
	select * from dbo.ResultInfo(@procName, @dbConnectionName, @schema);
end