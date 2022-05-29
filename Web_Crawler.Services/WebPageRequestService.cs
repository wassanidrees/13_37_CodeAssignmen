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
        public WebPageRequestService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Constants.BaseURL);
        }
        public async Task<WebPage> Request(string link)
        {
            try
            {
                var response = await _httpClient.GetAsync(link);
                response.EnsureSuccessStatusCode();
                return new WebPage() { ContentStream = await response.Content.ReadAsByteArrayAsync(), PageUrl = link };
            }
            catch (HttpRequestException e)
            {
               return new WebPage() { ContentStream = null, PageUrl = string.Empty };
            }
        }
    }
}
