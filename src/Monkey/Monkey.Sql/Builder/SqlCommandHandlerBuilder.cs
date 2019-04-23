using System;
using Microsoft.Extensions.Options;
using Monkey.Builder;

namespace Monkey.Sql.Builder
{
    public sealed class SqlCommandHandlerBuilder : HandlerBuilder<SqlCommandHandlerBuilder>
    {
        string _commandType;
        public SqlCommandHandlerBuilder WithRootName(string name)
        {
            _name = name + "CommandHandler";
            _commandType = name + "Command";
            _resultType = name + "Result";
            return (SqlCommandHandlerBuilder)this;
        }
        public SqlCommandHandlerBuilder WithCommandTypeName(string commandName)
        {
            _commandType = MergeUseOfType(commandName);
            return (SqlCommandHandlerBuilder)this;
        }
        protected override void GenerateExecuteMethod(SourceCodeBuilder sb)
        {
            string collection = _isCollectionResult ? "[]" : "";
            sb.AppendLine($"public async Task<{_resultType}{collection}> Execute({_commandType} cmd)")
                .OpenBlock();
            GenerateMethodBody(sb);
            sb.CloseBlock();
        }
        protected override void Validate()
        {
            base.Validate();
            if (string.IsNullOrWhiteSpace(_commandType)) throw new ArgumentException("Command Type");
        }

        protected override string GetHandlerInterfaceName()
        {
            return $"ICommandHandler<{_commandType}, {_resultType}>";
        }
    }
}
