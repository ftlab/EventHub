using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using EventHub.Core;

namespace EventHub.Sql
{
    class SqlQueryExecutor : IDisposable
    {
        private ConnectionStringSettings _connectionSettings;

        public SqlQueryExecutor(ConnectionStringSettings connectionSettings)
        {
            _connectionSettings = Guard.ArgumentNotNull(connectionSettings, nameof(connectionSettings));
        }

        public ConnectionStringSettings ConnectionSettings
        {
            get
            {
                return _connectionSettings;
            }
        }

        internal SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionSettings.ConnectionString);
        }

        internal SqlCommand CreateCommand(string commandText
            , object parameterBag
            , SqlConnection connection
            , SqlTransaction transaction
            , TimeSpan timeout)
        {
            Guard.ArgumentNotNull(connection, nameof(connection));

            var cmd = connection.CreateCommand();

            cmd.Transaction = transaction;
            cmd.CommandText = commandText;
            cmd.CommandTimeout = (int)Math.Ceiling(timeout.TotalSeconds);

            var parameters = CreateParameters(parameterBag);
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            return cmd;
        }

        internal void ExecuteNonQuery(string commandText
            , object parameterBag
            , SqlConnection connection
            , SqlTransaction transaction)
        {
            throw new NotImplementedException();
        }

        internal object ExecuteScalar(string commandText
            , object parameterBag
            , SqlConnection connection
            , SqlTransaction transaction
            , TimeSpan timeout)
        {
            using (var cmd = CreateCommand(commandText, parameterBag, connection, transaction, timeout))
            {
                var result = cmd.ExecuteScalar();

                return result;
            }
        }

        private SqlParameter[] CreateParameters(object parameterBag)
        {
            SqlParameter[] result = null;

            if (parameterBag != null)
            {
                var properties = parameterBag.GetType().GetProperties();

                result = new SqlParameter[properties.Length];

                for (int i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];
                    var value = property.GetValue(parameterBag, null);
                    var parameter = new SqlParameter(property.Name, value ?? DBNull.Value);

                    result[i] = parameter;
                }
            }

            return result;
        }

        internal int GetOrInsertSourceId(string sourceName, int hubId)
        {
            throw new NotImplementedException();
        }

        internal void InsertEventData(int sourceId, EventData eventData, SqlConnection connection, SqlTransaction transaction)
        {
            Guard.ArgumentNotNull(eventData, nameof(eventData));

            ExecuteNonQuery(
    @"INSERT INTO dbo.EventData
( SourceId ,
  Timestamp ,
  Body
)
VALUES (@SourceId, @Timestamp, @Body);"
            , new
            {
                SourceId = sourceId,
                Timestamp = eventData.Timestamp,
                Body = eventData.Body,
            }
            , connection, transaction);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }
        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SqlQueryExecutor() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
