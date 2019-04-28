using System;
using System.Threading.Tasks;
using Monkey.Cqrs;

namespace Monkey.WebApi
{
    public class DispatchingQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    {
        private readonly IQueryHandlerRegister _register;
        private readonly IServiceProvider _serviceProvider;

        public DispatchingQueryHandler(IQueryHandlerRegister register, IServiceProvider serviceProvider)
        {
            _register = register;
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult[]> Execute(TQuery cmd)
        {
            var handler = (IQueryHandler<TQuery, TResult>)_serviceProvider.GetService(_register[typeof(TQuery)]);
            return await handler.Execute(cmd);
        }
    }
}