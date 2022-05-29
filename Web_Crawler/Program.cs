using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Web_Crawler.Services;
using Web_Crawler.Services.Interfaces;

namespace Web_Crawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider =await ServicesResolver();
            var crawlerService = serviceProvider.GetService<ICrawlerService>();
            await App.Execure(crawlerService);
            DisposeService(serviceProvider);
        }
        private static Task<IServiceProvider> ServicesResolver()
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<ICrawlerService, CrawlerService>();
            collection.AddSingleton<IWebPageRequestService, WebPageRequestService>();
            collection.AddSingleton<IHtmlLinksService, HtmlLinksService>();
            collection.AddSingleton<IDirectoryAndFileHandlerService, DirectoryAndFileHandlerService>();
            IServiceProvider serviceProvider = collection.BuildServiceProvider();
            return Task.FromResult(serviceProvider);
        }
        private static void DisposeService(IServiceProvider serviceProvider)
        {
            if (serviceProvider is IDisposable)
            {
                ((IDisposable)serviceProvider).Dispose(); ;
            }
        }
    }
}
