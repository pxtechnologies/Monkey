create view ProcBindings
as
select 
	pr.[Name] as [ProcedureName], 
	pr.[Schema] as [Schema], 
	pr.ConnectionName as [ConnectionName], 
	pb.[Name] as HandlerName,
	pb.IsResultCollection as [IsResultCollection],
	case pb.[Mode] when 2 then 'Command' else 'Query' end as [Mode],
	otreq.[Name] as [RequestType],
	otreq.[Namespace] as [RequestTypeNamespace],
	otres.[Name] as [ResultType],
	otres.[Namespace] as [ResultTypeNamespace]
from dbo.ProcedureBindings pb
inner join dbo.ProcedureDescriptors pr on pr.Id=pb.ProcedureId
inner join dbo.ObjectTypes otreq on pb.RequestId=otreq.Id
inner join dbo.ObjectTypes otres on pb.ResultId=otres.Id