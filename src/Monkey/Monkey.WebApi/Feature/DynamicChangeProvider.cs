using System.Threading;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using Monkey.Services;

namespace Monkey.WebApi.Feature
{
    
    public class DynamicChangeProvider : IActionDescriptorChangeProvider, IMetadataChangedSubscriber
    {
        public static DynamicChangeProvider Instance = new DynamicChangeProvider();
        public CancellationTokenSource TokenSource { get; private set; }
        public IChangeToken GetChangeToken()
        {
            TokenSource = new CancellationTokenSource();
            return new CancellationChangeToken(TokenSource.Token);
        }
        public bool HasChanged { get; set; }

        public void Changed()
        {
            if(this != Instance)
                Instance.Changed();
            else
            {
                HasChanged = true;
                TokenSource?.Cancel();
            }
        }
    }
}