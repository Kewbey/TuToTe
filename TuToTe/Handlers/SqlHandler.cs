using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using 

namespace TuToTe.Handlers
{
    class SqlHandler
    {
        private SQLiteConnection _connection;

        public SqlHandler(string dbName)
        {
            _connection = new SQLiteConnection(dbName);
        }

        /// <summary>
        /// Basic SQL query
        /// </summary>
        /// <param name="query">The body of the query</param>
        public void Query(string query)
        {
            _connection.Open();
            SQLiteCommand command = new SQLiteCommand(query, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }

        /// <summary>
        /// Checks if the update was already processed
        /// </summary>
        /// <param name="id">ID of the update</param>
        /// <returns></returns>
        [Obsolete("The update checks were optimised, this thing is kept in case it's needed some time")]
        public bool IsUpdateChecked(int id)
        {
            int returnedId = 0;
            
            using (var command = new SQLiteCommand($"SELECT id from UpdateIDs WHERE id = {id}", _connection))
            {
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        returnedId = (int)reader["id"];
                    }
                }
                _connection.Close();
            }
           
            if (returnedId == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public List<long> FetchCheckedUpdates()
        {
            var updates = new List<long>(); ;
            using (var command = new SQLiteCommand($"SELECT id from UpdateIDs", _connection))
            {
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        updates.Add((long)reader["id"]);
                    }
                }
                _connection.Close();
            }
            return updates;
        }

        public void InsertUpdates ()
    }
}
