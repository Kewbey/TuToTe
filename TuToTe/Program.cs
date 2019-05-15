using System;
using System.Threading;
using TuToTe.Handlers;

namespace TuToTe
{
    class Program
    {
        private TelegramHandler TelegramWorker;
        private string TelegramBotApi;

        static void Main(string[] args)
        {
            Program program = new Program();
            
            if (args.Length > 0)
            {
                program.TelegramBotApi = args[0];
                program.Go();
            }
            else
            {
                Console.WriteLine("Provide an API key for Telegram bot!");
                return;
            }            
        }

        public void Go()
        {
            TelegramWorker = new TelegramHandler(TelegramBotApi);
            var telegramThread = new Thread(TelegramWorker.Start);
            telegramThread.Start();
        }
    }
}
