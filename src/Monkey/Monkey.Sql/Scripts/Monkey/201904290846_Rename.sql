create procedure Rename @procName sysname,
	@dbName sysname, 
	@schema sysname = 'dbo', 
	@objType sysname,
	@value nvarchar(255)
as
begin
SEt NOCOUNT ON;

-- validation
if not exists (select 1 from dbo.ProcedureDescriptors p where p.ConnectionName = @dbName and p.[Schema] = @schema and p.[Name] = @procName)
	throw 51000, 'Records dost not exist', 1; 

if @objType not in ('handler','query','command','result')
	throw 51000, 'Unkown objType, should be one of: handler, query, command, result',1;

if @value is null or @value = ''
	throw 51000, 'Value cannot be null or empty',1;

-- body
begin try
	begin tran WebApi_Rename
	declare @procId bigint = (select Id from dbo.ProcedureDescriptors p where p.ConnectionName = @dbName and p.[Schema] = @schema and p.[Name] = @procName)
	
	if @objType = 'handler'
	begin
		update dbo.ProcedureBindings set [Name] = @value where ProcedureId = @procId;
		declare @mode int = (select Mode from dbo.ProcedureBindings where ProcedureId = @procId);
		if @mode = 2
		begin
			set @objType = 'command';
			set @value = @value + 'Command';
		end
		else if @mode = 4
		begin
			set @objType = 'query';
			set @value = @value + 'Query';
		end
	end
	
	if @objType = 'query'
	begin
		update ot
			set [Name] = @value
		from dbo.ObjectTypes ot
		inner join dbo.ProcedureBindings pb on pb.RequestId=ot.Id
		where pb.ProcedureId = @procId;
	end
	else if @objType = 'command'
	begin
		update ot
			set [Name] = @value
		from Monkey.dbo.ObjectTypes ot
		inner join dbo.ProcedureBindings pb on pb.RequestId=ot.Id
		where pb.ProcedureId = @procId;
	end
	else if @objType = 'result'
	begin
		update ot
			set [Name] = @value
		from dbo.ObjectTypes ot
		inner join dbo.ProcedureBindings pb on pb.ResultId=ot.Id
		where pb.ProcedureId = @procId;
	end
	
	commit tran WebApi_Rename
	print 'Binding renamed'
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
