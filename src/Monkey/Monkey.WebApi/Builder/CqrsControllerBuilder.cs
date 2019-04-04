using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monkey.Builder;

namespace Monkey.WebApi.Builder
{
    class CqrsControllerBuilder : ICodeBlockBuilder
    {
        private string _name;
        private string _nameSpace;
        public string _serviceName;
        private List<string> _usingNs;
        private List<Action> _actions;

        public CqrsControllerBuilder()
        {
            this._actions = new List<Action>();
            this._usingNs = new List<string>();
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
                .AddUsing("System.Data.SqlClient")
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

            sb.AppendLine($"public class {_name} : ControllerBase");
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
                var args = a.RequestArguments.Select(x => x.ToDeclaration());
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
                sb.AppendLine($"private _{a.Name.StartLower()}Handler;");
            }

            return sb;
        }
        private SourceCodeBuilder GenerateCtor(SourceCodeBuilder sb)
        {
            var args = string.Join(",", _actions.Select(x =>
                $"{x.HandlerGenericInterfaceType.Name}<{x.HandlerRequestType.Name},{x.HandlerReturnType.Name}> {x.Name.StartLower()}Handler"));
            sb.AppendLine($"public {_name}({args}, IMapper mapper)");
            sb.OpenBlock();

            sb.AppendLine("this._mapper = mapper;");
            foreach (var a in _actions)
            {
                sb.AppendLine($"this._{a.Name.StartLower()}Handler = {a.Name.StartLower()}Handler");
            }
            sb.CloseBlock();
            return sb;
        }

        
    }
}
