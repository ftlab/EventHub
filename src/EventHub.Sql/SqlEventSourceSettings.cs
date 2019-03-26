using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace EventHub.Sql
{
    public class SqlEventSourceSettings
    {
        public string SourceName { get; set; }

        public string HubName { get; set; }

        public ConnectionStringSettings ConnectionSettings { get; set; }

        public TimeSpan SqlTimeout { get; set; } = TimeSpan.FromMinutes(1);
    }
}
