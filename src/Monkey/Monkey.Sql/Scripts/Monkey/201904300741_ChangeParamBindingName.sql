create proc ChangeParamBindingName @procName sysname, 
		@dbConnectionName sysname, 
		@schema sysname = 'dbo', 
		@paramName sysname, 
		@dstPropName sysname
as
begin
	SEt NOCOUNT ON;

	update op
		set [Name] = @dstPropName
	
	from dbo.ProcedureBindings pb
		inner join dbo.ProcedureDescriptors pd on pd.Id = pb.ProcedureId
		inner join dbo.ProcedureParameterDescriptors ppd on ppd.ProcedureId = pd.Id
		inner join dbo.ProcedureParameterBindings ppb on ppb.ParameterId = ppd.Id
		inner join dbo.ObjectProperties op on op.Id = ppb.PropertyId
		
	where 
		pd.ConnectionName=@dbConnectionName and 
		pd.[Name] = @procName and 
		pd.[Schema] = @schema and
		ppd.[Name] = @paramName

		if @@ROWCOUNT > 0
			print 'Parameter binding name changed.'
		else throw 51000, 'No binding was found to match the criteria', 16; 
end