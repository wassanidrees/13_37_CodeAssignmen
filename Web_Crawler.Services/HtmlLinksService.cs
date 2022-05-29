using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Web_Crawler.Services.Interfaces;

namespace Web_Crawler.Services
{
    public class HtmlLinksService : IHtmlLinksService
    {
        public async Task<HashSet<string>> ExtractLinks(byte[] PageContentStream, string PageUrl)
        {
            HashSet<string> linksList = new HashSet<string>();
            var html = System.Text.Encoding.UTF8.GetString(PageContentStream);
            var matchResult = Regex.Matches(html, @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))", RegexOptions.IgnoreCase);
            var sourceTags = Regex.Matches(html, " src=\"(?'url'[^\"]*)|  href=\"(?'url'[^\"]*)", RegexOptions.IgnoreCase);
            linksList.UnionWith(await ValidateLinks(matchResult, PageUrl));
            linksList.UnionWith(await ValidateLinks(sourceTags, PageUrl));
            return linksList;
        }
        private Task<HashSet<string>> ValidateLinks(MatchCollection matchResult, string PageUrl)
        {
            HashSet<string> ValidatedLinksResult = new HashSet<string>();

            for (int i = 0; i < matchResult.Count; i++)
            {
                Match m = matchResult[i];
                string link = m.Groups[1].Value.Trim();
                if (string.IsNullOrEmpty(link)) continue;
                if (link.Contains("#")
                    || link.StartsWith("http")
                    || link.StartsWith("//")
                    || link.StartsWith("www")
                    || link.StartsWith("javascript")
                    || link.StartsWith("mailto")
                    || link.StartsWith("tel")
                    || link.StartsWith("sms")
                    || link.Contains("{")) { continue; }


                if (link.StartsWith("/"))
                {
                    link = link.TrimStart('/');
                }
                if (Path.GetExtension(link).ToLowerInvariant() == ".html")
                {
                    link = Path.ChangeExtension(link, null);
                }
                if (link.StartsWith(".."))
                {
                    int level = 0;
                    while (link.Substring(level * 3, 3) == $"../")
                    {
                        level++;
                    }
                    var patharray = PageUrl.Split(Path.DirectorySeparatorChar);
                    int levelsToKeep = patharray.Length - level - 1;

                    var finalComp = patharray.Take(levelsToKeep).Append(link.Substring(level * 3));

                    link = Path.Combine(finalComp.ToArray());
                }
                if (link.Contains('?'))
                {
                    link = link.Substring(0, link.IndexOf('?'));
                }
                if (string.IsNullOrEmpty(link)) { continue; }
                ValidatedLinksResult.Add(link);
            }
            return Task.FromResult(ValidatedLinksResult);
        }
    }
}
