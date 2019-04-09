using System.Collections.Generic;
using System.Linq;

namespace Monkey.Builder
{
    public class AutomapperProfilerBuilder : ICodeBlockBuilder
    {
        private string _name;
        private string _nameSpace;
        private List<string> _usingNs;
        public string Namespace => _nameSpace;
        public string Name => _name;
        private string _sourceType;
        private string _destinationType;
        public AutomapperProfilerBuilder WithName(string name)
        {
            this._name = name;
            return this;
        }
        public AutomapperProfilerBuilder ForType(string dst, string src)
        {
            _sourceType = src;
            _destinationType = dst;
            this._name = src + dst.EndsWithSingleSuffix("Profile");
            return this;
        }
        public AutomapperProfilerBuilder()
        {
            _name = "Profile";
            _nameSpace = "";
            Mappings = new List<Mapping>();
            _usingNs = new List<string>() { "System" };
        }
        public AutomapperProfilerBuilder InNamespace(string ns)
        {
            this._nameSpace = ns;
            return this;
        }

        public AutomapperProfilerBuilder AddUsing(string ns)
        {
            if (!_usingNs.Contains(ns))
                _usingNs.Add(ns);
            return this;
        }
        public void GenerateCode(SourceCodeBuilder sb)
        {

            if (!string.IsNullOrWhiteSpace(_nameSpace))
            {
                sb.AppendLine($"namespace {_nameSpace}");
                sb.OpenBlock();
            }

            if (_usingNs.Any())
            {
                foreach (var n in _usingNs)
                    sb.AppendLine($"using {n};");
            }

            sb.AppendLine();

            sb.AppendLine($"public class {_name} : Profile");
            

            sb.OpenBlock();


            sb.AppendLine($"public {_name}()");
            sb.OpenBlock();
            sb.AppendLine($"this.CreateMap<{_sourceType},{_destinationType}>();");
            foreach (var m in Mappings)
            {
                sb.AppendLine($"this.CreateMap<{m.SrcType}, {_destinationType}>().ForMember(x => x.{m.DstMemberName}, opt => opt.MapFrom(dst => dst));");
            }
            sb.CloseBlock();

            sb.CloseBlock();

            if (!string.IsNullOrWhiteSpace(_nameSpace))
            {
                sb.CloseBlock();
            }
        }
        private List<Mapping> Mappings { get; set; }
        
        public AutomapperProfilerBuilder WithDefaultMapping()
        {
            return this;
        }

        public void WithValueMapping(string srcType, string dstMemberName)
        {
            Mappings.Add(new Mapping(srcType, dstMemberName));
        }

        class Mapping
        {
            public string SrcType { get; private set; }
            public string DstMemberName { get; private set; }

            public Mapping(string srcType, string dstMemberName)
            {
                SrcType = srcType;
                DstMemberName = dstMemberName;
            }
        }
    }
}