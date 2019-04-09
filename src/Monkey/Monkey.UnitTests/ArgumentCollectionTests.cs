using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Monkey.Builder;
using Monkey.Patterns.UnitTests;
using Xunit;

namespace Monkey.UnitTests
{
    public class ArgumentCollectionTests : TestFor<ArgumentCollection>
    {
        protected override ArgumentCollection CreateSut()
        {
            return  new ArgumentCollection();
        }

        [Fact]
        public void ToDeclaration()
        {
            Sut.Add("string", "name");
            Sut.Add("string", "lastName");

            var actual = Sut.ToString();

            actual.Should().Be("string name, string lastName");
        }
    }
}
