using System;
using System.Threading;
using TuToTe.Handlers;

namespace TuToTe
{
    class Program
    {
        private TelegramHandler TelegramWorker;
        private TumblrHandler TumblrWorker;
        private string TelegramApiKey;
        private string TumblrApiKey;

        static void Main(string[] args)
        {
            Program program = new Program();
            
            if (args.Length > 1)
            {
                program.TelegramApiKey = args[0];
                program.TumblrApiKey = args[1];
                program.Go();
            }
            else
            {
                Console.WriteLine("Provide both API keys!");
                return;
            }            
        }

        public void Go()
        {
            TelegramWorker = new TelegramHandler(TelegramApiKey);
            TumblrWorker = new TumblrHandler(TumblrApiKey);
            var telegramThread = new Thread(TelegramWorker.Start);
            var tumblrThread = new Thread(TumblrWorker.Start);
            telegramThread.Start();
            tumblrThread.Start();
        }
    }
}
