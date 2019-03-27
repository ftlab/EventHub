using EventHub.Core;
using System;
using System.Collections.Generic;

namespace EventHub.Sql
{
    public class SqlEventReceiver : IEventReceiver
    {
        public IEnumerable<EventData> Receive()
        {
            throw new NotImplementedException();
        }

        IEnumerable<EventData> IEventReceiver.Receive()
        {
            return Receive();
        }
    }
}
