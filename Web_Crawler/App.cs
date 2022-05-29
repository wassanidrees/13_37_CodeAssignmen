using System;
using System.IO;
using System.Threading.Tasks;
using Web_Crawler.Services;
using Web_Crawler.Services.Interfaces;

namespace Web_Crawler
{
    public static class App
    {
        public static async Task Execure(ICrawlerService crawlerService)
        {
            if (!Directory.Exists(Constants.SavingDirectory))
            {
                Directory.CreateDirectory(Constants.SavingDirectory);
            }
            Console.WriteLine("Scrapping Startd");
            await crawlerService.StartCrawling();
            Console.WriteLine("Scrapping Ended");
            Console.ReadKey();
        }
    }
}
