using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace TuToTe.Handlers
{

    // Make the class static
    class SqlHandler
    {
        private string connectionCommand;

        public SqlHandler(string dbName)
        {
            connectionCommand = dbName;
        }

        /// <summary>
        /// Checks if the update was already processed
        /// </summary>
        /// <param name="id">ID of the update</param>
        /// <returns></returns>
        [Obsolete("The update checks were optimised, this thing is kept in case it's needed some time")]
        public bool IsUpdateChecked(int id)
        {
            var connection = new SQLiteConnection(connectionCommand);
            int returnedId = 0;
            connection.Open();
            using (var command = new SQLiteCommand("SELECT ID FROM UpdateIDs WHERE ID = @ID", connection))
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
            connection.Close();

            if (returnedId == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Get list of already checked Telegram updates
        /// </summary>
        /// <returns></returns>
        public List<long> FetchCheckedUpdates()
        {
            var connection = new SQLiteConnection(connectionCommand);
            var updates = new List<long>();
            connection.Open();
            using (var command = new SQLiteCommand("SELECT id from UpdateIDs", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        updates.Add((long)reader["id"]);
                    }
                }
            }
            connection.Close();
            return updates;
        }

        /// <summary>
        /// Stores the update content and ID
        /// </summary>
        /// <param name="up">Update object</param>
        public void InsertUpdate(DataTypes.Telegram.Update up)
        {
            var connection = new SQLiteConnection(connectionCommand);
            connection.Open();
            using (var command = new SQLiteCommand("INSERT INTO UpdateIDs (ID, Content, Date, Sender) " +
                                                    "VALUES (@ID, @Content, @Date, @Sender)", connection))
            {
                command.Parameters.Add("@ID", System.Data.DbType.Int32).Value = up.UpdateId;
                command.Parameters.Add("@Content", System.Data.DbType.String).Value = JsonConvert.SerializeObject(up);
                command.Parameters.Add("@Date", System.Data.DbType.DateTime).Value = up.Message.Date;
                command.Parameters.Add("@Sender", System.Data.DbType.String).Value = up.Message.From.Id;
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        /// <summary>
        /// Get list of checked Tumblr posts
        /// </summary>
        /// <returns></returns>
        public List<long> FetchCheckedTumblrUpdates()
        {
            var connection = new SQLiteConnection(connectionCommand);
            var updates = new List<long>();
            connection.Open();
            using (var command = new SQLiteCommand("SELECT ID FROM TumblrPostIDs", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        updates.Add((long)reader["id"]);
                    }
                }
            }
            connection.Close();
            return updates;
        }

        /// <summary>
        /// Get list of blog IDs users are subscribed to
        /// </summary>
        /// <returns></returns>
        public List<string> GetBlogsToCheck()
        {
            var connection = new SQLiteConnection(connectionCommand);
            var blogs = new List<string>();
            connection.Open();
            using (var command = new SQLiteCommand("SELECT DISTINCT TumblrBlogID FROM SubData", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        blogs.Add((string)reader["TumblrBlogID"]);
                    }
                }
            }
            connection.Close();

            return blogs;
        }

        /// <summary>
        /// Insert newly checked Tumblr posts
        /// </summary>
        /// <param name="posts">List of posts checked</param>
        /// <param name="isNewBlog">If the blog is new, all its old posts won't be added to the list of posts needed to send to a user</param>
        public void InsertTumblrUpdates(List<DataTypes.Tumblr.Post> posts, bool isNewBlog)
        {
            var connection = new SQLiteConnection(connectionCommand);
            connection.Open();
            if (!isNewBlog)
            {
                using (var command = new SQLiteCommand("INSERT INTO TumblrToProcess (PostID, PostLink, BlogTitle, BlogID) " +
                                                    "VALUES (@PostID, @PostLink, @BlogTitle, @BlogID)", connection))
                {
                    foreach (var post in posts)
                    {
                        command.Parameters.Add("@PostID", System.Data.DbType.Int64).Value = post.Id;
                        command.Parameters.Add("@PostLink", System.Data.DbType.String).Value = post.PostUrl;
                        command.Parameters.Add("@BlogTitle", System.Data.DbType.String).Value = post.Blog.Title;
                        command.Parameters.Add("@BlogID", System.Data.DbType.String).Value = post.Blog.Uuid;
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                }
            }

            using (var command = new SQLiteCommand("INSERT INTO TumblrPostIDs (ID) VALUES (@ID)", connection))
            {
                foreach (var post in posts)
                {
                    command.Parameters.Add("@ID", System.Data.DbType.Int64).Value = post.Id;
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }

            connection.Close();
        }

        /// <summary>
        /// Check if the specified user already subscribed to specified blog
        /// </summary>
        /// <param name="sender">Telegram user</param>
        /// <param name="blogId">ID of the Tumblr blog</param>
        /// <returns></returns>
        public bool IsAlreadySubbed(DataTypes.Telegram.Chat sender, string blogId)
        {
            var connection = new SQLiteConnection(connectionCommand);
            bool isSubbed;

            connection.Open();
            using (var command = new SQLiteCommand("SELECT @ChatID FROM SubData WHERE TumblrBlogID = @BlogID", connection))
            {
                command.Parameters.Add("@ChatID", System.Data.DbType.Int64).Value = sender.Id;
                command.Parameters.Add("@BlogID", System.Data.DbType.String).Value = blogId;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        isSubbed = true;
                    }
                    else
                    {
                        isSubbed = false;
                    }
                }
            }
            connection.Close();

            return isSubbed;
        }

        /// <summary>
        /// Create new subscriber entry
        /// </summary>
        /// <param name="sender">Telegram user</param>
        /// <param name="update">Telegram update with info about subscribe request</param>
        /// <param name="blog">Tumblr blog name</param>
        /// <param name="blogId">Tumblr blog ID</param>
        public void MakeNewSubEntry(DataTypes.Telegram.Chat sender, DataTypes.Telegram.Update update, string blog, string blogId)
        {
            var connection = new SQLiteConnection(connectionCommand);
            connection.Open();
            using (var command = new SQLiteCommand("INSERT INTO SubData (UpdateID, ChatID, TumblrBlog, SubDate, TumblrBlogID) " +
                                                    "VALUES (@UpdateID, @ChatID, @TumblrBlog, @SubDate, @TumblrBlogID)", connection))
            {
                command.Parameters.Add("@UpdateID", System.Data.DbType.Int32).Value = update.UpdateId;
                command.Parameters.Add("@ChatID", System.Data.DbType.Int64).Value = sender.Id;
                command.Parameters.Add("@TumblrBlog", System.Data.DbType.String).Value = blog;
                command.Parameters.Add("@SubDate", System.Data.DbType.DateTime).Value = update.Message.Date;
                command.Parameters.Add("@TumblrBlogID", System.Data.DbType.String).Value = blogId;
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public bool RemoveSubEntry(DataTypes.Telegram.Chat sender, string blogId)
        {
            int rowsAffected;

            var connection = new SQLiteConnection(connectionCommand);
            connection.Open();
            using (var command = new SQLiteCommand("DELETE FROM SubData " +
                                                    "WHERE ChatID = @ChatID AND TumblrBlogID = @TumblrBlogID"))
            {
                command.Connection = connection;
                command.Parameters.Add("@ChatID", System.Data.DbType.Int64).Value = sender.Id;
                command.Parameters.Add("@TumblrBlogID", System.Data.DbType.String).Value = blogId;
                rowsAffected = command.ExecuteNonQuery();
                
            }
            connection.Close();

            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateBlogNames(string blogName, string blogId)
        {
            var connection = new SQLiteConnection(connectionCommand);
            connection.Open();
            using (var command = new SQLiteCommand("UPDATE SubData " +
                                                    "SET TumblrBlog = @TumblrBlog " +
                                                    "WHERE TumblrBlogID = @TumblrBlogID"))
            {
                command.Connection = connection;
                command.Parameters.Add("@TumblrBlog", System.Data.DbType.String).Value = blogName;
                command.Parameters.Add("@TumblrBlogID", System.Data.DbType.String).Value = blogId;
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public List<string> GetUserSubscriptions(long chatId)
        {
            var subscriptions = new List<string>();

            var connection = new SQLiteConnection(connectionCommand);
            connection.Open();
            using (var command = new SQLiteCommand("SELECT DISTINCT TumblrBlog FROM SubData WHERE ChatID = @ChatID"))
            {
                command.Connection = connection;
                command.Parameters.Add("@ChatID", System.Data.DbType.Int64).Value = chatId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subscriptions.Add((string)reader["TumblrBlog"] + ".tumblr.com");
                    }
                }
            }
            connection.Close();

            return subscriptions;
        }

        public List<DataTypes.Helper.PostToProcess> GetPostsToProcess()
        {
            var posts = new List<DataTypes.Helper.PostToProcess>();

            var connection = new SQLiteConnection(connectionCommand);
            connection.Open();
            using (var command = new SQLiteCommand("SELECT PostLink, BlogTitle, BlogID FROM TumblrToProcess"))
            {
                command.Connection = connection;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var post = new DataTypes.Helper.PostToProcess
                        {
                            PostLink = (string)reader["PostLink"],
                            BlogTitle = (string)reader["BlogTitle"],
                            BlogId = (string)reader["BlogID"]
                        };
                        posts.Add(post);
                    }
                }
            }
            connection.Close();

            return posts;
        }

        public List<long> GetBlogSubscribers(string blogId)
        {
            var subs = new List<long>();

            var connection = new SQLiteConnection(connectionCommand);
            connection.Open();
            using (var command = new SQLiteCommand("SELECT ChatID FROM SubData WHERE TumblrBlogID = @TumblrBlogID"))
            {
                command.Connection = connection;
                command.Parameters.Add("@TumblrBlogID", System.Data.DbType.String).Value = blogId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subs.Add((long)reader["ChatID"]);
                    }
                }
            }
            connection.Close();

            return subs;
        }

        public void ClearPostsToProcess()
        {
            var connection = new SQLiteConnection(connectionCommand);
            connection.Open();
            using (var command = new SQLiteCommand("DELETE FROM TumblrToProcess"))
            {
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}
