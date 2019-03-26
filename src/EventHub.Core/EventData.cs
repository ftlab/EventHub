using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventHub.Core
{
    public class EventData
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public byte[] Body { get; set; }
    }
}
