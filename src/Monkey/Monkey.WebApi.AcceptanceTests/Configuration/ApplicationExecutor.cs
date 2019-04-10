using System;
using System.Threading.Tasks;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Monkey.WebApi.AcceptanceTests.Configuration
{
    public interface IApplicationExecutor
    {
        void Execute(Type obj, Action<object> action);
        object Execute(Type obj, Func<object, object> method);
        void Execute<TService>(Action<TService> method);
        TResult Execute<TService, TResult>(Func<TService, TResult> method);
        Task<TResult> ExecuteAsync<TService, TResult>(Func<TService, Task<TResult>> method);
        Task ExecuteAsync<TService>(Func<TService, Task> method);
    }


    public sealed class ApplicationExecutor : IApplicationExecutor
    {
        private readonly Container _container;
        private ApplicationExecutor(Container container)
        {
            _container = container;
        }

        public static IApplicationExecutor Create(Container c)
        {
            return new ApplicationExecutor(c);
        }

        public void Execute(Type obj, Action<object> action)
        {
            using (var scope = AsyncScopedLifestyle.BeginScope(_container))
            {
                var service = scope.GetInstance(obj);
                action(service);
            }
        }

        public object Execute(Type obj, Func<object, object> method)
        {
            using (var scope = AsyncScopedLifestyle.BeginScope(_container))
            {
                var service = scope.GetInstance(obj);
                return method(service);
            }
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