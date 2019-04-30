create proc ChangeResultColBindingName @procName sysname, 
		@dbConnectionName sysname, 
		@schema sysname = 'dbo', 
		@resColName sysname, 
		@dstPropName sysname
as
begin
	SEt NOCOUNT ON;

	update op
		set [Name] = @dstPropName
	
	from dbo.ProcedureBindings pb
		inner join dbo.ProcedureDescriptors pd on pd.Id = pb.ProcedureId
		inner join dbo.ProcedureResultDescriptors prd on prd.ProcedureId = pd.Id
		inner join dbo.ProcedureResultColumnBindings prcb on prcb.ResultColumnId = prd.Id
		inner join dbo.ObjectProperties op on op.Id = prcb.PropertyId
		
	where 
		pd.ConnectionName=@dbConnectionName and 
		pd.[Name] = @procName and 
		pd.[Schema] = @schema and
		prd.[Name] = @resColName

		if @@ROWCOUNT > 0
			print 'Column binding name changed.'
		else throw 51000, 'No binding was found to match the criteria', 16; 
end