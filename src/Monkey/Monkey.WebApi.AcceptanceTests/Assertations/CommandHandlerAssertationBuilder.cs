using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Monkey.Builder;
using Monkey.Compilation;
using Monkey.Cqrs;
using Monkey.WebApi.AcceptanceTests.Integration;

namespace Monkey.WebApi.AcceptanceTests.Assertion
{
    
    public class MockHandlerBuilder
    {
        public Type RequestType { get; private set; }
        public Type ResponseType { get; private set; }
        public string ReceivedStatement { get; private set; }
        public string ReturnsStatement { get; private set; }
        public string ArgStatement { get; private set; }
        public bool IsAssertion { get; private set; }
        
        public MockHandlerBuilder WithArg(string stm)
        {
            ArgStatement = stm;
            return this;
        }

        public MockHandlerBuilder WithReceive(string stm = null)
        {
            ReceivedStatement = stm;
            return this;
        }
        public MockHandlerBuilder With(Type request, Type response)
        {
            RequestType = request;
            ResponseType = response;
            IsAssertion = true;
            return this;
        }

        public MockHandlerBuilder WithReturn(string stm = null)
        {
            ReturnsStatement = stm;
            IsAssertion = false;
            return this;
        }
        public Type CreateDynamicAssertType()
        {
            if (ReceivedStatement == null) ReceivedStatement = "Received(1)";
            if (ArgStatement == null) ArgStatement = $"Arg.Any<{RequestType.Name}>()";
            if (ReturnsStatement == null) ReturnsStatement = $"new {ResponseType.Name}()";

            SourceCodeBuilder code = new SourceCodeBuilder();
            code.AppendLine("using System;");
            code.AppendLine("using System.Threading.Tasks;");
            code.AppendLine($"using {typeof(IDynamicMock).Namespace};");
            code.AppendLine($"using {typeof(NSubstitute.Substitute).Namespace};");
            code.AppendLine($"using {typeof(ICommandHandler<,>).Namespace};");
            code.AppendLine($"using {typeof(MockRegister).Namespace};");
            
            code.AppendLine($"using {RequestType.Namespace};");
            code.AppendLine("namespace DynamicUtils").OpenBlock();
            code.AppendLine($"public class DynamicMock : {typeof(IDynamicMock).Name}").OpenBlock();
            code.AppendLine("private MockRegister _register;");
            code.AppendLine($"public async Task Execute()").OpenBlock();
            var handler = $"_register.GetHandler<{RequestType.Name},{ResponseType.Name}>()";
            if (IsAssertion)
                code.AppendLine($"await {handler}.{ReceivedStatement}.Execute({ArgStatement});");
            else
                code.AppendLine($"{handler}.Execute({ArgStatement}).Returns({ReturnsStatement});");

            code.CloseBlock();
            code.AppendLine($"public DynamicMock(MockRegister register)").OpenBlock();
            code.AppendLine("this._register = register;").CloseBlock();
            code.CloseBlock().CloseBlock();

            TypeCompiler compiler = new TypeCompiler();
            var assertationAssembly = compiler.FastLoad(code.ToString(),
                RequestType.Assembly,
                typeof(ValueTask<>).Assembly,
                Assembly.Load("System.Threading.Tasks.Extensions, Version=4.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"));
            return assertationAssembly.GetType("DynamicUtils.DynamicMock");
        }
    }
}
