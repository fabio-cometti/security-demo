using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace FC.SecurityDemo.SQLInjection.Example05.Infrastructure
{
    public class DBManager
    {
        public const string CONNECTION_STRING = "Data Source=:memory:";
        private static SqliteConnection conn = null;

        public int ExecuteNonQuery(string commandText, int timeout = 30)
        {
            using var connection = GetConnection();            
            var command = connection.CreateCommand();
            command.CommandTimeout = timeout;
            command.CommandText = commandText;
            return command.ExecuteNonQuery();
        }

        public T ExecuteScalar<T>(string commandText, int timeout = 30)
        {
            using var connection = GetConnection();            
            var command = connection.CreateCommand();
            command.CommandTimeout = timeout;
            command.CommandText = commandText;
            return (T)command.ExecuteScalar();
        }

        public object ExecuteScalar(string commandText, int timeout)
        {
            using var connection = GetConnection();            
            var command = connection.CreateCommand();
            command.CommandTimeout = timeout;
            command.CommandText = commandText;
            return command.ExecuteScalar();
        }

        public DbDataReader ExecuteReader(string commandText)
        {
            using var connection = GetConnection();            
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            return command.ExecuteReader();
        }        

        public SqliteConnection GetConnection()
        {
            if(conn == null)
            {
                conn = new SqliteConnection(CONNECTION_STRING);
                conn.Open();
            }
            return conn;
        }
    }
}
