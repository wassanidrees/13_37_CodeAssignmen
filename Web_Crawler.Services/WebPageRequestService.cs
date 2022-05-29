using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Web_Crawler.Contracts;
using Web_Crawler.Services.Interfaces;

namespace Web_Crawler.Services
{
    public class WebPageRequestService : IWebPageRequestService
    {
        private HttpClient _httpClient;
        private ILogger _logger;
        public WebPageRequestService(ILogger<WebPageRequestService> logger)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Constants.BaseURL);
            _logger = logger;
        }
        public async Task<WebPage> Request(string link)
        {
            try
            {
                _logger.LogInformation("Page downloading started : {0}{1}", _httpClient.BaseAddress, link);
                var response = await _httpClient.GetAsync(link);
                _logger.LogInformation("Downloading finished with request code {0}",response.StatusCode);
                response.EnsureSuccessStatusCode();

                return new WebPage() { ContentStream = await response.Content.ReadAsByteArrayAsync(), PageUrl = link };
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("Error downloading link {0}",e.Message);

                return new WebPage() { ContentStream = null, PageUrl = string.Empty };
            }
        }
    }
}
