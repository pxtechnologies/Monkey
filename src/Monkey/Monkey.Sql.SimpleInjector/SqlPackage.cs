using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Monkey.Sql.Generator;
using Monkey.Sql.Model;
using Monkey.Sql.Services;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Monkey.Sql.SimpleInjector
{
    public class SqlPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<ISqlCqrsGenerator, SqlCqrsGenerator>(Lifestyle.Scoped);
            container.Register<IServiceMatadataLoader, ServiceMatadataLoader>(Lifestyle.Scoped);
            container.Register<IDbChangeListener, DbChangeListener>(Lifestyle.Singleton);
            container.Register<IRepository>(()=> SqlPackage.OnCreate(container), Lifestyle.Scoped);
            container.Register<IMonkeyDatabase>(() => new MonkeyDatabase(SqlPackage.OnCreate(container).Database));
        }

        private static MonkeyDbContext OnCreate(Container container)
        {
            var config = container.GetInstance<IConfiguration>();
            DbContextOptionsBuilder<MonkeyDbContext> builder = new DbContextOptionsBuilder<MonkeyDbContext>();
            builder.UseSqlServer(config.GetConnectionString("Monkey"));
            return new MonkeyDbContext(builder.Options, config.GetValue<string>("DefaultSchema"));
        }
    }
}
