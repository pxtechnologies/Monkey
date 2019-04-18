create function fn_WebApi_NormalizeSqlTypeName(@nameWithPrecision sysname)
returns sysname
as 
begin
	declare @result sysname = @nameWithPrecision;
	
	declare @startIx bigint = CHARINDEX('(',@nameWithPrecision);
	declare @endIx bigint = CHARINDEX(')',@nameWithPrecision);

	if @startIx > 0
	begin
		set @result = SUBSTRING(@nameWithPrecision,1,@startIx-1);
	end


	return(@result);
end