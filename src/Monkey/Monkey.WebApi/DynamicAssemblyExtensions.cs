using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Monkey.Generator;
using Monkey.PubSub;

namespace Monkey.WebApi
{
    public static class EventHubExtensions
    {
        public static IEventHub WireWebApiEvents(this IEventHub hub)
        {
            hub.WireEvents(typeof(EventHubExtensions).Assembly);
            return hub;
        }
    }
    public static class DynamicAssemblyExtensions
    {
        public static DynamicAssembly AddWebApiReferences(this DynamicAssembly a)
        {
            a.AddReferenceFromType<Profile>();
            a.AddReferenceFromType<ControllerBase>();
            a.Purpose = AssemblyPurpose.RequestProfiles |
                        AssemblyPurpose.ResponseProfiles |
                        AssemblyPurpose.Controllers |
                        AssemblyPurpose.Responses |
                        AssemblyPurpose.Requests;
            return a;
        }
    }
}