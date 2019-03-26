using System.Linq;

namespace EventHub.Core
{
    public static class EventSourceExtension
    {
        public static void SendEvent(this IEventSource source, EventData eventData)
        {
            Guard.ArgumentNotNull(source, nameof(source));
            Guard.ArgumentNotNull(eventData, nameof(eventData));

            source.SendEvents(Enumerable.Repeat(eventData, 1));
        }
    }
}
