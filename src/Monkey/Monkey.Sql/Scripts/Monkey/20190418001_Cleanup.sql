create procedure [dbo].[Cleanup]
as 
begin
	begin try

	begin tran webapi_Cleanup
		delete from ProcedureResultColumnBindings
		delete from ProcedureParameterBindings
		delete from ProcedureBindings

		delete from ProcedureResultDescriptors
		delete from ProcedureParameterDescriptors
		delete from ProcedureDescriptors

		delete from ObjectProperties
		delete from ObjectTypes where Id >= 100
	commit tran webapi_Cleanup

	end try
	begin catch
	IF (@@TRANCOUNT > 0)
    BEGIN
      rollback tran webapi_Cleanup
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