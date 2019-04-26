﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Monkey.Sql.WebApiHost.AcceptanceTests.Configuration;
using TechTalk.SpecFlow;

namespace Monkey.Sql.WebApiHost.AcceptanceTests.SharedSteps
{
    [Binding]
    public class AppSteps
    {
        private readonly ApplicationUnderTests _app;

        public AppSteps(ApplicationUnderTests app)
        {
            _app = app;
        }

        [Given(@"WebApiHost has started")]
        public void GivenWebApiHostHasStarted()
        {
            _app.Run();
        }

    }
}
