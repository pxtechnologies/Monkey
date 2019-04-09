using System;
using System.Collections.Generic;
using System.Linq;
using Monkey.Builder;
using Monkey.Generator;
using Monkey.Logging;
using Monkey.WebApi.Builder;

namespace Monkey.WebApi.Generator
{
    public interface ICqrsControllerGenerator
    {
        IEnumerable<SourceUnit> Generate(ServiceInfo service);
    }
    public class CqrsControllerGenerator : ICqrsControllerGenerator
    {
        class ControllerSourceUnit
        {
            public SourceUnitCollection Requests { get; private set; }
            public SourceUnitCollection Responses { get; private set; }
            public SourceUnitCollection RequestToCommandMappers { get; private set; }
            public SourceUnitCollection ResultToResponseMappers { get; private set; }
            public SourceUnit Controller { get; set; }

            public ControllerSourceUnit()
            {
                Requests = new SourceUnitCollection();
                RequestToCommandMappers = new SourceUnitCollection();
                Responses = new SourceUnitCollection();
                ResultToResponseMappers = new SourceUnitCollection();
            }

            public IEnumerable<SourceUnit> GetAllUnits()
            {
                var result = new SourceUnitCollection();
                result.Append(Controller);
                result.Append(Requests);
                result.Append(RequestToCommandMappers);
                result.Append(Responses);
                result.Append(ResultToResponseMappers);
                return result;
            }
        }
        private readonly ILogger _logger;

        public CqrsControllerGenerator(ILogger logger)
        {
            _logger = logger;
        }

        public IEnumerable<SourceUnit> Generate(ServiceInfo service)
        {
            if (service.IsValid)
            {
                ControllerSourceUnit unit = new ControllerSourceUnit();

                CqrsControllerBuilder builder = new CqrsControllerBuilder()
                    .WithName(service.Name)
                    .InNamespace(service.Assembly.GetName().Name + ".WebApi");

                foreach (HandlerInfo h in service.Handlers)
                {
                    // we need to construct:
                    // arguments
                    // Automapper configuration
                    // TODO: Refactor and write UT
                    if (h.IsCommandHandler)
                    {
                        GenerateWriteAction(unit, h, builder);
                    }
                    else if (h.IsQueryHandler)
                    {

                    }
                }
                unit.Controller = new SourceUnit(builder.Namespace, builder.TypeName, builder.ToString());
                return unit.GetAllUnits();

            }
            else _logger.Warn("Cannot generate controller for service {serviceName}", service.Name);
            return new SourceUnit[0];
        }

        private static void GenerateWriteAction(ControllerSourceUnit srcUnit, HandlerInfo handler, CqrsControllerBuilder builder)
        {
            if (handler.RequestType.GetProperties().Any(x => x.Name == "Id" || x.Name == $"{handler.Service.Name}Id"))
            {
                var prop = handler.RequestType.GetProperties()
                    .First(x => x.Name == "Id" || x.Name == $"{handler.Service.Name}Id");

                ArgumentCollection arguments = new ArgumentCollection();

                var idArg = arguments.Add(prop.PropertyType.ToString(), prop.Name.StartLower(), "FromUrl");
                var cmdArg = arguments.Add(handler.RequestType.Name.EndsWithSingleSuffix("Request", "Command"), "request", "FromBody");
                // this is update action
                // lets check if this put or post. 
                // Put assumes that we have command starting with name "Update" or "Modify"
                if (handler.RequestType.Name.StartsWithPrefixes("Update", "Modify"))
                {
                    // this is pure PUT
                    // Id is taken from command.id
                    // ResultResponse Put(int id, updaterequest request);

                    var responseType = handler.ResponseType.Name.EndsWithSingleSuffix("Response");
                    builder.AppendAction(handler, "Put", responseType, HttpVerb.Put,
                        false, $"api/{handler.Service.Name}/{{{idArg.Name}}}", arguments.ToArray());
                    srcUnit.Responses.Append(GenerateResponseType(responseType, handler.ResponseType));
                    srcUnit.ResultToResponseMappers.Append(GenerateResultToResponseMapper(responseType,handler.RequestType));
                    srcUnit.Requests.Append(GenerateRequestType(cmdArg.Type, handler.RequestType, prop.PropertyType, prop.Name));
                    srcUnit.RequestToCommandMappers.Append(GenerateRequestToCommandMapper(cmdArg.Type, handler.RequestType, prop.PropertyType, prop.Name));
                }
                else
                {
                    // This is custom method
                    var responseType = handler.ResponseType.Name.EndsWithSingleSuffix("Response");
                    string name = handler.RequestType.Name.RemoveSuffixWords("Command").RemoveWords(handler.Service.Name);
                    builder.AppendAction(handler, name, responseType,
                        HttpVerb.Post, handler.IsResponseCollection, $"api/{handler.Service.Name}/{{{idArg.Name}}}/{name}",
                        arguments.ToArray());

                    srcUnit.Responses.Append(GenerateResponseType(responseType, handler.ResponseType));
                    srcUnit.ResultToResponseMappers.Append(GenerateResultToResponseMapper(responseType, handler.RequestType));
                    srcUnit.Requests.Append(GenerateRequestType(cmdArg.Type, handler.RequestType, prop.PropertyType, prop.Name));
                    srcUnit.RequestToCommandMappers.Append(GenerateRequestToCommandMapper(cmdArg.Type, handler.RequestType, prop.PropertyType, prop.Name));
                }
            }
            else
            {
                ArgumentCollection arguments = new ArgumentCollection();
                var cmdArg = arguments.Add(handler.RequestType.Name.EndsWithSingleSuffix("Request", "Command"), "request", "FromBody");
                // this might be create method
                if (handler.RequestType.Name.StartsWithPrefixes("Create", "Insert"))
                {
                    var responseType = handler.ResponseType.Name.EndsWithSingleSuffix("Response");
                    builder.AppendAction(handler, "Post", responseType,
                        HttpVerb.Post, handler.IsResponseCollection, $"api/{handler.Service.Name}",
                        arguments.ToArray());

                    srcUnit.Responses.Append(GenerateResponseType(responseType, handler.ResponseType));
                    srcUnit.ResultToResponseMappers.Append(GenerateResultToResponseMapper(responseType, handler.RequestType));
                    srcUnit.Requests.Append(GenerateRequestType(cmdArg.Type, handler.RequestType));
                    srcUnit.RequestToCommandMappers.Append(GenerateRequestToCommandMapper(cmdArg.Type, handler.RequestType));
                }
                else
                {
                    // This is unkown method.
                    var responseType = handler.ResponseType.Name.EndsWithSingleSuffix("Response");
                    string name = handler.RequestType.Name.RemoveSuffixWords("Command").RemoveWords(handler.Service.Name);
                    builder.AppendAction(handler, name, responseType,
                        HttpVerb.Post, handler.IsResponseCollection, $"api/{handler.Service.Name}/{name}",
                        arguments.ToArray());


                    srcUnit.Responses.Append(GenerateResponseType(responseType, handler.ResponseType));
                    srcUnit.ResultToResponseMappers.Append(GenerateResultToResponseMapper(responseType, handler.RequestType));
                    srcUnit.Requests.Append(GenerateRequestType(cmdArg.Type, handler.RequestType));
                    srcUnit.RequestToCommandMappers.Append(GenerateRequestToCommandMapper(cmdArg.Type, handler.RequestType));
                }
            }
        }

        private static SourceUnit GenerateRequestToCommandMapper(string requestType, Type commandType, Type exPropType, string exPropName)
        {
            AutomapperProfilerBuilder profile = new AutomapperProfilerBuilder()
                .InNamespace(commandType.Namespace + ".WebApi.Profiles")
                .ForType(requestType, commandType.Name);

            profile.WithDefaultMapping()
                .WithValueMapping(exPropType.Name, exPropName);
            
            return new SourceUnit(profile.Namespace, profile.Name, profile.GenerateCode());
        }
        private static SourceUnit GenerateRequestToCommandMapper(string requestType, Type commandType)
        {
            AutomapperProfilerBuilder profile = new AutomapperProfilerBuilder()
                .InNamespace(commandType.Namespace + ".WebApi.Profiles")
                .ForType(requestType, commandType.Name);

            profile.WithDefaultMapping();

            return new SourceUnit(profile.Namespace, profile.Name, profile.GenerateCode());
        }
        private static SourceUnit GenerateRequestType(string requestTypeName, Type commandType)
        {
            DataClassBuilder builder = new DataClassBuilder()
                .WithName(requestTypeName)
                .InNamespace(commandType.Namespace + ".WebApi");

            foreach (var p in commandType.GetProperties())
            {
                builder.WithProperty(p.PropertyType.Name, p.Name);
            }

            var src = builder.GenerateCode();
            return new SourceUnit(builder.Namespace, builder.Name, src);
        }
        private static SourceUnit GenerateRequestType(string requestTypeName, Type commandType, Type exPropType, string exPropName)
        {
            DataClassBuilder builder = new DataClassBuilder()
                .WithName(requestTypeName)
                .InNamespace(commandType.Namespace + ".WebApi");

            foreach (var p in commandType.GetProperties().Where(x=>x.Name != exPropName))
            {
                builder.WithProperty(p.PropertyType.Name, p.Name);
            }

            var src = builder.GenerateCode();
            return new SourceUnit(builder.Namespace, builder.Name, src);
        }

        private static SourceUnit GenerateResultToResponseMapper(string responseTypeName, Type resultType)
        {
            AutomapperProfilerBuilder profile = new AutomapperProfilerBuilder()
                .InNamespace(resultType.Namespace + ".WebApi.Profiles")
                .ForType(responseTypeName, resultType.Name);

            profile.WithDefaultMapping();

            return new SourceUnit(profile.Namespace, profile.Name, profile.GenerateCode());
        }

        private static SourceUnit GenerateResponseType(string responseTypeName, Type responseType)
        {
            DataClassBuilder builder = new DataClassBuilder()
                .WithName(responseTypeName)
                .InNamespace(responseType.Namespace + ".WebApi");

            foreach (var p in responseType.GetProperties())
            {
                builder.WithProperty(p.PropertyType.Name, p.Name);
            }

            var src = builder.GenerateCode();
            return new SourceUnit(builder.Namespace, builder.Name, src);
        }
    }
}