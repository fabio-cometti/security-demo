using FC.SecurityDemo.SQLInjection.Example05.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FC.SecurityDemo.SQLInjection.Example05.Infrastructure
{
    public static class DataInitializers
    {
        public static void SeedData(DBManager db)
        {
            var connection = db.GetConnection();
            
            connection.Open();
            CreateUserTable(connection);
            InsertUserInTable(connection, "Alice", "alice@localhost.it", "password1");
            InsertUserInTable(connection, "Bob", "bob@localhost.it", "password2");
            InsertUserInTable(connection, "Charles", "charles@localhost.it", "password3");
            InsertUserInTable(connection, "Dave", "dave@localhost.it", "password4");    
        }

        public static void CreateUserTable(SqliteConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS Users(
                        [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
                        [Username] NVARCHAR(64) NOT NULL, 
                        [Email] NVARCHAR(128) NOT NULL, 
                        [Password] NVARCHAR(128) NOT NULL)";
                    
                command.ExecuteNonQuery();

                
            }
        }

        public static void InsertUserInTable(SqliteConnection connection, string username, string email, string password)
        {
            using (var command = connection.CreateCommand())
            {
                // Insert a record
                command.CommandText = @$"INSERT INTO Users(Username, Email, Password) VALUES('{username}', '{email}', '{password}') ";
                command.ExecuteNonQuery();
            }
        }        
    }
}
