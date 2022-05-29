using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Crawler.Contracts;

namespace Web_Crawler.Services.Interfaces
{
    public interface IWebPageRequestService
    {
        Task<WebPage> Request(string link);
    }
}
