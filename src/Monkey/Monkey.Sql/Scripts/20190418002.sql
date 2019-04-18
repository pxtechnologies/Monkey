create function TableStats (@minId bigint=0)
RETURNS TABLE
as
return
select '__EFMigrationsHistory' as [TableName], count(*) as [Count], null as MaxId from  [dbo].[__EFMigrationsHistory]
union all select 'Compilations', count(*), max(Id) from  [dbo].[Compilations] where Id > @minId
union all select 'Controllers', count(*), max(Id) from  [dbo].[Controllers] where Id > @minId
union all select 'ObjectTypes', count(*), max(Id) from  [dbo].[ObjectTypes] where Id > @minId
union all select 'ProcedureDescriptors', count(*), max(Id) from  [dbo].[ProcedureDescriptors] where Id > @minId
union all select 'ControllerActions', count(*), max(Id) from  [dbo].[ControllerActions] where Id > @minId
union all select 'ObjectProperties', count(*), max(Id) from  [dbo].[ObjectProperties] where Id > @minId
union all select 'SqlObjectTypeMappings', count(*), max(Id) from  [dbo].[SqlObjectTypeMappings] where Id > @minId
union all select 'ProcedureBindings', count(*), null from  [dbo].[ProcedureBindings] pb where pb.ProcedureId > @minId or pb.ResultId > @minId
union all select 'ProcedureParameterDescriptors', count(*), max(Id) from  [dbo].[ProcedureParameterDescriptors] where Id > @minId
union all select 'ProcedureResultDescriptors', count(*), max(Id) from  [dbo].[ProcedureResultDescriptors] where Id > @minId
union all select 'ActionParameterBindings', count(*), null from  [dbo].[ActionParameterBindings]
union all select 'ProcedureParameterBindings', count(*), null from  [dbo].[ProcedureParameterBindings] b where b.ParameterId > @minId or b.PropertyId > @minId
union all select 'ProcedureResultColumnBindings', count(*), null from  [dbo].[ProcedureResultColumnBindings] bc where bc.PropertyId > @minId or bc.ResultColumnId > @minId
