create or alter proc Help @objName nvarchar(255) = null as
begin
if @objName is null
	select phelp.objname as [Routine name], phelp.[value] [Description]
	FROM fn_listextendedproperty ('MS_Description', 'schema', 'dbo', 'PROCEDURE', default, NULL, NULL) phelp
	union all select objname, [value] FROM fn_listextendedproperty ('MS_Description', 'schema', 'dbo', 'FUNCTION', default, NULL, NULL); 

else 
begin
	select phelp.objname as [Routine name], phelp.[value] [Description]
	FROM fn_listextendedproperty ('MS_Description', 'schema', 'dbo', 'PROCEDURE', @objName, NULL, NULL) phelp
	union all select objname, [value] FROM fn_listextendedproperty ('MS_Description', 'schema', 'dbo', 'FUNCTION', @objName, NULL, NULL); 

	select p.PARAMETER_NAME [Parameter name], d.[value] as [Description]
	from [INFORMATION_SCHEMA].PARAMETERS p
	INNER JOIN [INFORMATION_SCHEMA].ROUTINES r on p.SPECIFIC_NAME=r.ROUTINE_NAME
	CROSS APPLY fn_listextendedproperty ('MS_Description', 'schema', 'dbo', r.ROUTINE_TYPE, @objName, 'PARAMETER', p.PARAMETER_NAME) d
	WHERE r.ROUTINE_NAME=@objName
end
end