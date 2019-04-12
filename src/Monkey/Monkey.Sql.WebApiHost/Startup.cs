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
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monkey.PubSub;
using Monkey.SimpleInjector;
using Monkey.Sql.Services;
using Monkey.Sql.SimpleInjector;
using Monkey.Sql.WebApiHost.Configuration;
using Monkey.Sql.WebApiHost.Services;
using Monkey.WebApi;
using Monkey.WebApi.Feature;
using Monkey.WebApi.SimpleInjector;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using Swashbuckle.AspNetCore.Swagger;

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

            services.AddSingleton<IActionDescriptorChangeProvider>(DynamicChangeProvider.Instance);
            services.AddSingleton(DynamicChangeProvider.Instance);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

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

        private Assembly[] _assemblyCatalog;
        private void InitializeContainer(IApplicationBuilder app)
        {
            _assemblyCatalog = new Assembly[]
            {
                typeof(HostPackage).Assembly,
                typeof(MonkeyPackage).Assembly,
                typeof(WebApiPackage).Assembly,
                typeof(SqlPackage).Assembly
            };
            _container.RegisterPackages(_assemblyCatalog);
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

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Monkey Dynamic API");
            });

            app.UseMvc();
            _isReady = true;
            _container.GetInstance<IEventHub>().WireEvents(_assemblyCatalog);
            _container.GetInstance<IDbChangeListener>().Start();
        }
    }
}
