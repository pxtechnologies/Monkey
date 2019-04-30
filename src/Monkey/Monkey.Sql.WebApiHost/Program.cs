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
            if (Environment.GetEnvironmentVariable("Debug") != null || args.Contains("Debug"))
            {
                var ens = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
                foreach (var e in ens.Keys)
                {
                    Console.WriteLine($"{e}={ens[e]}");
                }

                if(args.Length > 0)
                    Console.WriteLine("Args: " + string.Join(' ',args));
            }
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
