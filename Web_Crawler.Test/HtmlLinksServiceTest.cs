using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Crawler.Services;
using Xunit;

namespace Web_Crawler.Test
{
    public class HtmlLinksServiceTest
    {
        [Fact(DisplayName = "Validating link and should return single result")]
        public async Task Validating_Link_And_Should_Return_Single_Result()
        {
            //Arrange 
            HtmlLinksService htmlLinksService = new HtmlLinksService();
            string link = "<a href='../fakelink.css?abc'>path</a>";
            var linkByteArray = System.Text.Encoding.UTF8.GetBytes(link);
            //Act
            var links = await htmlLinksService.ExtractLinks(linkByteArray, "fakelink");
            //Assert
            Assert.Single(links);
        }
        
    }
}
