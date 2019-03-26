using EventHub.Core;
using System;
using System.Collections.Generic;

namespace EventHub.Sql
{
    public class SqlEventSource : IEventSource
    {
        private SqlEventSourceSettings _settings;

        private int? _sourceId;

        public SqlEventSource(SqlEventSourceSettings settings)
        {
            _settings = Guard.ArgumentNotNull(settings, nameof(settings));
        }

        public SqlEventSourceSettings Settings { get { return _settings; } }

        public int SourceId
        {
            get
            {
                if (_sourceId == null)
                {

                }

                return _sourceId.Value;
            }
        }

        public void SendEvents(IEnumerable<EventData> events)
        {
            using (var db = new SqlQueryExecutor())
            {
                using (var connection = db.CreateConnection())
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var eventData in events)
                        {
                            db.ExecuteNonQuery(
    @"INSERT INTO dbo.EventData
( SourceId ,
  Timestamp ,
  Body
)
VALUES (@SourceId, @Timestamp, @Body);"
                            , new
                            {
                                SourceId = SourceId,
                                Timestamp = eventData.Timestamp,
                                Body = eventData.Body,
                            }
                            , connection, transaction);
                        }
                    }
                }
            }
        }

        #region IEventSource support

        void IEventSource.SendEvents(IEnumerable<EventData> events)
        {
            SendEvents(events);
        }

        #endregion
    }
}
