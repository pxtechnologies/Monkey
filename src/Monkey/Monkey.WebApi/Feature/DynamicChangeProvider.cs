using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using Monkey.Generator;
using Monkey.PubSub;

namespace Monkey.WebApi.Feature
{
    
    public class DynamicChangeProvider : IActionDescriptorChangeProvider, 
        IEventSubscriber<ServiceMadatadaChangedEvent>
    {
        public static DynamicChangeProvider Instance = new DynamicChangeProvider();
        public CancellationTokenSource TokenSource { get; private set; }
        public IChangeToken GetChangeToken()
        {
            TokenSource = new CancellationTokenSource();
            return new CancellationChangeToken(TokenSource.Token);
        }
        public bool HasChanged { get; set; }

        private void Changed()
        {
            if(this != Instance)
                Instance.Changed();
            else
            {
                HasChanged = true;
                TokenSource?.Cancel();
            }
        }

        public async Task Handle(ServiceMadatadaChangedEvent ev)
        {
            Instance.Changed();
        }
    }
}