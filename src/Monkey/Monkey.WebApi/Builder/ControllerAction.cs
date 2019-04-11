using System;
using System.Collections.Generic;
using System.Linq;
using Monkey.Builder;
using Monkey.Generator;

namespace Monkey.WebApi.Builder
{
    public class ControllerAction : Addnotable
    {
        private string _route;
        private HttpVerb _verb;
        public override void WriteAttributes(SourceCodeBuilder sb)
        {
            sb.Append(sb.Prefix);
            base.WriteAttributes(sb);
            sb.AppendLine();
        }

        public string Route
        {
            get { return _route; }
            set
            {
                if (_route != null)
                {
                    var old = $"Route(\"{_route}\")";
                    _attributes.Remove(old);
                }
                _route = value;
                base.WithAttribute($"Route(\"{_route}\")");
            }
        }

        public string Name { get; private set; }
        public Type HandlerGenericInterfaceType { get; private set; }
        public Type HandlerRequestType { get; private set; }
        public Type HandlerReturnType { get; private set; }

        public HttpVerb Verb
        {
            get { return _verb; }
            set
            {
                string old = MapVerbToAttribute(_verb);
                _attributes.Remove(old);
                _verb = value;
                WithAttribute(MapVerbToAttribute(_verb));
            }
        }

        private string MapVerbToAttribute(HttpVerb verb)
        {
            switch (verb)
            {
                case HttpVerb.Post: return "HttpPost";
                case HttpVerb.Put: return "HttpPut";
                case HttpVerb.Delete: return "HttpDelete";
                case HttpVerb.Get: return "HttpGet";

                default: throw new NotSupportedException();
            }
        }

        public ArgumentCollection RequestArguments { get; private set; }
        public string ResponseType { get; private set; }
        public bool IsResponseCollection { get; private set; }
        private readonly Lazy<FullTypeNameInfo> _handlerInterfaceInfo;
        public FullTypeNameInfo HandlerInterfaceInfo
        {
            get { return _handlerInterfaceInfo.Value; }
        }

        public ControllerAction(HandlerInfo handler, string name,
            string responseType,
            HttpVerb verb,
            bool isResponseCollection,
            string route,
            params Argument[] requestArguments) : this(name, 
            requestArguments, 
            responseType, 
            verb, 
            isResponseCollection, 
            route, 
            handler.HandlerIType,
            handler.RequestType,
            handler.ResponseType)
        {
        }

        public ControllerAction(string name,
            IEnumerable<Argument> requestArguments, 
            string responseType, 
            HttpVerb verb, 
            bool isResponseCollection, 
            string route, 
            Type handlerGenericInterfaceType, 
            Type handlerRequestType, 
            Type handlerReturnType)
        {
            Name = name;
            RequestArguments = new ArgumentCollection(requestArguments);
            ResponseType = responseType;
            Verb = verb;
            IsResponseCollection = isResponseCollection;
            Route = route;
            HandlerGenericInterfaceType = handlerGenericInterfaceType;
            HandlerRequestType = handlerRequestType;
            HandlerReturnType = handlerReturnType;
            _handlerInterfaceInfo = new Lazy<FullTypeNameInfo>(() => FullTypeNameInfo.Parse(HandlerGenericInterfaceType.FullName));
        }
    }
}