using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace Monkey.WebApi.AcceptanceTests.Features
{
    [Binding]
    public class CommandHandlerSteps
    {
        [Given(@"I have written '(.*)' that accepts '(.*)' and returns '(.*)' in '(.*)' namespace")]
        public void GivenIHaveWrittenThatAcceptsAndReturnsInNamespace(string p0, string p1, string p2, string p3)
        {
            ScenarioContext.Current.Pending();
        }
        [Given(@"I have written '(.*)' with properties")]
        public void GivenIHaveWrittenWithProperties(string p0, Table table)
        {
            ScenarioContext.Current.Pending();
        }

    }
    public class WebAppSteps
    {
        [When(@"I run the application")]
        public void WhenIRunTheApplication()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
