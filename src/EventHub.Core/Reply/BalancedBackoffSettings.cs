using System;

namespace EventHub.Core.Reply
{
    public class BalancedBackoffSettings
    {
        public TimeSpan MinIdling = TimeSpan.Zero;
        public TimeSpan Step = TimeSpan.FromSeconds(1);
        public TimeSpan MaxIdling = TimeSpan.FromMinutes(1);
    }
}
