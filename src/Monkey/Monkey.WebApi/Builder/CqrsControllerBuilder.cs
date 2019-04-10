using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monkey.Builder;
using Monkey.Generator;

namespace Monkey.WebApi.Builder
{
    class CqrsControllerBuilder : ICodeBlockBuilder
    {
        private string _name;
        private string _nameSpace;
        //public string _serviceName;
        private List<string> _usingNs;
        private List<ControllerAction> _actions;

        public void AppendAction(HandlerInfo handler, 
            string name,
            string responseType,
            HttpVerb verb,
            bool isResponseCollection,
            string route,
            params Argument[] requestArguments)
        {
            if(_actions.Any(x=>x.Route == route && x.Verb == verb))
                throw new ArgumentException("Route");
            // TODO: add validation and UT
            _actions.Add(new ControllerAction(handler, name, responseType, verb, isResponseCollection, route, requestArguments));
        }

        public CqrsControllerBuilder()
        {
            this._actions = new List<ControllerAction>();
            this._usingNs = new List<string>();
        }

        public string Namespace => _nameSpace;
        public string TypeName => $"{_name}Controller";
        public CqrsControllerBuilder WithName(string name)
        {
            this._name = name.Replace("Controller","");
            return this;
        }

        public CqrsControllerBuilder InNamespace(string ns)
        {
            this._nameSpace = ns;
            return this;
        }
        public CqrsControllerBuilder AddUsing(string ns)
        {
            if (!_usingNs.Contains(ns))
                _usingNs.Add(ns);
            return this;
        }
        public CqrsControllerBuilder AddDefaultUsings()
        {
            AddUsing("System")
                .AddUsing("System.Configuration")
                .AddUsing("System.Data")
                .AddUsing("System.Threading")
                .AddUsing("System.Threading.Tasks")
                .AddUsing("Monkey.Cqrs")
                .AddUsing("Microsoft.Extensions.Configuration");

            return this;
        }
        public void GenerateCode(SourceCodeBuilder sb)
        {
            if (!string.IsNullOrWhiteSpace(_nameSpace))
                sb.AppendLine($"namespace {_nameSpace}")
                    .OpenBlock();

            if (_usingNs.Any())
                foreach (var ns in _usingNs)
                    sb.AppendLine($"using {ns};");
            sb.AppendLine();

            sb.AppendLine($"public class {TypeName} : ControllerBase");
            sb.OpenBlock();

            GenerateLocals(sb).AppendLine();
            GenerateCtor(sb).AppendLine();
            GenerateActions(sb);

            sb.CloseBlock();

            if (!string.IsNullOrWhiteSpace(_nameSpace))
                sb.CloseBlock();
        }

        private SourceCodeBuilder GenerateActions(SourceCodeBuilder sb)
        {
            foreach (var a in _actions)
            {
                a.WriteAttributes(sb);
                var args = a.RequestArguments.ToString();
                a.WriteAttributes(sb);
                sb.AppendLine($"public async Task<{a.ResponseType}> {a.Name}({args})");
                sb.OpenBlock();

                sb.AppendLine($"{a.HandlerRequestType.Name} arg = new {a.HandlerRequestType.Name}();");

                foreach (var arg in a.RequestArguments)
                {
                    sb.AppendLine($"this._mapper.Map({arg.Name}, arg);");
                }

                sb.AppendLine($"{a.HandlerReturnType.Name} result = await _{a.Name.StartLower()}Handler.Execute(arg);");
                sb.AppendLine($"return _mapper.Map<{a.ResponseType}>(result);");
                sb.CloseBlock().AppendLine();
            }

            
            return sb;
        }

        public SourceCodeBuilder GenerateLocals(SourceCodeBuilder sb)
        {
            sb.AppendLine("private IMapper _mapper;");
            foreach (var a in _actions)
            {
                sb.AppendLine($"private {a.HandlerInterfaceInfo} _{a.Name.StartLower()}Handler;");
            }

            return sb;
        }
        private SourceCodeBuilder GenerateCtor(SourceCodeBuilder sb)
        {
            
            var args = string.Join(",", _actions.Select(x => $"{x.HandlerInterfaceInfo} {x.Name.StartLower()}Handler"));
            sb.AppendLine($"public {TypeName}({args}, IMapper mapper)");
            sb.OpenBlock();

            sb.AppendLine("this._mapper = mapper;");
            foreach (var a in _actions)
            {
                sb.AppendLine($"this._{a.Name.StartLower()}Handler = {a.Name.StartLower()}Handler;");
            }
            sb.CloseBlock();
            return sb;
        }

        
    }
}
