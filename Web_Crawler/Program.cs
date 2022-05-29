using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            var host = CreateHostBuilder(args).Build();
            var crawlerService = host.Services.GetService<ICrawlerService>();
            await App.Execure(crawlerService);
            DisposeService(host.Services);
        }
        static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
          .ConfigureServices((_, collection) =>
          {
              collection.AddSingleton<ICrawlerService, CrawlerService>();
              collection.AddSingleton<IWebPageRequestService, WebPageRequestService>();
              collection.AddSingleton<IHtmlLinksService, HtmlLinksService>();
              collection.AddSingleton<IDirectoryAndFileHandlerService, DirectoryAndFileHandlerService>();
          })
          .ConfigureLogging((_, logging) =>
          {
              logging.ClearProviders();
              logging.AddSimpleConsole(options => options.IncludeScopes = true);
          });
        private static void DisposeService(IServiceProvider serviceProvider)
        {
            if (serviceProvider is IDisposable)
            {
                ((IDisposable)serviceProvider).Dispose(); ;
            }
        }
    }
}
