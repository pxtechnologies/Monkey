using System;
using System.Threading.Tasks;
using Monkey.Cqrs;

namespace Monkey.WebApi
{
    public class DispatchingCommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    {
        private readonly ICommandHandlerRegister _register;
        private readonly IServiceProvider _serviceProvider;

        public DispatchingCommandHandler(ICommandHandlerRegister register, IServiceProvider serviceProvider)
        {
            _register = register;
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult> Execute(TCommand cmd)
        {
            var handler = (ICommandHandler<TCommand, TResult>)_serviceProvider.GetService(_register[typeof(TCommand)]);
            return await handler.Execute(cmd);
        }
    }
}