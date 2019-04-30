using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monkey.Cqrs;
using Monkey.Generator;
using Monkey.PubSub;

namespace Monkey.WebApi
{

    public class CommandHandlerRegisterListener : IEventSubscriber<AssemblyLoadedEvent>
    {
        private readonly ICommandHandlerRegister _commandHandlerRegister;
        private readonly IQueryHandlerRegister _queryHandlerRegister;
        public CommandHandlerRegisterListener(ICommandHandlerRegister commandHandlerRegister, 
            IQueryHandlerRegister queryHandlerRegister)
        {
            _commandHandlerRegister = commandHandlerRegister;
            _queryHandlerRegister = queryHandlerRegister;
        }

        public async Task Handle(AssemblyLoadedEvent e)
        {
            if ((e.Purpose & AssemblyPurpose.Handlers) > 0)
            {
                var handlers = e.Assembly.GetExportedTypes()
                    .Select(x => new
                    {
                        ServiceType = x,
                        ImplementedCommandHandlers = x.GetInterfaces()
                            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)),
                        ImplementedQueryHandlers = x.GetInterfaces()
                            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))

                    }).ToArray();
                foreach (var h in handlers)
                {
                    foreach (var i in h.ImplementedCommandHandlers)
                        _commandHandlerRegister.Register(i.GetGenericArguments()[0], h.ServiceType);

                    foreach (var i in h.ImplementedQueryHandlers)
                        _queryHandlerRegister.Register(i.GetGenericArguments()[0], h.ServiceType);
                }


            }
        }
    }
    public interface IQueryHandlerRegister : IRequestHandlerRegister { }
    public interface ICommandHandlerRegister : IRequestHandlerRegister { }
    public interface IRequestHandlerRegister
    {
        Type this[Type type] { get; }
        void Register(Type commandType, Type handlerType);
    }
    public class QueryHandlerRegister : HandlerRegister, IQueryHandlerRegister { }
    public class CommandHandlerRegister : HandlerRegister, ICommandHandlerRegister { }
    public abstract class HandlerRegister : IRequestHandlerRegister
    {
        private readonly Dictionary<Type, Type> _dict;

        public HandlerRegister()
        {
            _dict = new Dictionary<Type, Type>();
        }

        public Type this[Type type]
        {
            get { return _dict[type]; }
        }

        public void Register(Type commandType, Type handlerType)
        {
            _dict.Add(commandType, handlerType);
        }
    }
}