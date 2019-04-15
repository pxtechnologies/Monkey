using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Monkey.Builder;
using Xunit;

namespace Monkey.UnitTests
{
    public class FullTypeNameInfoTests 
    {
        [Fact]
        public void ParseNameOnly()
        {
            FullTypeNameInfo actual = "string".ParseType();

            actual.Name.Should().Be("string");
            actual.IsNamespaceDefined.Should().BeFalse();
        }

        [Fact]
        public void ParseFullName()
        {
            FullTypeNameInfo actual = "System.String".ParseType();

            actual.Name.Should().Be("String");
            actual.IsNamespaceDefined.Should().BeTrue();
            actual.Namespace.Should().Be("System");
        }
        
        [Fact]
        public void ParseCSharpName()
        {
            FullTypeNameInfo actual = "System.Nullable<System.String>";

            actual.Name.Should().Be("Nullable");
            actual.IsGeneric.Should().BeTrue();
            actual.IsNullable.Should().BeTrue();
            actual.Namespace.Should().Be("System");
            actual.GenericArguments.First().Name.Should().Be("String");
            actual.GenericArguments.First().Namespace.Should().Be("System");
        }

        [Fact]
        public void ToStringAsCSharpCodeOnlyName()
        {
            var type = "System.Nullable<System.String>";

            FullTypeNameInfo fullTypeInfo = type;

            string actual = fullTypeInfo.ToString(false, false);

            actual.Should().Be("Nullable<string>");
        }
        [Fact]
        public void ToStringAsCSharpCode()
        {
            var type = "System.Nullable<System.String>";

            FullTypeNameInfo fullTypeInfo = type;

            string actual = fullTypeInfo.ToString(false, true);

            actual.Should().Be(type);
        }
        [Fact]
        public void ToStringAsCSharpCodeShortOnlyName()
        {
            var type = "System.Nullable<System.String>";

            FullTypeNameInfo fullTypeInfo = type;

            string actual = fullTypeInfo.ToString(true, false);

            actual.Should().Be("string?");
        }
        [Fact]
        public void ToStringAsCSharpCodeShort()
        {
            var type = "System.Nullable<System.String>";

            FullTypeNameInfo fullTypeInfo = type;

            string actual = fullTypeInfo.ToString(true, true);

            actual.Should().Be("System.String?");
        }
        [Fact]
        public void ParseGenericsWithManyArgs()
        {
            FullTypeNameInfo actual = FullTypeNameInfo.Parse(typeof(Dictionary<string, int>).FullName);

            actual.Name.Should().Be("Dictionary");
            actual.Namespace.Should().Be("System.Collections.Generic");
            actual.IsGeneric.Should().BeTrue();
            actual.GenericArguments.First().Name.Should().Be("String");
            actual.GenericArguments.First().Namespace.Should().Be("System");
            actual.GenericArguments.Last().Name.Should().Be("Int32");
            actual.GenericArguments.Last().Namespace.Should().Be("System");
        }

        [Fact]
        public void ParseAssemblyFullName()
        {
            FullTypeNameInfo actual = typeof(string).AssemblyQualifiedName.ParseType();

            actual.Name.Should().Be("String");
            actual.IsNamespaceDefined.Should().BeTrue();
            actual.Namespace.Should().Be("System");
        }
        [Fact]
        public void ParseAssemblyGenericFullName()
        {
            FullTypeNameInfo actual = typeof(List<string>).AssemblyQualifiedName.ParseType();

            actual.Name.Should().Be("List");
            actual.IsNamespaceDefined.Should().BeTrue();
            actual.Namespace.Should().Be("System.Collections.Generic");
            var fArg = actual.GenericArguments.First();
            fArg.Name.Should().Be("String");
            fArg.Namespace.Should().Be("System");
        }

        [Fact]
        public void ParseDetectsGenerics()
        {
            FullTypeNameInfo actual = typeof(Nullable<int>).FullName.ParseType();

            actual.IsGeneric.Should().BeTrue();
        }
        [Fact]
        public void ParseDetectsNestedGenerics()
        {
            FullTypeNameInfo actual = typeof(List<List<string>>).FullName.ParseType();

            actual.Name.Should().Be("List");
            actual.IsNamespaceDefined.Should().BeTrue();
            actual.Namespace.Should().Be("System.Collections.Generic");

            actual = actual.GenericArguments.First();
            actual.IsNamespaceDefined.Should().BeTrue();
            actual.Name.Should().Be("List");
            actual.Namespace.Should().Be("System.Collections.Generic");

            actual = actual.GenericArguments.First();
            actual.IsNamespaceDefined.Should().BeTrue();
            actual.Name.Should().Be("String");
            actual.Namespace.Should().Be("System");
        }
        [Fact]
        public void ParseDetectsNameOfGenerics()
        {
            FullTypeNameInfo actual = typeof(Nullable<int>).FullName.ParseType();

            actual.Name.Should().Be("Nullable");
        }
        [Fact]
        public void ParseDetectsArgsOfGenerics()
        {
            FullTypeNameInfo actual = typeof(Nullable<int>).FullName.ParseType();

            actual.GenericArguments.First().Name.Should().Be("Int32");
        }
    }
}