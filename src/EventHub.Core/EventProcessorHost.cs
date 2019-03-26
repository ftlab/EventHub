using System;
using System.Linq;
using System.Threading;

namespace EventHub.Core
{
    public class EventProcessorHost
    {
        private Thread _thread;
        private bool _stoped;

        private IEventProcessor _processor;
        private IEventReceiver _receiver;

        public EventProcessorHost(
            IEventProcessor processor
            , IEventReceiver receiver)
        {
            _processor = Guard.ArgumentNotNull(processor, nameof(processor));
            _receiver = Guard.ArgumentNotNull(receiver, nameof(receiver));
        }

        public IEventProcessor Processor { get { return _processor; } }
        public IEventReceiver Receiver { get { return _receiver; } }

        public void Stop()
        {
            lock (this)
            {
                if (_thread == null) throw new Exception();

                _stoped = true;
                if (_thread.IsAlive)
                    _thread.Abort();

                _thread = null;
            }
        }

        public void Start()
        {
            lock (this)
            {
                if (_thread != null) throw new Exception();

                _stoped = false;

                _thread = new Thread(RunLoop);
                _thread.Name = Processor.GetType().Name;
                _thread.Start();
            }
        }

        private void RunLoop()
        {
            while (!_stoped)
            {
                try
                {
                    Loop();
                }
                catch (Exception e)
                {

                }
            }
        }

        private void Loop()
        {
            var events = Receiver.Receive();
            Processor.ProcessEvents(events);
        }
    }
}
