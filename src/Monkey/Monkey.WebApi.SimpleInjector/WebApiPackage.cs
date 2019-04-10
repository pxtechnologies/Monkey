using System;
using AutoMapper;
using Monkey.Generator;
using Monkey.WebApi.Generator;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Monkey.WebApi.SimpleInjector
{
    public class WebApiPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<ICommandHandlerRegister, CommandHandlerRegister>(Lifestyle.Singleton);
            container.Register<ISourceCodeGenerator, WebApiCqrsGenerator>(Lifestyle.Singleton);
            container.Register<ICqrsControllerGenerator, CqrsControllerGenerator>(Lifestyle.Singleton);
            container.Register<IDynamicTypePool,DynamicTypePool>(Lifestyle.Singleton);
            container.Register<AutoMapperFactory>(Lifestyle.Singleton);
            container.Register<IMapper>(() => container.GetInstance<AutoMapperFactory>().Create(), Lifestyle.Scoped);
        }
    }

    public class AutoMapperFactory
    {
        private IDynamicTypePool pool;
        private Guid _signature;
        private IMapper _mapper;
        public AutoMapperFactory(IDynamicTypePool pool)
        {
            this.pool = pool;
        }

        public IMapper Create()
        {
            if (_signature != pool.Signature || _mapper == null)
            {
                _signature = pool.Signature;
                _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(pool.GetAssemblies())));
            }

            return _mapper;
        }
    }
}
