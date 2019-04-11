using System;
using System.Collections.Generic;
using System.Text;
using Monkey.Builder;

namespace Monkey.Extensions
{
    public static class ServiceProviderExtension
    {
        static ServiceProviderExtension()
        {
            ScopeFactory = (x) => new DisposableScope();
        }
        class DisposableScope : IDisposable
        {
            public void Dispose()
            {
                
            }
        }
        public static Func<IServiceProvider, IDisposable> ScopeFactory { get; set; }
        public static TService GetService<TService>(this IServiceProvider provider)
        {
            return (TService) provider.GetService(typeof(TService));
        }

        public static IDisposable Scope(this IServiceProvider provider)
        {
            return ScopeFactory(provider);
        }
    }
}
