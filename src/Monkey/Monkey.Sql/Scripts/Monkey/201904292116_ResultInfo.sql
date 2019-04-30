create function ResultInfo(@procName sysname, @dbConnectionName sysname, @schema sysname)
returns table
as
return 
select 
	pd.[Name] as ProcedureName,
	prd.[Name] as ResultColumnName,
	prd.[Type] as ResultColumnype,
	op.[Name] as PropertyName,
	t.[Name] as PropertyType,
	parent.[Name] as DeclaringType
from dbo.ProcedureBindings pb
inner join dbo.ProcedureDescriptors pd on pd.Id = pb.ProcedureId
inner join dbo.ProcedureResultDescriptors prd on prd.ProcedureId = pd.Id
inner join dbo.ProcedureResultColumnBindings prcb on prcb.ResultColumnId = prd.Id
inner join dbo.ObjectProperties op on op.Id = prcb.PropertyId
inner join dbo.ObjectTypes parent on op.DeclaringTypeId = parent.Id
inner join dbo.ObjectTypes t on op.PropertyTypeId = t.Id

where pd.ConnectionName=@dbConnectionName and pd.[Name] = @procName and pd.[Schema] = @schema