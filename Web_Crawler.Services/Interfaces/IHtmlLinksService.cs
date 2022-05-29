using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web_Crawler.Services.Interfaces
{
    public interface IHtmlLinksService
    {
        Task<HashSet<string>> ExtractLinks(byte[] PageContentStream, string PageUrl);
    }
}
