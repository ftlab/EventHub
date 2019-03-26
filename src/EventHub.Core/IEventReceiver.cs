using System.Collections.Generic;

namespace EventHub.Core
{
    public interface IEventReceiver
    {
        IEnumerable<EventData> Receive();
    }
}
