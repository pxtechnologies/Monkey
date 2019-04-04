using System;
using System.Collections.Generic;
using Monkey.Builder;

namespace Monkey.WebApi.Builder
{
    public class Action : Addnotable
    {
        public string Route { get; private set; }
        public string Name { get; private set; }
        public Type HandlerGenericInterfaceType { get; private set; }
        public Type HandlerRequestType { get; private set; }
        public Type HandlerReturnType { get; private set; }
        public HttpVerb Verb { get; private set; }
        public IList<Argument> RequestArguments { get; private set; }
        public string ResponseType { get; private set; }
        public bool IsResponseCollection { get; private set; }
    }
}