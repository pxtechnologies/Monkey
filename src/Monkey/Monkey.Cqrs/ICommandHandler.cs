using System;
using System.Threading.Tasks;

namespace Monkey.Cqrs
{
    public interface ICommandHandler<in TCommand, TResult>
    {
        Task<TResult> Execute(TCommand cmd);
    }

    public interface IQueryHandler<in TQuery, TQueryResult>
    {
        Task<TQueryResult[]> Execute(TQuery query);
    }

    public interface ISingleQueryHandler<in TQuery, TQueryResult>
    {
        Task<TQueryResult> Execute(TQuery query);
    }
}
