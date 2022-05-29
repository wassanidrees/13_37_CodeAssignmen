using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Web_Crawler.Services;
using Xunit;

namespace Web_Crawler.Test
{
    public class WebPageRequestServiceTest
    {
        private Mock<HttpClient> _httpClientMock;
        private Mock<ILogger<WebPageRequestService>> _logger;
        WebPageRequestService webPageRequestService;
        public WebPageRequestServiceTest()
        {
            _httpClientMock = new();
            _logger = new();
            webPageRequestService = new(_logger.Object, _httpClientMock.Object);
        }
        [Fact(DisplayName = "Request Root URL and reponse should not be null")]
        public async Task Request_Root_Url_and_Response_Should_Not_Be_Null()
        {
            //Arrange
            _httpClientMock.Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    return response;
                });
            
            //Act
            var result = await webPageRequestService.Request("/");

            //Assert
            Assert.NotNull(result);

        }
    }
}
