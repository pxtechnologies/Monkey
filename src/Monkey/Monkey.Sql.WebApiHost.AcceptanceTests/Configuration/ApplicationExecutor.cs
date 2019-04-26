using System;
using System.Threading.Tasks;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Monkey.Sql.WebApiHost.AcceptanceTests.Configuration
{
    public interface IApplicationExecutor
    {
        void BindContainer(object container);
        void Execute<TService>(Action<TService> method);
        TResult Execute<TService, TResult>(Func<TService, TResult> method);
        Task<TResult> ExecuteAsync<TService, TResult>(Func<TService, Task<TResult>> method);
        Task ExecuteAsync<TService>(Func<TService, Task> method);
    }

    public sealed class ApplicationExecutor : IApplicationExecutor
    {
        private  Container _container;
        private ApplicationExecutor(Container container)
        {
            _container = container;
        }

        public static IApplicationExecutor Create(Container c)
        {
            return new ApplicationExecutor(c);
        }
        public static IApplicationExecutor CreateEmpty()
        {
            return new ApplicationExecutor(null);
        }

        public void BindContainer(object container)
        {
           if(_container != null)
               throw new InvalidOperationException();
           _container = (Container)container;
        }

        public void Execute<TService>(Action<TService> method)
        {
            using (var scope = AsyncScopedLifestyle.BeginScope(_container))
            {
                var service = (TService)scope.GetInstance(typeof(TService));
                method(service);
            }
        }
        public TResult Execute<TService, TResult>(Func<TService, TResult> method)
        {
            using (var scope = AsyncScopedLifestyle.BeginScope(_container))
            {
                var service = (TService)scope.GetInstance(typeof(TService));
                return method(service);
            }
        }
        public async Task<TResult> ExecuteAsync<TService, TResult>(Func<TService, Task<TResult>> method)
        {
            using (var scope = AsyncScopedLifestyle.BeginScope(_container))
            {
                var service = (TService)scope.GetInstance(typeof(TService));
                return await method(service);
            }
        }
        public async Task ExecuteAsync<TService>(Func<TService, Task> method)
        {
            using (var scope = AsyncScopedLifestyle.BeginScope(_container))
            {
                var service = (TService)scope.GetInstance(typeof(TService));
                await method(service);
            }
        }
    }
}