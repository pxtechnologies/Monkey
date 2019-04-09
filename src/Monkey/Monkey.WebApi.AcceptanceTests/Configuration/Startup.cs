using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monkey.SimpleInjector;
using Monkey.WebApi.SimpleInjector;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SimpleInjector.Integration.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Monkey.WebApi.AcceptanceTests.Configuration
{
    public class ContainerAccessor
    {
        public ContainerAccessor(Container container)
        {
            Container = container;
        }

        public Container Container { get; private set; }
    }
    public class Startup
    {
        private Container _container;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            CreateContainer();
            IntegrateSimpleInjector(services);
        }
        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(_container));

            services.EnableSimpleInjectorCrossWiring(_container);
            services.UseSimpleInjectorAspNetRequestScoping(_container);
            services.Add(new ServiceDescriptor(typeof(ContainerAccessor), x => new ContainerAccessor(_container), ServiceLifetime.Singleton ));
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
                typeof(ApplicationUnderTestsPackage).Assembly,
                typeof(MonkeyPackage).Assembly,
                typeof(WebApiPackage).Assembly
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
        }
    }
}