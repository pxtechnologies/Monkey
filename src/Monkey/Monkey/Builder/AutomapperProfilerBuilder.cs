using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Monkey.Builder
{
    public class AutomapperProfilerBuilder : ICodeBlockBuilder
    {
        private string _name;
        private string _nameSpace;
        private List<string> _usingNs;
        public string Namespace => _nameSpace;
        public string Name => _name;
        private FullTypeNameInfo _sourceType;
        private FullTypeNameInfo _destinationType;
        public AutomapperProfilerBuilder WithName(string name)
        {
            this._name = name;
            return this;
        }
        public AutomapperProfilerBuilder ForType(FullTypeNameInfo dst, FullTypeNameInfo src)
        {
            _sourceType = src;
            _destinationType = dst;
            this._name = src.IsNullable ? src.GenericArguments.First().Name : src.Name + dst.Name.EndsWithSingleSuffix("Profile");
            return this;
        }

        public AutomapperProfilerBuilder AddIgnore(params string[] ignoreParams)
        {
            IgnoreProperties.AddRange(ignoreParams);
            return this;
        }
        public AutomapperProfilerBuilder()
        {
            _name = "Profile";
            _nameSpace = "";
            Mappings = new List<Mapping>();
            IgnoreProperties = new List<string>();
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

            string eol = IgnoreProperties.Any() ? "" : ";";
            sb.AppendLine($"{sb.Prefix}this.CreateMap<{_sourceType.ToString()}, {_destinationType.ToString()}>(){eol}");
            if(IgnoreProperties.Any())
            {
                sb.IndentUp();
                for (var index = 0; index < this.IgnoreProperties.Count; index++)
                {
                    var i = this.IgnoreProperties[index];
                    eol = index == this.IgnoreProperties.Count - 1 ? ";" : "";
                    sb.AppendLine($".ForMember(dst => dst.{i}, opt => opt.Ignore()){eol}");
                }

                sb.IndentDown();
            }
            foreach (var m in Mappings)
            {
                if(m.SrcType.IsNullable)
                    sb.AppendLine($"this.CreateMap<{m.SrcType.GenericArguments.First().ToString()}, {_destinationType.ToString()}>().ForMember(x => x.{m.DstMemberName}, opt => opt.MapFrom(dst => dst)).ForAllOtherMembers(opt => opt.Ignore());");

                else
                    sb.AppendLine($"this.CreateMap<{m.SrcType.ToString()}, {_destinationType.ToString()}>().ForMember(x => x.{m.DstMemberName}, opt => opt.MapFrom(dst => dst)).ForAllOtherMembers(opt => opt.Ignore());");
            }

            
            sb.CloseBlock();

            sb.CloseBlock();

            if (!string.IsNullOrWhiteSpace(_nameSpace))
            {
                sb.CloseBlock();
            }
        }
        private List<Mapping> Mappings { get; set; }
        private List<string> IgnoreProperties { get; set; }
        public AutomapperProfilerBuilder WithDefaultMapping()
        {
            return this;
        }
        public AutomapperProfilerBuilder WithDefaultUsings()
        {
            this.AddUsing("AutoMapper");
            this.AddUsing("System.Linq");
            this.AddUsing("System.Collections.Generic");
            return this;
        }

        public AutomapperProfilerBuilder WithValueMapping(FullTypeNameInfo srcType, string dstMemberName)
        {
            Mappings.Add(new Mapping(srcType, dstMemberName));
            return this;
        }

        class Mapping
        {
            public FullTypeNameInfo SrcType { get; private set; }
            public string DstMemberName { get; private set; }

            public Mapping(FullTypeNameInfo srcType, string dstMemberName)
            {
                SrcType = srcType;
                DstMemberName = dstMemberName;
            }
        }
    }
}