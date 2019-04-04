using System;

namespace Monkey.Generator
{
    public class HandlerInfo
    {
        internal HandlerInfo(Type handlerInterfaceType, 
            Type handlerType,
            Type requestType,
            Type responseType,
            bool isResponseCollection,
            bool isCommandHandler,
            bool isQueryHandler)
        {
            HandlerType = handlerType;
            RequestType = requestType;
            ResponseType = responseType;
            IsResponseCollection = isResponseCollection;
            IsCommandHandler = isCommandHandler;
            IsQueryHandler = isQueryHandler;
            HandlerInterfaceType = handlerInterfaceType;
        }
        public Type HandlerInterfaceType { get; private set; }
        public Type HandlerType { get; private set; }
        public Type RequestType { get; private set; }
        public Type ResponseType { get; private set; }
        public bool IsResponseCollection { get; private set; }
        public bool IsCommandHandler { get; private set; }
        public bool IsQueryHandler { get; private set; }
    }
}