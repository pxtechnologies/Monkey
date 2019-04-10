using System;
using System.Collections.Generic;

namespace Monkey.WebApi
{
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