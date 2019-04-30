create function MakePropertyName(@parameterOrColumn sysname)
returns sysname
as 
begin
	declare @result sysname;
	if len(@parameterOrColumn) > 2
	begin
		
		if SUBSTRING(@parameterOrColumn,1,1) = '@'
			set @parameterOrColumn = SUBSTRING(@parameterOrColumn,2,len(@parameterOrColumn)-1);
		set @result = UPPER(SUBSTRING(@parameterOrColumn,1,1)) + SUBSTRING(@parameterOrColumn,2, len(@parameterOrColumn)-1)
	end
	else 
		set @result = @parameterOrColumn;
	return(@result);
end