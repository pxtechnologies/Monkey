using System;
using System.Threading.Tasks;

namespace Monkey.Cqrs
{
    public interface ICommandHandler<in TCommand, TResult>
    {
        Task<TResult> Execute(TCommand cmd);
    }
}
