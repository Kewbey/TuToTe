using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TuToTe.DataTypes.Telegram;
using TuToTe.Requests;

namespace TuToTe.Handlers
{
    class TelegramHandler
    {
        private List<long> MessageIds = new List<long>();
        private List<Update> FetchedUpdates = new List<Update>();
        private string BotApiKey;
        private SqlHandler SqlWorker;

        public TelegramHandler(string botApiKey)
        {
            BotApiKey = botApiKey;
            SqlWorker = new SqlHandler("Data Source=MainDatabase.db;Version=3;");
        }

        public void Start()
        {
            bool botRunning = true;

            // Fetching already checked updates to store them in a local variables to avoid tons of SQL requests
            MessageIds = SqlWorker.FetchCheckedUpdates();

            while (botRunning)
            {
                FetchedUpdates = FetchUpdates().Result;
                if (FetchedUpdates != null)
                {
                    Task.WaitAll(HandleUpdates(FetchedUpdates));
                }
            }
        }

        // TODO: make a fancier way to handle API calls
        public async Task<List<Update>> FetchUpdates()
        {
            var resp = new Response();
            var updates = new List<Update>();
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri($"https://api.telegram.org/bot{BotApiKey}/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.GetAsync("getUpdates");
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        resp = JsonConvert.DeserializeObject<Response>(@responseBody);

                        updates = resp.Result.ToList().Where(o => !MessageIds.Contains(o.UpdateId)).ToList();

                        return updates;

                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine("\nException caught!");
                        Console.WriteLine($"Message: {ex.Message}");
                        return null;
                    }
                }
            }
        }

        public async Task HandleUpdates(List<Update> updates)
        {
            foreach (var up in updates)
            {
                MessageIds.Add(up.UpdateId);
                switch (up.Message.Text)
                {
                    case "/test2":
                        Console.WriteLine("Tested!");
                        await SendMessage("You've been tested", up.Message.Chat.Id);
                        break;

                    default:
                        Console.WriteLine(up.Message.Text + " " + up.Message.Date);
                        break;
                }

                SqlWorker.InsertUpdate(up);
            }

            // Have the app make and handle calls every 1000 ms
            await Task.Delay(1000);
        }

        // id requires chat id, not user id
        public async Task SendMessage(string text, long chatId)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri($"https://api.telegram.org/bot{BotApiKey}/");

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
    }
}



