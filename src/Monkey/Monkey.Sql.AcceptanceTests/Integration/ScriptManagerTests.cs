using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Monkey.Sql.AcceptanceTests.Database;
using Monkey.Sql.Model;
using Monkey.Sql.Scripts;
using NUnit.Framework;

namespace Monkey.Sql.AcceptanceTests.Integration
{
    [TestFixture]
    public class ScriptManagerTests
    {
        private ScriptManager Sut;
        private ScriptManager OnSutCreate()
        {
            var config = GetConfig();
            ScriptManager sm = new ScriptManager(config);
            return sm;
        }

        private static IConfigurationRoot GetConfig()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables()
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return config;
        }

        [SetUp]
        public async Task Setup()
        {
            var config = GetConfig();

            await SqlDbTools.ReCreateDatabase(config.GetConnectionString("Test"));
            await SqlDbTools.ReCreateDatabase(config.GetConnectionString("Monkey"));

            Sut = OnSutCreate();
        }
        
        [Test]
        public async Task ScriptManagerInstallsTestScripts()
        {
            await Sut.InstallExternal("Test");

            var migrations = await Sut.GetMigrations("Test");

            migrations.Should().HaveCount(3);
        }
        [Test]
        public async Task ScriptManagerInstallsMonkeyScripts()
        {
            await Sut.InstallMonkey();

            var migrations = await Sut.GetMigrations();

            migrations.Should().HaveCount(14);
        }
    }
}
