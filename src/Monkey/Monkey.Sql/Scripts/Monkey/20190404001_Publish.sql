create procedure Publish
as
begin
	declare @id bigint = next value for HiLo;

	insert into Workspaces(Id, [VersionSignature], IsDisabled,
		[Status], [HeartBeat], [Created])
	values(@id, newid(), 0, 0, getdate(), getdate())

	declare @i int = 0;
	while @i < 100
	begin
		WAITFOR DELAY '00:00:01'
		if exists (select 1 from [Workspaces] where Id=@id and NodeName is not null and [Status] <> 0)
		begin
			declare @r nvarchar(255);
			declare @s int;
			declare @error varchar(max);

			select @r = case [Status] 
				when -1 then 'Error' 
				when 2 then 'Loaded'
				when 3 then 'Running'
				when 1 then 'Compiled' end, 
				@error = [Error], 
				@s = [Status]
			from [Workspaces] where Id=@id and NodeName is not null and [Status] <> 0
						
			if @r = 'Error' 
				throw 51000, @error, 1;
			print N'Workspace ' + @r
			set @i = 101;
		end
			else set @i = @i + 1;
	end
end
