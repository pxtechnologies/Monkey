using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monkey.SimpleInjector;
using Monkey.Sql.SimpleInjector;
using Monkey.Sql.WebApiHost.Configuration;
using Monkey.Sql.WebApiHost.Services;
using Monkey.WebApi;
using Monkey.WebApi.SimpleInjector;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace Monkey.Sql.WebApiHost
{
    public class Startup
    {
        private Container _container;
        private bool _isReady;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            CreateContainer();
            IntegrateSimpleInjector(services);

            services.AddMvc()
                .ConfigureApplicationPartManager(m =>
                {
                    var dynamicApiFeatureProvider = new DynamicApiFeatureProvider(_container, () => _isReady);
                    m.FeatureProviders.Add(dynamicApiFeatureProvider);
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
        }
        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(_container));

            services.EnableSimpleInjectorCrossWiring(_container);
            services.UseSimpleInjectorAspNetRequestScoping(_container);
            services.Add(new ServiceDescriptor(typeof(ContainerAccessor), x => new ContainerAccessor(_container), ServiceLifetime.Singleton));
        }
        private void CreateContainer()
        {
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            _container.Options.DefaultLifestyle = Lifestyle.Scoped;


        }
        private void InitializeContainer(IApplicationBuilder app)
        {
            Assembly[] catalog = new Assembly[]
            {
                typeof(HostPackage).Assembly,
                typeof(MonkeyPackage).Assembly,
                typeof(WebApiPackage).Assembly,
                typeof(SqlPackage).Assembly
            };
            _container.RegisterPackages(catalog);
            // Add application presentation components:
            _container.RegisterMvcControllers(app);
            _container.RegisterMvcViewComponents(app);

            // Allow Simple Injector to resolve services from ASP.NET Core.
            _container.AutoCrossWireAspNetComponents(app);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeContainer(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            _container.Verify();
            app.UseMvc();
            _isReady = true;
        }
    }
}
