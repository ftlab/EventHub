using EventHub.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventProcessorSample
{
    public class TestEventProcessor : IEventProcessor
    {
        public void ProcessEvents(IEnumerable<EventData> events)
        {
            foreach (var eventData in events)
            {
                Console.WriteLine(eventData.Timestamp);
            }
        }
    }
}
