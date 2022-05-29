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
        private Mock<IHtmlLinksService> _htmlLinksServiceMock;
        private Mock<IWebPageRequestService> _webPageRequestServiceMock;
        private Mock<IFileSaverServices> _fileSaverServicesMock;
        private Mock<ILogger<CrawlerService>> _logger;
        private CrawlerService _crawlerService;

        public CrawlerServiceTest()
        {
            _htmlLinksServiceMock = new();
            _webPageRequestServiceMock = new();
            _fileSaverServicesMock = new();
            _logger = new();
            _crawlerService = new CrawlerService(_htmlLinksServiceMock.Object, _webPageRequestServiceMock.Object, _fileSaverServicesMock.Object, _logger.Object);
        }
        [Fact(DisplayName ="Start Crawling and each nested method should run once only")]
        public async Task Start_Crawling_And_Each_Nested_Method_Should_Run_Once_Only()
        {
            //Arrange
            _webPageRequestServiceMock.Setup(x => x.Request(It.IsAny<string>())).ReturnsAsync(new Contracts.WebPage() {ContentStream=new byte[0], PageUrl="/"});
            _htmlLinksServiceMock.Setup(x => x.ExtractLinks(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(new HashSet<string>());
            _fileSaverServicesMock.Setup(x => x.SaveLocally(It.IsAny<byte[]>(), It.IsAny<string>())).Returns(Task.FromResult(0));
            //Act
            await _crawlerService.StartCrawling();
            //Assert
            _webPageRequestServiceMock.Verify(x => x.Request(It.IsAny<string>()), Times.Once);
            _htmlLinksServiceMock.Verify(x => x.ExtractLinks(It.IsAny<byte[]>(), It.IsAny<string>()), Times.Once);
            _fileSaverServicesMock.Verify(x => x.SaveLocally(It.IsAny<byte[]>(), It.IsAny<string>()), Times.Once);
        }
    }
}
