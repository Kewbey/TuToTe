using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TuToTe.DataTypes.Telegram;
using TuToTe.DataTypes.Helper;
using TuToTe.Requests;

namespace TuToTe.Handlers
{
    class TelegramHandler
    {
        // Used to notify the handler about new Tumblr posts from TumblrHandler
        public static bool NewPosts = false;

        private List<long> MessageIds = new List<long>();
        private List<Update> FetchedUpdates = new List<Update>();
        private string ApiKey;
        private SqlHandler SqlWorker;

        public TelegramHandler(string botApiKey)
        {
            ApiKey = botApiKey;
            SqlWorker = new SqlHandler("Data Source=MainDatabase.db;Version=3;Cache=Shared;");
        }

        public void Start()
        {
            bool botRunning = true;

            // Fetching already checked updates to store them in a local variable to avoid tons of SQL requests
            MessageIds = SqlWorker.FetchCheckedUpdates();

            while (botRunning)
            {
                FetchedUpdates = FetchUpdates().Result;
                if (FetchedUpdates != null)
                {
                    Task.WaitAll(HandleUpdates(FetchedUpdates));
                }
                if (NewPosts)
                {
                    Task.WaitAll(ProcessNewPosts());
                }
            }
        }

        // TODO: make a fancier way to handle API calls
        private async Task<List<Update>> FetchUpdates()
        {
            var resp = new Response();
            var updates = new List<Update>();
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri($"https://api.telegram.org/bot{ApiKey}/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.GetAsync("getUpdates");
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        resp = JsonConvert.DeserializeObject<Response>(@responseBody);

                        updates = resp.Result.ToList().Where(o => !MessageIds.Contains(o.UpdateId)).ToList();

                        // Have the app make and handle calls every 1000 ms
                        await Task.Delay(1000);

                        return updates;

                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine("\nException caught!");
                        Console.WriteLine($"Message: {ex.Message}");

                        await Task.Delay(1000);
                        return null;
                    }
                }
            }
        }

        private async Task HandleUpdates(List<Update> updates)
        {
            foreach (var up in updates)
            {
                if (up.Message != null)
                {
                    string temp = up.Message.Text.Trim();

                    if (temp.StartsWith("/test"))
                    {
                        Console.WriteLine("Tested!");
                        await SendMessage("You've been tested", up.Message.Chat.Id);
                    }

                    else if (temp.StartsWith("/add"))
                    {
                        temp = StripPrefix(temp, "/add").Trim();
                        AddTumblrEntry(up.Message.Chat, temp, up);
                    }

                    else if (temp.StartsWith("/remove"))
                    {
                        temp = StripPrefix(temp, "/remove").Trim();
                        RemoveTumblrEntry(up.Message.Chat, temp);
                    }

                    else if (temp.StartsWith("/help"))
                    {
                        await SendMessage("List of commands to use:\n" +
                            "/add %tumblrblog% -- Subscribe to a Tumblr blog. Blog should be specified either by its address without http://" +
                            "like /add test.tumblr.com, or the blog identifier from the address, like simply /add test.\n" +
                            "/remove %tumblrblog% -- Remove the specified Tumblr blog from your list of subscriptions. " +
                            "Blog should be specified the same way as it's said earlier. " +
                            "Be careful that blogs sometimes change their addresses.\n" +
                            "/list -- Show the list of Tumblr blogs you're subscribed to. Blog addresses get updated if the blog owners change them.\n" +
                            "/help -- Show this message.", up.Message.Chat.Id);
                    }

                    else if (temp.StartsWith("/list"))
                    {
                        PostSubList(up.Message.Chat);
                    }

                    else if (temp.StartsWith("/start"))
                    {

                    }

                    else
                    {
                        Console.WriteLine(up.Message.Text + " " + up.Message.Date);
                    }

                    SqlWorker.InsertUpdate(up);
                }
                MessageIds.Add(up.UpdateId);
            }
        }

        // id requires chat id, not user id
        private async Task SendMessage(string text, long chatId)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri($"https://api.telegram.org/bot{ApiKey}/");

                    var request = new SendMessageRequest(chatId, text);
                    string serializedMessage = JsonConvert.SerializeObject(request);
                    HttpContent content = new StringContent(serializedMessage, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("sendMessage", content);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("\nException caught!");
                    Console.WriteLine($"Message: {ex.Message}");
                }
            }
        }

        private async void AddTumblrEntry(Chat sender, string command, Update up)
        {
            StringBuilder blogId = new StringBuilder();
            bool check = await TumblrHandler.CheckBlog(command, blogId);
            bool alreadySubbed = SqlWorker.IsAlreadySubbed(sender, blogId.ToString());
            if (alreadySubbed)
            {
                await SendMessage("You're already subscribed to the specified blog.", sender.Id);
            }
            else if (!check)
            {
                await SendMessage("This blog can be seen only on the website dashboard or a blog with such address doesn't exist. Maybe you made a typo in the command?\n" +
                    "A proper way to use the command would be like \"/add goods.tumblr.com\"", sender.Id);
            }
            else
            {
                SqlWorker.MakeNewSubEntry(sender, up, command, blogId.ToString());
                await SendMessage("Successfully subscribed!", sender.Id);
            }
        }

        private async void RemoveTumblrEntry(Chat sender, string command)
        {
            StringBuilder blogId = new StringBuilder();
            bool check = await TumblrHandler.CheckBlog(command, blogId);
            bool entriesRemoved = SqlWorker.RemoveSubEntry(sender, blogId.ToString());
            if (!check)
            {
                await SendMessage("The specified blog doesn't exist. Perhaps the owner has changed its name?\n" +
                    "To see the list of blogs you're subscribed to, send /list", sender.Id);
            }
            else if (!entriesRemoved)
            {
                await SendMessage("You're not subscribed to the specified blog.\n" +
                    "To see the list of blogs you're subscribed to, send /list", sender.Id);
            }
            else
            {
                await SendMessage("Successfully unsubscribed!", sender.Id);
            }
        }

        private async void PostSubList(Chat sender)
        {
            var subscriptions = SqlWorker.GetUserSubscriptions(sender.Id);
            if (subscriptions.Count == 0)
            {
                await SendMessage("You have no subscriptions.", sender.Id);
            }
            else
            {
                var list = new StringBuilder("Your subscriptions:\n");
                foreach (var sub in subscriptions)
                {
                    list.AppendLine(sub);
                }
                await SendMessage(list.ToString(), sender.Id);
            }
            
        }

        private static string StripPrefix(string text, string prefix)
        {
            return text.StartsWith(prefix) ? text.Substring(prefix.Length) : text;
        }

        public async Task ProcessNewPosts()
        {
            var posts = SqlWorker.GetPostsToProcess();
            foreach (var post in posts)
            {
                var subs = SqlWorker.GetBlogSubscribers(post.BlogId);
                foreach (var sub in subs)
                {
                    await SendMessage($"New post on {post.BlogTitle}:\n{post.PostLink}", sub);
                }
            }

            SqlWorker.ClearPostsToProcess();
            NewPosts = false;
        }
    }
}