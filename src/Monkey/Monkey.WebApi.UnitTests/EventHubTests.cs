using System;
using System.Collections.Generic;
using System.Text;
using Monkey.Generator;
using Monkey.PubSub;
using Monkey.WebApi.Feature;
using NSubstitute;
using Xunit;

namespace Monkey.WebApi.UnitTests
{
    public class EventHubTests
    {
        [Fact]
        public void WireEventsRegisteresDynamicSubscriber()
        {
            IEventHub hub = NSubstitute.Substitute.For<IEventHub>();

            hub.WireEvents(typeof(DynamicChangeProvider).Assembly);

            hub.Received(1).Subscribe(typeof(DynamicChangeProvider), typeof(ServiceMadatadaChangedEvent));
        }
    }
}
