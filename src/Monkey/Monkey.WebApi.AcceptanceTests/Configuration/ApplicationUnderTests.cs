﻿using System;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Monkey.SimpleInjector;
using Monkey.WebApi.SimpleInjector;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using TechTalk.SpecFlow;

namespace Monkey.WebApi.AcceptanceTests.Configuration
{
    [Binding]
    public class ApplicationUnderTests
    {
        private ScenarioContext _context;
        private Container _container;
        private IApplicationExecutor _executor;
        private IWebHost _webHost;
        public IApplicationExecutor Executor => _executor;

        public ApplicationUnderTests(ScenarioContext context)
        {
            _context = context;
        }

        [AfterScenario()]
        public void CleanUp()
        {
            _webHost.StopAsync().GetAwaiter().GetResult();
        }

        [BeforeScenario()]
        public void InitContainer()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddCommandLine(Environment.GetCommandLineArgs());

            var config = builder.Build();

            var applicationUrls = config.GetApplicationUrls();

            _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(applicationUrls)
                .UseStartup<Startup>()
                .Build();

            _webHost.Start();
            _container = ((ContainerAccessor)_webHost.Services.GetService(typeof(ContainerAccessor))).Container;
            _executor = ApplicationExecutor.Create(_container);

            _context.ScenarioContainer.RegisterInstanceAs(Executor);
        }
    }
}