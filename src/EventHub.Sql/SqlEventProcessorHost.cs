using EventHub.Core;

namespace EventHub.Sql
{
    public class SqlEventProcessorHost : EventProcessorHost
    {
        public SqlEventProcessorHost(IEventProcessor processor)
            : base(processor, new SqlEventReceiver())
        {
        }
    }
}
