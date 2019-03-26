using System.Collections.Generic;

namespace EventHub.Core
{
    public interface IEventSource
    {
        void SendEvents(IEnumerable<EventData> events);
    }
}
