using EventHub.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

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
                    _hubId = GetOrInsertHubId();

                return _hubId.Value;
            }
        }

        public int SourceId
        {
            get
            {
                if (_sourceId == null)
                    _sourceId = GetOrInsertSourceId();

                return _sourceId.Value;
            }
        }

        private SqlQueryExecutor CreateExecutor()
        {
            return new SqlQueryExecutor(ConnectionSettings);
        }

        private int GetOrInsertSourceId()
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
            FROM    dbo.EventSource WITH ( HOLDLOCK, UPDLOCK )
            WHERE   Name = @Name
                    AND HubId = @HubId
          );

IF @Id IS NULL
    BEGIN
        INSERT  INTO dbo.EventSource
                ( Name, HubId )
        VALUES  ( @Name, @HubId );
        SET @Id = SCOPE_IDENTITY();
    END;

SELECT  @Id;"
                    , new
                    {
                        Name = SourceName,
                        HubId = HubId,
                    }
                    , connection, transaction
                    , Settings.GetOrInsertSourceIdTimeout);

                    var result = (int)idValue;
                    transaction.Commit();

                    return result;
                }
            }
        }

        private int GetOrInsertHubId()
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
                    , Settings.GetOrInsertHubIdTimeout);

                    var result = (int)idValue;
                    transaction.Commit();

                    return result;
                }
            }
        }

        private int InsertEventData(EventData eventData
            , SqlQueryExecutor db, SqlConnection connection, SqlTransaction transaction)
        {
            var idValue = db.ExecuteScalar(
@"INSERT INTO dbo.EventData
( SourceId ,
  Timestamp ,
  Body
)
OUTPUT Inserted.Id
VALUES (@SourceId, @Timestamp, @Body);"
            , new
            {
                SourceId = SourceId,
                Timestamp = eventData.Timestamp,
                Body = eventData.Body,
            }
            , connection, transaction
            , Settings.InsertEventDataTimeout);

            var result = (int)idValue;

            return result;
        }

        public void SendEvents(IEnumerable<EventData> events)
        {
            Guard.ArgumentNotNull(events, nameof(events));

            using (var db = CreateExecutor())
            {
                using (var connection = db.CreateConnection())
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var eventData in events)
                        {
                            InsertEventData(eventData, db, connection, transaction);
                        }

                        transaction.Commit();
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
