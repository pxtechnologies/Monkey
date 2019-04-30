create procedure ChangeMode @procName sysname,
	@dbName sysname, 
	@schema sysname = 'dbo', 
	@mode sysname,
	@isResultCollection bit = null
as
begin
SEt NOCOUNT ON;

-- validation
if not exists (select 1 from dbo.ProcedureDescriptors p where p.ConnectionName = @dbName and p.[Schema] = @schema and p.[Name] = @procName)
	throw 51000, 'Records dost not exist', 1; 

if @mode not in ('query','command')
	throw 51000, 'Unkown mode, should be one of: query, command',1;

if @isResultCollection is null
	select @isResultCollection = case when @mode = 'query' then 1 else 0 end;
-- body
begin try
	begin tran WebApi_ChangeMode
	declare @procId bigint = (select Id from dbo.ProcedureDescriptors p where p.ConnectionName = @dbName and p.[Schema] = @schema and p.[Name] = @procName)
	
	if @mode = 'command'
	begin
		update ot
			set [Usage]='Command'
		from dbo.ObjectTypes ot
		inner join ProcedureBindings pb on pb.RequestId=ot.Id
		where pb.ProcedureId = @procId;

		update pb
			set [Mode]=2
		from ProcedureBindings pb
		where pb.ProcedureId = @procId;
	end
	else if @mode = 'query'
	begin 
		update ot
			set [Usage]='Query'
		from dbo.ObjectTypes ot
		inner join ProcedureBindings pb on pb.RequestId=ot.Id
		where pb.ProcedureId = @procId;

		update pb
			set [Mode]=4
		from ProcedureBindings pb
		where pb.ProcedureId = @procId;
	end
	
	update ProcedureBindings
	set IsResultCollection = @isResultCollection
	where ProcedureId = @procId;

	commit tran WebApi_ChangeMode
	print 'Mode changed'
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
