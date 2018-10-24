using System;
using System.Threading;

namespace EventHub.Core.Reply
{
    public class BalancedBackoff : IDisposable
    {
        private AutoResetEvent _event = new AutoResetEvent(false);

        private TimeSpan _value;

        private int _stepCount;

        public BalancedBackoff(BalancedBackoffSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));

            _value = Settings.MinIdling;
        }

        public BalancedBackoffSettings Settings { get; }

        public void Release()
        {
            _event.Set();
        }

        public TimeSpan Next(BalancedBackoffSignal signal)
        {
            if (signal == BalancedBackoffSignal.None)
                return _value;

            if (signal == BalancedBackoffSignal.Increase)
            {
                _value = _value + Settings.Step;

                if (_value <= Settings.MaxIdling)
                    _stepCount++;
                else
                    _value = Settings.MaxIdling;

                return _value;
            }

            if (signal == BalancedBackoffSignal.Decrease)
            {
                bool exp = _stepCount < 0;
                _stepCount = 0;


            }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                _event.Close();
                _event = null;
                disposedValue = true;
            }
        }

        ~BalancedBackoff()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
