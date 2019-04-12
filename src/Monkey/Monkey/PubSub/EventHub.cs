using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Monkey.Extensions;

namespace Monkey.PubSub
{
    public static class HubExtensions
    {
        public static void WireEvents(this IEventHub hub, params Assembly[] assemblies)
        {
            var registrations = assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && !x.IsInterface && !x.IsAbstract)
                .Where(x => typeof(IEventSubscriber).IsAssignableFrom(x))
                .Select(x => new
                {
                    ServiceType = x, Interfaces = x.GetInterfaces()
                        .Where(i => i.IsGenericType && !x.IsGenericTypeDefinition && i.GetGenericTypeDefinition() == typeof(IEventSubscriber<>)).ToArray()
                })
                .Where(x => x.Interfaces.Any())
                .ToArray();
            foreach (var r in registrations)
            {
                foreach (var s in r.Interfaces)
                {
                    hub.Subscribe(r.ServiceType, s.GetGenericArguments()[0]);
                }
            }
        }
    }
    /// <summary>
    /// Markup interface
    /// </summary>
    public interface IEventSubscriber { }
    public interface IEventSubscriber<TEvent> : IEventSubscriber
    {
        Task Handle<TEvent>();
    }

    public interface IEventHub
    {
        Task Publish<TEvent>(TEvent e);

        void Subscribe<TSubscriber, TEvent>()
            where TSubscriber : IEventSubscriber<TEvent>;

        void Subscribe(Type subscriberType, Type eventType);
    }

    public class EventHub : IEventHub
    {
        private readonly IServiceProvider _serviceProvider;
        private Dictionary<Type, List<Type>> _subscribers;

        public EventHub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _subscribers = new Dictionary<Type, List<Type>>();
        }
        public async Task Publish<TEvent>(TEvent e)
        {
            if(_subscribers.TryGetValue(typeof(TEvent), out List<Type> subscribers))
                foreach (var s in subscribers)
                {
                    using (_serviceProvider.Scope())
                    {
                        dynamic subscriber = _serviceProvider.GetService(s);
                        subscriber.Handle(e);
                    }
                }
        }

        public void Subscribe<TSubscriber, TEvent>()
            where TSubscriber : IEventSubscriber<TEvent>
        {
            Subscribe(typeof(TSubscriber), typeof(TEvent));
        }

        public void Subscribe(Type subscriberType, Type eventType)
        {
            if (!_subscribers.TryGetValue(eventType, out List<Type> subscribers))
            {
                subscribers = new List<Type>();
                _subscribers.TryAdd(eventType, subscribers);
            }
            subscribers.Add(subscriberType);
        }
    }
}
