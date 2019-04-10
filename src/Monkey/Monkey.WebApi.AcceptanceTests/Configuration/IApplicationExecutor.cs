using System;
using System.Threading.Tasks;

namespace Monkey.WebApi.AcceptanceTests.Configuration
{
    public interface IApplicationExecutor
    {
        void Execute(Type obj, Action<object> action);
        object Execute(Type obj, Func<object, object> method);
        void Execute<TService>(Action<TService> method);
        TResult Execute<TService, TResult>(Func<TService, TResult> method);
        Task<TResult> ExecuteAsync<TService, TResult>(Func<TService, Task<TResult>> method);
        Task ExecuteAsync<TService>(Func<TService, Task> method);
    }
}