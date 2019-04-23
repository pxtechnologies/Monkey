using System;
using System.Collections.Generic;
using System.Linq;
using Monkey.Builder;

namespace Monkey.Sql.Builder
{
    public abstract class HandlerBuilder<TBuilder> : ICodeBlockBuilder
        where TBuilder: HandlerBuilder<TBuilder>
    {
        protected string _name;
        protected string _nameSpace;
        
        protected string _resultType;
        protected string _procName;
        protected string _dbName;
        protected bool _isCollectionResult;
        protected readonly List<BindingResult> _resultBindings;
        protected readonly List<BindingParameter> _parameterBindings;
        protected List<string> _usingNs;

        public TBuilder BindColumnResult(string path, string propType, string sqlColumnName)
        {
            _resultBindings.Add(new BindingResult()
            {
                Path = path,
                PropertyType = propType,
                SqlColumnName = sqlColumnName
            });
            return (TBuilder)this;
        }

        public TBuilder BindParameter(string path, string paramName)
        {
            _parameterBindings.Add(new BindingParameter()
            {
                Path = path,
                ParameterName = paramName
            });
            return (TBuilder)this;
        }

        public TBuilder AddUsing(string ns)
        {
            if (!_usingNs.Contains(ns))
                _usingNs.Add(ns);
            return (TBuilder)this;
        }
        public HandlerBuilder()
        {
            _usingNs = new List<string>();
            _resultBindings = new List<BindingResult>();
            _parameterBindings = new List<BindingParameter>();
        }

        public TBuilder AddDefaultUsings()
        {
            AddUsing("System")
                .AddUsing("System.Configuration")
                .AddUsing("System.Collections.Generic")
                .AddUsing("System.Data")
                .AddUsing("System.Data.SqlClient")
                .AddUsing("System.Threading")
                .AddUsing("System.Threading.Tasks")
                .AddUsing("System.Data.SqlClient")
                .AddUsing("Monkey.Sql.Extensions")
                .AddUsing("Monkey.Sql")
                .AddUsing("Monkey.Cqrs")
                .AddUsing("Microsoft.Extensions.Configuration");

            return (TBuilder)this;
        }

        

        public TBuilder WithProcedureName(string procName, string dbName)
        {
            _dbName = dbName;
            _procName = procName;
            return (TBuilder)this;
        }

        

        protected string MergeUseOfType(string typeName)
        {
            var type = typeName.ParseType();
            if (type.IsNamespaceDefined)
            {
                if (!_usingNs.Contains(type.Namespace))
                    _usingNs.Add(type.Namespace);
                typeName = type.Name;
            }

            return typeName;
        }

        public TBuilder WithResultTypeName(string resultName)
        {
            _resultType = MergeUseOfType(resultName);
            return (TBuilder)this;
        }
        public TBuilder WithName(string name)
        {
            this._name = name;
            return (TBuilder)this;
        }

        public TBuilder InNamespace(string ns)
        {
            this._nameSpace = ns;
            return (TBuilder)this;
        }

        protected virtual string GetHandlerInterfaceName()
        {
            return "IHandler";
        }

        public void GenerateCode(SourceCodeBuilder sb)
        {
            Validate();
            if (!string.IsNullOrWhiteSpace(_nameSpace))
                sb.AppendLine($"namespace {_nameSpace}")
                    .OpenBlock();

            if (_usingNs.Any())
                foreach (var ns in _usingNs)
                    sb.AppendLine($"using {ns};");
            sb.AppendLine();

            sb.AppendLine($"public class {_name} : {GetHandlerInterfaceName()}");

            sb.OpenBlock();

            GenerateConsts(sb);
            GenerateCtor(sb);
            GenerateExecuteMethod(sb);

            sb.CloseBlock();

            if (!string.IsNullOrWhiteSpace(_nameSpace))
                sb.CloseBlock();

        }

        protected virtual void Validate()
        {
            if (string.IsNullOrWhiteSpace(_name)) throw new ArgumentException("Name");
            if (string.IsNullOrWhiteSpace(_resultType)) throw new ArgumentException("Result Type");
            if (string.IsNullOrWhiteSpace(_procName)) throw new ArgumentException("Procedure Name");
            if (string.IsNullOrWhiteSpace(_dbName)) throw new ArgumentException("DbName");

        }

        protected virtual void GenerateConsts(SourceCodeBuilder sb)
        {
            sb.AppendLine($"private const string _dbName = {_dbName.DblQuoted()};");
            sb.AppendLine($"private const string _procName = {_procName.DblQuoted()};");
        }

        protected virtual void GenerateCtor(SourceCodeBuilder sb)
        {
            sb.AppendLine("private IConfiguration _config;");
            sb.AppendLine();

            sb.AppendLine($"public {_name}(IConfiguration config)");
            sb.OpenBlock();
            sb.AppendLine($"this._config = config;");
            sb.CloseBlock();
        }

        protected abstract void GenerateExecuteMethod(SourceCodeBuilder sb);
        

        protected virtual void GenerateResult(SourceCodeBuilder sb, string objName = "result")
        {
            sb.AppendLine($"var {objName} = new {_resultType}();");
            int i = 0;
            foreach (var r in this.ResultBindings)
            {
                sb.AppendLine($"if(!(await rd.IsDBNullAsync(ix[{i}])))").IndentUp();
                sb.AppendLine($"{objName}.{r.Path} = rd.{_mthDict[r.PropertyType.GetPrimitiveType()]}(ix[{i++}]);").IndentDown();
            }
        }
        protected ISqlReaderMethodDictionary _mthDict = new SqlReaderMethodDictionary();
        public IEnumerable<BindingResult> ResultBindings
        {
            get { return _resultBindings; }

        }

        protected virtual string MethodArg() => "cmd";
        protected void GenerateMethodBody(SourceCodeBuilder sb)
        {
            sb.AppendLine($"using(var connection = new SqlConnection(_config.GetConnectionString(_dbName)))");
            sb.OpenBlock();

            sb.AppendLine("await connection.OpenAsync();");
            sb.AppendLine($"using(var command = connection.CreateCommand())").OpenBlock();
            sb.AppendLine($"command.CommandType = CommandType.StoredProcedure;");
            sb.AppendLine($"command.CommandText = _procName;");
            foreach (var p in this.ParameterBindings)
                sb.AppendLine($"command.Parameters.AddWithValue({p.ParameterName.DblQuoted()}, {MethodArg()}.{p.Path});");
            sb.AppendLine();

            sb.AppendLine($"using(var rd = await command.ExecuteReaderAsync())").OpenBlock();

            sb.AppendLine($"var lz = new Lazy<int[]>(() => rd.GetIndexes({String.Join(", ", ResultBindings.Select(x => x.SqlColumnName.DblQuoted()))}), LazyThreadSafetyMode.None);");

            if (_isCollectionResult)
            {
                sb.AppendLine($"List<{_resultType}> resultSet = new List<{_resultType}>();");
                sb.AppendLine($"while(await rd.ReadAsync())").OpenBlock();
                sb.AppendLine($"var ix = lz.Value;");
                GenerateResult(sb);
                sb.AppendLine($"resultSet.Add(result);");

                sb.CloseBlock();
                sb.AppendLine("return resultSet.ToArray();");
            }
            else
            {
                sb.AppendLine($"if(await rd.ReadAsync())").OpenBlock();
                sb.AppendLine($"var ix = lz.Value;");
                GenerateResult(sb);
                sb.AppendLine("return result;");

                sb.CloseBlock();
                sb.AppendLine("return null;");
            }

            sb.CloseBlock();
            sb.CloseBlock();
            sb.CloseBlock();
        }

        public IEnumerable<BindingParameter> ParameterBindings
        {
            get { return _parameterBindings; }

        }
    }
}