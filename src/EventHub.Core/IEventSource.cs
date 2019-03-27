using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventHub.Core
{
    public interface IEventSource
    {
        void SendEvents(IEnumerable<EventData> events);
    }
}
