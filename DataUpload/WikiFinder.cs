using DataUpload.Data;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace DataUpload
{
    public static class WikiFinder
    {
        private static string _wikiLink = "https://en.wikipedia.org/wiki/";

        public static List<string> FindWikiUrls(Ship ship)
        {
            var url = $"https://www.google.com/search?q=wikipedia+warship+{ship.Nation}+{ship.ClassType}+{ship.ClassName}";

            if (ship.Launched != null)
                url += $"+{ship.Launched}";
            else if(ship.Completed != null)
                url += $"+{ship.Completed}";
            
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(url);

            var wikiLinks = new List<string>();
            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
				string urlMatch = link.Attributes["href"].Value;

                if (urlMatch?.StartsWith(_wikiLink) == true)
                    wikiLinks.Add(urlMatch);
            }
            return wikiLinks;
        }
    }
}