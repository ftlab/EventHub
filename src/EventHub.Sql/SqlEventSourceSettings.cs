using System;
using System.Configuration;

namespace EventHub.Sql
{
    public class SqlEventSourceSettings
    {
        public string SourceName { get; set; }

        public string HubName { get; set; }

        public ConnectionStringSettings ConnectionSettings { get; set; }

        public TimeSpan GetOrInsertHubIdTimeout { get; set; } = TimeSpan.FromMinutes(1);

        public TimeSpan GetOrInsertSourceIdTimeout { get; set; } = TimeSpan.FromMinutes(1);

        public TimeSpan InsertEventDataTimeout { get; set; } = TimeSpan.FromMinutes(1);
    }
}
