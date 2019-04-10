using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Monkey.Builder;
using Monkey.Compilation;
using Monkey.Cqrs;
using Monkey.Generator;
using Monkey.WebApi.AcceptanceTests.Assertion;
using Monkey.WebApi.AcceptanceTests.Configuration;
using Monkey.WebApi.Generator;
using Monkey.WebApi.SimpleInjector;
using Newtonsoft.Json;
using NSubstitute;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Monkey.WebApi.AcceptanceTests.Integration
{
    [Binding]
    public class ControllerInvocationSteps
    {
        private readonly ControllerInvocationDataHolder _data;
        private readonly IApplicationExecutor _applicationExecutor;

        public ControllerInvocationSteps(ControllerInvocationDataHolder data, IApplicationExecutor applicationExecutor)
        {
            _data = data;
            _applicationExecutor = applicationExecutor;
        }
        [Given(@"I have written command-handler that accepts '(.*)' and returns '(.*)'")]
        public void GivenIHaveWrittenCommand_HandlerThatAcceptsAndReturns(string commandType, string resultType)
        {
            _data.CommandHandlerInterface =
                typeof(ICommandHandler<,>).MakeGenericType(_data.Types[commandType], _data.Types[resultType]);
            
            var mockInstance = _applicationExecutor.Execute<MockRegister,object>(register => register.GetMock(_data.CommandHandlerInterface) );
            _applicationExecutor.Execute<IServiceMetadataProvider>(provider =>
                provider.Discover(x => x.HandlerIType == _data.CommandHandlerInterface, mockInstance.GetType().Assembly));

            var ret = "new UserEntity() { Name = \"Elton\" }";
            MockHandlerBuilder assertBuilder = new MockHandlerBuilder()
                .With(_data.Types[commandType], _data.Types["UserEntity"])
                .WithReturn(ret);

            _applicationExecutor.InvokeDynamic(assertBuilder);
        }

        [Given(@"I have written command '(.*)' and result as:")]
        public void GivenIHaveWrittenCommandAndResultAs(string commandType, Table table)
        {
            var lines = table.CreateSet<CSharpCode>().ToArray();
            SourceCodeBuilder sb = new SourceCodeBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("namespace Test").OpenBlock();
            sb.AppendLines(lines.Select(x => x.Code)).CloseBlock();
            
            TypeCompiler compiler = new TypeCompiler();
            var typeAssembly = compiler.FastLoad(sb.ToString());

            _data.Types[commandType] = typeAssembly.GetType($"Test.{commandType}");
            _data.Types["UserEntity"] = typeAssembly.GetType("Test.UserEntity");
        }
        [When(@"I found record with id to update")]
        public void WhenIFoundRecordWithIdToUpdate()
        {
            _data.Id = Guid.NewGuid();
        }


        [When(@"I invoke '(.*)' with '(.*)' method and '(.*)' argument:")]
        public async Task WhenIInvokeWithMethodAndArgument(string controllerTypeName, string methodName, string requestType, Table table)
        {
            var json = string.Join(Environment.NewLine, table.CreateSet<JsonScript>().Select(x => x.Json));

            DynamicAssembly assembly = null;
            var result = await this._applicationExecutor.ExecuteAsync<ISourceCodeGenerator, IEnumerable<SourceUnit>>(async x => (await x.Generate()).ToArray());
            this._applicationExecutor.Execute<IDynamicTypePool>(pool =>
            {
                assembly = new DynamicAssembly();
                pool.Add(assembly);
                assembly.AppendSourceUnits(result);
                assembly.AddWebApiReferences();
                assembly.AddReferenceFromTypes(_data.Types.Values);
                assembly.Compile();
            });
            

            var controllerType = assembly.Load(assembly.SourceUnits.First(x => x.FullName.Contains("Controller")).FullName);
            _applicationExecutor.Execute(controllerType, controller =>
            {
                var createUserObj = JsonConvert.DeserializeObject(json, assembly.Load("Test.WebApi", requestType));
                var parameters = new List<object>() { createUserObj };
                if(_data.Id != Guid.Empty) 
                    parameters.Insert(0, _data.Id);
                Task task = (Task)controllerType.GetMethod(methodName).Invoke(controller, parameters.ToArray());
                task.GetAwaiter().GetResult();
                dynamic dTask = task;
                var response = dTask.Result;
                _data.ActualResult = response;
                
            });
        }
        [Then(@"CommandHandler is invoked with corresponding '(.*)' argument")]
        public void ThenCommandHandlerIsInvokedWithCorrespondingArgument(string commandTypeName)
        {
            MockHandlerBuilder assertBuilder = new MockHandlerBuilder()
                .With(_data.Types[commandTypeName], _data.Types["UserEntity"])
                .WithArg($"Arg.Is<{commandTypeName}>(x => x.Name == \"John\" )");

            _applicationExecutor.InvokeDynamic(assertBuilder);
        }

        [Then(@"'(.*)' that corresponds to '(.*)' is returned")]
        public void ThenThatCorrespondsToIsReturned(string responseType, string resultType)
        {
            _data.ActualResult.GetType().Name.Should().Be(responseType);
            dynamic n = _data.ActualResult;
            string name = n.Name;
            name.Should().Be("Elton");
        }

    }
}
