using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using TuToTe.DataTypes.Telegram;
using Newtonsoft.Json;

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
        /// Basic SQL query, to use only internally
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
            _connection.Open();
            using (var command = new SQLiteCommand("SELECT ID FROM UpdateIDs WHERE ID = @ID", _connection))
            {
                command.Parameters.Add("@ID", System.Data.DbType.Int32).Value = id;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        returnedId = (int)reader["id"];
                    }
                }
            }
            _connection.Close();

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
            var updates = new List<long>();
            _connection.Open();
            using (var command = new SQLiteCommand("SELECT id from UpdateIDs", _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        updates.Add((long)reader["id"]);
                    }
                }
            }
            _connection.Close();
            return updates;
        }

        /// <summary>
        /// Stores the update content and ID
        /// </summary>
        /// <param name="up">Update object</param>
        public void InsertUpdate(Update up)
        {
            _connection.Open();
            using (var command = new SQLiteCommand("INSERT INTO UpdateIDs (ID, Content, Date, Sender) " +
                                                    "VALUES (@ID, @Content, @Date, @Sender)", _connection))
            {
                command.Parameters.Add("@ID", System.Data.DbType.Int32).Value = up.UpdateId;
                command.Parameters.Add("@Content", System.Data.DbType.String).Value = JsonConvert.SerializeObject(up);
                command.Parameters.Add("@Date", System.Data.DbType.DateTime).Value = up.Message.Date;
                command.Parameters.Add("@Sender", System.Data.DbType.String).Value = up.Message.From.Id;
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }
    }
}
