using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Monkey.Sql.WebApiHost.Configuration;
using Monkey.WebApi;

namespace Monkey.Sql.WebApiHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = ConfigurationFactory.Load();
            var applicationUrls = config.GetApplicationUrls();

            return WebHost.CreateDefaultBuilder(args)
                .UseUrls(applicationUrls)
                .UseStartup<Startup>();
        }
    }
}
