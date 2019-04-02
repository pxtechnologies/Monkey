using System;
using System.Collections.Generic;
using System.Linq;

namespace Monkey.Sql.Builder
{
    public class SqlCommandHandlerBuilder : ICodeBlockBuilder
    {
        private string _name;
        private string _nameSpace;
        private string _commandType;
        private string _resultType;
        private string _procName;
        private string _dbName;
        private bool _isCollectionResult;
        private readonly List<BindingResult> _resultBindings;
        private readonly List<BindingParameter> _parameterBindings;

        public SqlCommandHandlerBuilder BindColumnResult(string path, string propType, string sqlColumnName)
        {
            _resultBindings.Add(new BindingResult()
            {
                Path = path,
                PropertyType = propType,
                SqlColumnName = sqlColumnName
            });
            return this;
        }

        public SqlCommandHandlerBuilder BindParameter(string path, string paramName)
        {
            _parameterBindings.Add(new BindingParameter()
            {
                Path = path,
                ParameterName = paramName
            });
            return this;
        }

        public SqlCommandHandlerBuilder()
        {
            _resultBindings = new List<BindingResult>();
            _parameterBindings = new List<BindingParameter>();
        }

        public SqlCommandHandlerBuilder WithRootName(string name)
        {
            _name = name + "CommandHandler";
            _commandType = name + "Command";
            _resultType = name + "Result";
            return this;
        }

        public SqlCommandHandlerBuilder WithProcedureName(string procName, string dbName)
        {
            _dbName = dbName;
            _procName = procName;
            return this;
        }
        public SqlCommandHandlerBuilder BindSqlResultSetColumn(string columnName, string path)
        {
            return this;
        }
        public SqlCommandHandlerBuilder BindSqlParameter(string parameterName, string path)
        {
            return this;
        }

        public SqlCommandHandlerBuilder WithCommandTypeName(string commandName)
        {
            _commandType = commandName;
            return this;
        }

        public SqlCommandHandlerBuilder WithResultTypeName(string resultName)
        {
            _resultType = resultName;
            return this;
        }
        public SqlCommandHandlerBuilder WithName(string name)
        {
            this._name = name;
            return this;
        }

        public SqlCommandHandlerBuilder InNamespace(string ns)
        {
            this._nameSpace = ns;
            return this;
        }
        public void GenerateCode(SourceCodeBuilder sb)
        {
            if (!string.IsNullOrWhiteSpace(_nameSpace))
                sb.AppendLine($"namespace {_nameSpace}")
                    .OpenBlock();
            

            sb.AppendLine($"public class {_name} : ICommandHandler<{_commandType},{_resultType}");

            sb.OpenBlock();

            GenerateConsts(sb);
            GenerateExecuteMethod(sb);
            GenerateCtor(sb);

            sb.CloseBlock();

            if (!string.IsNullOrWhiteSpace(_nameSpace))
                sb.CloseBlock();
            
        }

        private void GenerateConsts(SourceCodeBuilder sb)
        {
            sb.AppendLine($"private const string _dbName = {_dbName.DblQuoted()};");
            sb.AppendLine($"private const string _procName = {_procName.DblQuoted()};");
        }

        private void GenerateCtor(SourceCodeBuilder sb)
        {
            sb.AppendLine("private IConfiguration _config;");
            sb.AppendLine();

            sb.AppendLine($"public {_name}(IConfiguration config)");
            sb.OpenBlock();
            sb.AppendLine($"this._config = config;");
            sb.CloseBlock();
        }

        private void GenerateExecuteMethod(SourceCodeBuilder sb)
        {
            string collection = _isCollectionResult ? "[]" : "";
            sb.AppendLine($"public async Task<{_resultType}{collection}> Execute({_commandType} cmd)")
                .OpenBlock();
            GenerateMethodBody(sb);
            sb.CloseBlock();
        }

        private void GenerateResult(SourceCodeBuilder sb, string objName = "result")
        {
            sb.AppendLine($"var {objName} = new {_resultType}({String.Join(",",ResultBindings.Select(x=>x.SqlColumnName.DblQuoted()))});");
            int i = 0;
            foreach (var r in this.ResultBindings)
            {
                sb.AppendLine($"int index = ix[{i++}];");
                sb.AppendLine($"if(!(await rd.IsDBNullAsync(index)))").IndentUp();
                sb.AppendLine($"{objName}.{r.Path} = rd.{_mthDict[r.PropertyType]}(index);").IndentDown();
            }
        }
        private ISqlReaderMethodDictionary _mthDict = new SqlReaderMethodDictionary();
        public IEnumerable<BindingResult> ResultBindings
        {
            get { return _resultBindings; }
            
        }

        
        private void GenerateMethodBody(SourceCodeBuilder sb)
        {
            sb.AppendLine("$using(var connection = new SqlConnection(_config.GetConnectionString(_dbName)))");
            sb.OpenBlock();

            sb.AppendLine("await connection.OpenAsync();");
            sb.AppendLine($"using(var command = connection.CreateCommand())").OpenBlock();
            sb.AppendLine($"command.CommandType = CommandType.StoredProcedure;");
            sb.AppendLine($"command.CommandText = _procName;");
            foreach (var p in this.ParameterBindings)
                sb.AppendLine($"command.AddWithValue({p.ParameterName},{p.Path});");

            sb.AppendLine($"using(var rd = await command.ExecuteReaderAsync())").OpenBlock();

            sb.AppendLine("var lz = new Lazy<int[]>(() => rd.GetIndexes(), LazyThreadSafetyMode.None);");

            if (_isCollectionResult)
            {
                sb.AppendLine($"List<{_resultType}> resultSet = new List<{_resultType}>();");
                sb.AppendLine($"while(await rd.ReadAsync())").OpenBlock();
                GenerateResult(sb);
                sb.AppendLine($"resultSet.Add(result);");

                sb.CloseBlock();
                sb.AppendLine("return resultSet;");
            }
            else
            {
                sb.AppendLine($"if(await rd.ReadAsync())").OpenBlock();
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
