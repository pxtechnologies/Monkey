using System;
using Monkey.Builder;

namespace Monkey.Sql.Builder
{
    public class SqlQueryHandlerBuilder : HandlerBuilder<SqlQueryHandlerBuilder>
    {
        string _queryType;
        protected override void GenerateExecuteMethod(SourceCodeBuilder sb)
        {
            string collection = _isCollectionResult ? "[]" : "";
            sb.AppendLine($"public async Task<{_resultType}{collection}> Execute({_queryType} query)")
                .OpenBlock();
            GenerateMethodBody(sb);
            sb.CloseBlock();
        }

        public SqlQueryHandlerBuilder()
        {
            _isCollectionResult = true;
        }
        protected override string GetHandlerInterfaceName()
        {
            return $"IQueryHandler<{_queryType},{_resultType}>";
        }
        protected override string MethodArg()
        {
            return "query";
        }

        public SqlQueryHandlerBuilder WithQueryTypeName(string name)
        {
            _queryType = name;
            return this;
        }
        public SqlQueryHandlerBuilder WithRootName(string name)
        {
            _name = name + "QueryHandler";
            _queryType = name + "Query";
            _resultType = name + "Result";
            return this;
        }
        protected override void Validate()
        {
            base.Validate();
            if (string.IsNullOrWhiteSpace(_queryType))
                throw new ArgumentException("Query Type");
        }
    }
}