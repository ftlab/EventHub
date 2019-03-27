using EventHub.Core;
using EventHub.Sql;
using System;

namespace EventProcessorSample
{
    public class EntryPoint
    {
        public static void Main(params string[] args)
        {
            try
            {
                var processor = new TestEventProcessor();

                var host = new SqlEventProcessorHost(processor);

                host.Run();
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
