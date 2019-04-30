create function RequestInfo(@procName sysname, @dbConnectionName sysname, @schema sysname)
returns table
as
return 
select 
	pd.[Name] as ProcedureName,
	ppd.[Name] as ParameterName,
	ppd.[Type] as ParemeterType,
	ppd.[Order] as ParameterOrder,
	op.[Name] as PropertyName,
	t.[Name] as PropertyType,
	parent.[Name] as DeclaringType
from dbo.ProcedureBindings pb
inner join dbo.ProcedureDescriptors pd on pd.Id = pb.ProcedureId
inner join dbo.ProcedureParameterDescriptors ppd on ppd.ProcedureId = pd.Id
inner join dbo.ProcedureParameterBindings ppb on ppb.ParameterId = ppd.Id
inner join dbo.ObjectProperties op on op.Id = ppb.PropertyId
inner join dbo.ObjectTypes parent on op.DeclaringTypeId = parent.Id
inner join dbo.ObjectTypes t on op.PropertyTypeId = t.Id

where pd.ConnectionName=@dbConnectionName and pd.[Name] = @procName and pd.[Schema] = @schema