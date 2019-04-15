using Monkey.PubSub;

namespace Monkey.Sql.Extensions
{
    public static class EventHubExtensions
    {
        public static IEventHub WireSql(this IEventHub hub)
        {
            hub.WireEvents(typeof(EventHubExtensions).Assembly);
            return hub;
        }
    }
}