using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Main.Infrastructure.Enums;
using OpenQA.Selenium.Chrome;
using StatsDataSource.Models;
using StatsDataSource.Soccer.Models;

namespace StatsDataSource.Soccer.Services
{
    public class SoccerMatchStatsSiteScrapperService : ISoccerMatchStatsSiteScrapperService
    {
        public IEnumerable<SoccerMatchStatsData> ScrapMatchesStats(IEnumerable<string> matchesStatsUrl)
        {
            using (var webDriver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory))
            {
                var resultsList = new List<SoccerMatchStatsData>(matchesStatsUrl.Count());

                foreach (var matchStatsUrl in matchesStatsUrl)
                {
                    var htmlDocument = InitializeDriverAndGetDocument(webDriver, matchStatsUrl);

                    var matchStats = new SoccerMatchStatsData
                    {
                        MatchDate = DateTime.Parse(htmlDocument.DocumentNode.Descendants().First(n => n.Id == "utime").InnerText),
                        HostsTeam = htmlDocument.DocumentNode.Descendants().First(n => n.HasClass("tname-home")).Descendants("a").First().InnerText,
                        GuestTeam = htmlDocument.DocumentNode.Descendants().First(n => n.HasClass("tname-away")).Descendants("a").First().InnerText
                    };

                    var matchScoreboard = htmlDocument.DocumentNode.Descendants().First(n => n.Id == "event_detail_current_result").InnerText.Split('-');
                    matchStats.MatchResult = (SingleBetResult)matchScoreboard[0].CompareTo(matchScoreboard[1]);

                    var statsRows = htmlDocument.DocumentNode.Descendants().First(n => n.Id == "tab-statistics-0-statistic").ChildNodes.ToList();

                    foreach (var row in statsRows)
                    {
                        var statRowModel = new StatsRow
                        {
                            HostsStatsValue = int.Parse(row.FirstChild.FirstChild.InnerText.TrimEnd('%')),
                            StatsName = row.FirstChild.FirstChild.NextSibling.InnerText,
                            GuestsStatsValue = int.Parse(row.FirstChild.LastChild.InnerText.TrimEnd('%'))
                        };

                        matchStats.StatsRows.Add(statRowModel);
                    }

                    resultsList.Add(matchStats);
                }

                return resultsList;
            }
        }

        private HtmlDocument InitializeDriverAndGetDocument(ChromeDriver webDriver, string matchStatsUrl)
        {
            var html = GoToStatsUrl(webDriver, matchStatsUrl);
            var statsTab = GetStatsTab(webDriver);

            while (statsTab == null)
            {
                html = GoToStatsUrl(webDriver, matchStatsUrl);
                statsTab = GetStatsTab(webDriver);
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            return doc;
        }

        private HtmlNode GetStatsTab(ChromeDriver webDriver)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(webDriver.PageSource);

            return htmlDocument.DocumentNode.Descendants().FirstOrDefault(n => n.Id == "tab-statistics-0-statistic");
        }

        private static string GoToStatsUrl(ChromeDriver webDriver, string matchStatsUrl)
        {
            webDriver.Url = matchStatsUrl;

            return webDriver.PageSource;
        }
    }
}
