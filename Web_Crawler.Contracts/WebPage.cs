using System;

namespace Web_Crawler.Contracts
{
    public class WebPage
    {
        public byte[] ContentStream { get; set; }
        public string PageUrl { get; set; }
    }
}
