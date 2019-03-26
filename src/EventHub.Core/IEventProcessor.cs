using System.Collections.Generic;

namespace EventHub.Core
{
    public interface IEventProcessor
    {
        void ProcessEvents(IEnumerable<EventData> events);
    }
}
