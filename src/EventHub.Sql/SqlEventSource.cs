using EventHub.Core;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace EventHub.Sql
{
    public class SqlEventSource : IEventSource
    {
        private SqlEventSourceSettings _settings;

        private int? _sourceId;

        private int? _hubId;

        public SqlEventSource(SqlEventSourceSettings settings)
        {
            _settings = Guard.ArgumentNotNull(settings, nameof(settings));
        }

        public SqlEventSourceSettings Settings { get { return _settings; } }

        public string SourceName { get { return Settings.SourceName; } }

        public string HubName { get { return Settings.HubName; } }

        public ConnectionStringSettings ConnectionSettings { get { return Settings.ConnectionSettings; } }

        public int HubId
        {
            get
            {
                if (_hubId == null)
                    _hubId = GetOrInsertEventHubId();

                return _hubId.Value;
            }
        }

        public int SourceId
        {
            get
            {
                if (_sourceId == null)
                {
                    using (var db = CreateExecutor())
                    {
                        _sourceId = db.GetOrInsertSourceId(SourceName, HubId);
                    }
                }
                return _sourceId.Value;
            }
        }

        private SqlQueryExecutor CreateExecutor()
        {
            return new SqlQueryExecutor(ConnectionSettings);
        }

        private int GetOrInsertEventHubId()
        {
            using (var db = CreateExecutor())
            using (var connection = db.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var idValue = db.ExecuteScalar(
 @"DECLARE @Id INT;

SET @Id = ( SELECT  Id
            FROM    dbo.EventHub WITH ( HOLDLOCK, UPDLOCK )
            WHERE   Name = @Name
          );

IF @Id IS NULL
    BEGIN
        INSERT  INTO dbo.EventHub
                ( Name )
        VALUES  ( @Name );
        SET @Id = SCOPE_IDENTITY();
    END;

SELECT  @Id;"
                    , new
                    {
                        Name = HubName
                    }
                    , connection, transaction
                    , Settings.SqlTimeout);

                    transaction.Commit();

                    return (int)idValue;

                }
            }
        }

        public void SendEvents(IEnumerable<EventData> events)
        {
            using (var db = CreateExecutor())
            {
                using (var connection = db.CreateConnection())
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var eventData in events)
                        {
                            db.InsertEventData(SourceId, eventData, connection, transaction);
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
