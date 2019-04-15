using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monkey.Cqrs;
using Monkey.Generator;
using Monkey.PubSub;

namespace Monkey.WebApi
{
    public class CommandHandlerRegisterListener : IEventSubscriber<AssemblyCompiledEvent>
    {
        private ICommandHandlerRegister _commandHandlerRegister;

        public CommandHandlerRegisterListener(ICommandHandlerRegister commandHandlerRegister)
        {
            _commandHandlerRegister = commandHandlerRegister;
        }

        public async Task Handle(AssemblyCompiledEvent e)
        {
            if ((e.Purpose & AssemblyPurpose.Handlers) > 0)
            {
                var handlers = e.Assembly.GetExportedTypes()
                    .Select(x => new
                    {
                        ServiceType = x,
                        ImplementedHandlers = x.GetInterfaces()
                            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))

                    }).ToArray();
                foreach (var h in handlers)
                {
                    foreach (var i in h.ImplementedHandlers)
                    {
                        _commandHandlerRegister.Register(i.GetGenericArguments()[0], h.ServiceType);
                    }
                }


            }
        }
    }
    public interface ICommandHandlerRegister
    {
        Type this[Type type] { get; }
        void Register(Type commandType, Type handlerType);
    }
    public class CommandHandlerRegister : ICommandHandlerRegister
    {
        private readonly Dictionary<Type, Type> _commandHandlerByCommandType;

        public CommandHandlerRegister()
        {
            _commandHandlerByCommandType = new Dictionary<Type, Type>();
        }

        public Type this[Type type]
        {
            get { return _commandHandlerByCommandType[type]; }
        }

        public void Register(Type commandType, Type handlerType)
        {
            _commandHandlerByCommandType.Add(commandType, handlerType);
        }
    }
}