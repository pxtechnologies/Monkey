using System;
using Monkey.Cqrs;

namespace Monkey.Generator
{
    public interface IHandlerInfoFactory
    {
        HandlerInfo Create(Type interfaceType, Type handlerType);
    }
    public class HandlerInfoFactory : IHandlerInfoFactory
    {
        public HandlerInfo Create(Type interfaceType, Type handlerType)
        {
            //TODO: Refactor to allow handlerType to be null

            if(interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

            if (interfaceType.IsAssignableFrom(handlerType))
            {
                if (interfaceType.IsGenericType && !interfaceType.IsGenericTypeDefinition)
                {
                    var genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    bool isCommandHandler = false;
                    bool isQueryHandler = false;
                    bool isResponseCollection = false;
                    Type[] args = interfaceType.GetGenericArguments();
                    if (genericTypeDefinition == typeof(ICommandHandler<,>))
                    {
                        if (args[0].IsArray)
                            return null;
                        isCommandHandler = true;
                        isResponseCollection = args[1].IsArray;
                    }
                    else if (genericTypeDefinition == typeof(IQueryHandler<,>))
                    {
                        if (args[0].IsArray || args[1].IsArray)
                            return null;
                        isQueryHandler = true;
                        isResponseCollection = true;
                    }
                    else if(genericTypeDefinition == typeof(ISingleQueryHandler<,>))
                    {
                        if (args[0].IsArray || args[1].IsArray)
                            return null;
                        isQueryHandler = true;
                    }
                    else
                    {
                        return null;
                    }
                    
                    return new HandlerInfo(interfaceType, 
                        handlerType, 
                        args[0], 
                        args[1], 
                        isResponseCollection, 
                        isCommandHandler, 
                        isQueryHandler);
                }
            }

            return null;
        }
    }
}