using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TuToTe.DataTypes.Tumblr;

namespace TuToTe.Handlers
{
    class TumblrHandler
    {
        private string ApiKey;
        private SqlHandler SqlWorker;
        private static string ApiKeyParam;
        private static List<long> PostIds = new List<long>();

        public TumblrHandler(string tumblrApiKey)
        {
            ApiKey = tumblrApiKey;
            ApiKeyParam = "api_key=" + tumblrApiKey;
            SqlWorker = new SqlHandler("Data Source=MainDatabase.db;Version=3;Cache=Shared;");
        }

        public void Start()
        {
            bool botRunning = true;

            // Fetching already checked updates to store them in a local variable to avoid tons of SQL requests
            PostIds = SqlWorker.FetchCheckedTumblrUpdates();

            while (botRunning)
            {
                Task.WaitAll(ProcessEverything());
            }
        }

        private async Task ProcessEverything()
        {            
            var blogsToCheck = SqlWorker.GetBlogsToCheck();
            await UpdateBlogNames(blogsToCheck);
            foreach (var blog in blogsToCheck)
            {
                await FetchBlogUpdates(blog);
            }

            // Cooldown for Tumblr checking
            await Task.Delay(1800000);
        }

        private async Task FetchBlogUpdates(string blog)
        {
            var resp = new ApiResponse();
            var newPosts = new List<Post>();

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync($"https://api.tumblr.com/v2/blog/{blog}/posts?{ApiKeyParam}");
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ApiResponse>(responseBody);

                    newPosts = resp.Response.Posts.ToList().Where(o => !PostIds.Contains(o.Id)).ToList();

                    SqlWorker.InsertTumblrUpdates(newPosts, false);

                    if (newPosts.Count > 0)
                    {
                        TelegramHandler.NewPosts = true;
                    }

                    foreach (var post in newPosts)
                    {
                        PostIds.Add(post.Id);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("\nException caught!");
                    Console.WriteLine($"Message: {ex.Message}");
                }
            }
        }

        private async Task UpdateBlogNames(List<string> blogs)
        {
            foreach (var blog in blogs)
            {
                var resp = new ApiResponse();

                using (var client = new HttpClient())
                {
                    try
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.GetAsync($"https://api.tumblr.com/v2/blog/{blog}/info?{ApiKeyParam}");
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        resp = JsonConvert.DeserializeObject<ApiResponse>(responseBody);

                        SqlWorker.UpdateBlogNames(resp.Response.Blog.Name, resp.Response.Blog.Uuid);

                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine("\nException caught in CheckBlog method!");
                        Console.WriteLine($"Message: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the blog exists
        /// </summary>
        /// <param name="blog">Blog link without https://, i.e. goods.tumblr.com</param>
        /// <param name="blogId">This is passed to get ID of the blog to insert it into database</param>
        /// <returns></returns>
        public static async Task<bool> CheckBlog(string blog, StringBuilder blogId)
        {
            var resp = new ApiResponse();

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync($"https://api.tumblr.com/v2/blog/{blog}/info?{ApiKeyParam}");
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ApiResponse>(responseBody);

                    if (resp.Meta.Msg == "OK")
                    {
                        blogId.Append(resp.Response.Blog.Uuid);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("\nException caught in CheckBlog method!");
                    Console.WriteLine($"Message: {ex.Message}");
                    return false;
                }
            }
        }

        // Get rid of dublicated method
        public static async Task AddTumblrBlog(string blog, SqlHandler sqlHandler)
        {
            var resp = new ApiResponse();
            var posts = new List<Post>();

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync($"https://api.tumblr.com/v2/blog/{blog}/posts?{ApiKeyParam}");
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    resp = JsonConvert.DeserializeObject<ApiResponse>(responseBody);

                    posts = resp.Response.Posts.ToList().Where(o => !PostIds.Contains(o.Id)).ToList();

                    foreach (var post in posts)
                    {
                        PostIds.Add(post.Id);
                    }

                    sqlHandler.InsertTumblrUpdates(posts, true);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("\nException caught in CheckBlog method!");
                    Console.WriteLine($"Message: {ex.Message}");
                }
            }
        }
    }
}
