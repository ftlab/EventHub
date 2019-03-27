using EventHub.Core;
using EventHub.Sql;
using System;
using System.Configuration;
using System.Text;

namespace EventSourceSample
{
    public class EntryPoint
    {
        public static void Main(params string[] args)
        {
            try
            {
                var settings = new SqlEventSourceSettings();
                settings.HubName = "test_hub";
                settings.SourceName = "test_source";
                settings.ConnectionSettings = new ConnectionStringSettings("test"
                    , @"Data Source=(localdb)\ProjectsV13;Initial Catalog=DB.EventHubs;Integrated Security=True;Pooling=False;Connect Timeout=30");

                var es = new SqlEventSource(settings);

                var events = new[]
                {
                    new EventData()
                    {
                        Body = Encoding.UTF8.GetBytes("Hello"),
                        Timestamp = DateTimeOffset.Now,
                    }
                };
                es.SendEvents(events);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
