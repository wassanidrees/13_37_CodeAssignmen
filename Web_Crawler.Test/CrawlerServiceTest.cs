using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Crawler.Services;
using Web_Crawler.Services.Interfaces;
using Xunit;

namespace Web_Crawler.Test
{
    public class CrawlerServiceTest
    {
        private Mock<IHtmlLinksService> _htmlLinkParserMock;
        private Mock<IWebPageRequestService> _webPageRequestMock;
        private Mock<IFileSaverServices> _directoryAndFileHandlerMock;
        private Mock<ILogger<CrawlerService>> _logger;
        private CrawlerService _crawlerService;

        public CrawlerServiceTest()
        {
            _htmlLinkParserMock = new();
            _webPageRequestMock = new();
            _directoryAndFileHandlerMock = new();
            _logger = new();
            _crawlerService = new CrawlerService(_htmlLinkParserMock.Object, _webPageRequestMock.Object, _directoryAndFileHandlerMock.Object, _logger.Object);
        }
        [Fact(DisplayName ="Start Crawling and each nested method should run once only")]
        public async Task Start_Crawling_And_Each_Nested_Method_Should_Run_Once_Only()
        {
            //Arrange
            _webPageRequestMock.Setup(x => x.Request(It.IsAny<string>())).ReturnsAsync(new Contracts.WebPage() {ContentStream=new byte[0], PageUrl="/"});
            _htmlLinkParserMock.Setup(x => x.ExtractLinks(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(new HashSet<string>());
            _directoryAndFileHandlerMock.Setup(x => x.SaveLocally(It.IsAny<byte[]>(), It.IsAny<string>())).Returns(Task.FromResult(0));
            //Act
            await _crawlerService.StartCrawling();
            //Assert
            _webPageRequestMock.Verify(x => x.Request(It.IsAny<string>()), Times.Once);
            _htmlLinkParserMock.Verify(x => x.ExtractLinks(It.IsAny<byte[]>(), It.IsAny<string>()), Times.Once);
            _directoryAndFileHandlerMock.Verify(x => x.SaveLocally(It.IsAny<byte[]>(), It.IsAny<string>()), Times.Once);
        }
    }
}
