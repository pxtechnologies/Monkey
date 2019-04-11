using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Monkey.Generator;

namespace Monkey.WebApi
{
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