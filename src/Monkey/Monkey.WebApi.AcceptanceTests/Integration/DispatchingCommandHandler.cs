using System.Threading.Tasks;
using Monkey.Cqrs;
using Monkey.WebApi.SimpleInjector;

namespace Monkey.WebApi.AcceptanceTests.Integration
{
    public class DispatchingCommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    {
        private readonly MockRegister _register;
        
        public DispatchingCommandHandler(MockRegister register)
        {
            _register = register;
        }

        public async Task<TResult> Execute(TCommand cmd)
        {
            return await _register.GetHandler<TCommand,TResult>().Execute(cmd);
        }
    }
}