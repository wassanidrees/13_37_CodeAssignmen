using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Crawler.Contracts;
using Web_Crawler.Services.Interfaces;

namespace Web_Crawler.Services
{
    public class CrawlerService : ICrawlerService
    {
        private IHtmlLinksService _htmlLinkParser;
        private IWebPageRequestService _webPageRequest;
        private ConcurrentBag<string> _linkTracker;
        private IDirectoryAndFileHandlerService _directoryAndFileHandler;
        public CrawlerService(IHtmlLinksService htmlLinkParser, IWebPageRequestService webPageRequest, IDirectoryAndFileHandlerService directoryAndFileHandler)
        {
            _htmlLinkParser = htmlLinkParser;
            _webPageRequest = webPageRequest;
            _directoryAndFileHandler = directoryAndFileHandler;
            _linkTracker = new ConcurrentBag<string>();
        }
        public async Task StartCrawling()
        {
            await ExecuteCrawling(new HashSet<string>() { "/" });
        }
        private async Task ExecuteCrawling(HashSet<string> links)
        {
            var pageRequestTasks = new List<Task<WebPage>>();
            var filehandlerTask = new List<Task>();
            foreach (var currentLink in links)
            {
                pageRequestTasks.Add(_webPageRequest.Request(currentLink));
                _linkTracker.Add(currentLink);
            }
            var RequestsTaskResult = await Task.WhenAll(pageRequestTasks);
            var foundLinks = new HashSet<string>();
            foreach (var item in RequestsTaskResult.Where(i => i.PageUrl != string.Empty))
            {
                foundLinks.UnionWith(await _htmlLinkParser.ExtractLinks(item.ContentStream, item.PageUrl));
                filehandlerTask.Add(_directoryAndFileHandler.SaveLocally(item.ContentStream, item.PageUrl));
            }
            foundLinks = foundLinks.Except(_linkTracker).ToHashSet();

            if (foundLinks.Count > 0)
            {
                await ExecuteCrawling(foundLinks);
            }
            await Task.WhenAll(filehandlerTask);
        }
    }
}

