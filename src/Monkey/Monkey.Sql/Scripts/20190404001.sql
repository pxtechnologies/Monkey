alter procedure sp_WebApi_Publish
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
		if exists (select 1 from [Workspaces] where Id=@id and NodeName is not null)
		begin
			print 'Workspace created and model reloaded'
			set @i = 101;
		end
			else set @i = @i + 1;
	end
end
