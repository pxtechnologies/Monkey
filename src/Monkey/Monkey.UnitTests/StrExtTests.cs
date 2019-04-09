using System;
using FluentAssertions;
using Xunit;
using Monkey;
using Monkey.Patterns.UnitTests;

namespace Monkey.UnitTests
{
    public class StrExtTests
    {
        [Fact]
        public void ToWords()
        {
            string text = "UserControllerCommand";

            var items = text.ToWords();

            items.Should().BeEquivalentTo("User", "Controller", "Command");
        }
        [Fact]
        public void ToWordsOneArg()
        {
            string text = "User";

            var items = text.ToWords();

            items.Should().BeEquivalentTo("User");
        }
        [Fact]
        public void ToWordsNone()
        {
            string text = "";

            var items = text.ToWords();

            items.Should().BeEquivalentTo("");
        }

        [Fact]
        public void EndsWithSingleSuffix()
        {
            string text = "AddUserCommand";

            var actual = text.EndsWithSingleSuffix("Request", "Command");

            actual.Should().Be("AddUserRequest");
        }
        [Fact]
        public void EndsWithSingleSuffixWithDuplication()
        {
            string text = "AddUserRequest";

            var actual = text.EndsWithSingleSuffix("Request");

            actual.Should().Be("AddUserRequest");
        }

        [Fact]
        public void DblQuoted()
        {
            var text = "Hello".DblQuoted();

            text.Should().Be("\"Hello\"");
        }

        [Fact]
        public void FindTypeName()
        {
            var fullType = "Monkey.CreateUserCommand";

            var ns = fullType.FindTypeName();

            ns.Should().Be("CreateUserCommand");
        }
        [Fact]
        public void FindNamespace()
        {
            var fullType = "Monkey.CreateUserCommand";

            var ns = fullType.FindNamespace();

            ns.Should().Be("Monkey");
        }
        [Fact]
        public void RemoveSuffixWords()
        {
            string text = "AddUserRequest";

            var actual = text.RemoveSuffixWords("Request", "Command");

            actual.Should().Be("AddUser");
        }
    }
}