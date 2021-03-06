﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Monkey.Builder;
using Monkey.Compilation;
using Monkey.Cqrs;
using Monkey.Generator;
using Monkey.PubSub;
using Monkey.WebApi.AcceptanceTests.Assertion;
using Monkey.WebApi.AcceptanceTests.Configuration;
using Monkey.WebApi.AcceptanceTests.Integration.Bindings;
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
        [Given(@"I have written command-handlers as follows:")]
        public void GivenIHaveWrittenCommand_HandlersAsFollows(Table table)
        {
            var hArgs = table.CreateSet<HandlerArgs>();
            Type[] handlers = hArgs.Select(x =>
                    typeof(ICommandHandler<,>).MakeGenericType(_data.Types[x.CommandType], _data.Types[x.ResultType]))
                .ToArray();

            var assemblies = _applicationExecutor.Execute<MockRegister, Assembly[]>(register =>
            {
                return handlers.Select(i => register.GetMock(i).GetType().Assembly).Distinct().ToArray();
            });

            _applicationExecutor.Execute<IServiceMetadataProvider>(provider =>
                provider.Discover(x => handlers.Contains(x.HandlerIType), assemblies));

            foreach (var h in hArgs)
            {
                var ret = "new UserEntity() { Name = \"Elton\" }";
                MockHandlerBuilder assertBuilder = new MockHandlerBuilder()
                    .With(_data.Types[h.CommandType], _data.Types[h.ResultType])
                    .WithReturn(ret);

                _applicationExecutor.InvokeDynamic(assertBuilder);
            }
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
        public void GivenIHaveWrittenCommandAndResultAs(string commandTypes, Table table)
        {
            var lines = table.CreateSet<CSharpCode>().ToArray();
            SourceCodeBuilder sb = new SourceCodeBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("namespace Test").OpenBlock();
            sb.AppendLines(lines.Select(x => x.Code)).CloseBlock();
            
            TypeCompiler compiler = new TypeCompiler();
            var typeAssembly = compiler.FastLoad(sb.ToString());

            foreach(var commandType in commandTypes.Split(','))
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
            ServiceInfo[] services = this._applicationExecutor.Execute<IServiceMetadataProvider, IEnumerable<ServiceInfo>>(x => x.GetServices()).ToArray();
            var result = await this._applicationExecutor.ExecuteAsync<IWebApiGenerator, SourceUnitCollection>(async x => x.Generate(services));
            this._applicationExecutor.Execute<IDynamicTypePool>(pool =>
            {
                if (pool.CanMerge)
                    assembly = pool.Merge(result);
                else
                {
                    assembly = new DynamicAssembly(NSubstitute.Substitute.For<IEventHub>());
                    pool.AddOrReplace(assembly);
                    assembly.AppendSourceUnits(result);
                    assembly.AddWebApiReferences();
                    assembly.AddReferenceFromTypes(_data.Types.Values);
                    assembly.Compile();
                }
            });
            

            var controllerType = assembly.Load(assembly.SourceUnits.Single(x => x.FullName.Contains("Controller")).FullName);
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
