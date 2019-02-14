using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OddsData.Infrastructure.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace OddsData.OddsPortal.Services.Scraper
{
    public class MatchDetailsScraperService : IMatchDetailsScraperService
    {
        public async Task<MatchBet> GetMatchBetDetails(string url)
        {
            var detailsCol = await GetDetailsColumn(url);

            var teamsNames = GetTeamsNames(detailsCol.ChildNodes.FindFirst("h1"));

            var matchBet = new MatchBet
            {
                HostsTeamName = teamsNames[0],
                GuestsTeamName = teamsNames[1]
            };

            var resultNode = detailsCol.ChildNodes.First(n => n.Id == "event-status").FirstChild;

            var results = GetMatchResults(resultNode);

            using (var driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory))
            {
                GoToPageAndScrapData(url, matchBet.FullTime, results.FullTime, driver);
                GoToPageAndScrapData($"{url}#1X2;3", matchBet.FirstHalf, results.FirstHalf, driver);
                GoToPageAndScrapData($"{url}#1X2;4", matchBet.SecondHalf, results.SecondHalf, driver);
            }

            return matchBet;
        }

        private void GoToPageAndScrapData(string url, SingleBet matchPartBet, SingleBetResult matchPartResult, IWebDriver driver)
        {
            var htmlDoc = new HtmlDocument();

            driver.Navigate().GoToUrl(url);
            driver.Navigate().Refresh();

            htmlDoc.LoadHtml(driver.PageSource);

            var detailsCol = htmlDoc.DocumentNode.Descendants().First(n => n.Id == "col-left").ChildNodes.First(n => n.Id == "col-content");

            GetOddsAndFillData(detailsCol, matchPartBet, matchPartResult);
        }

        private static async Task<HtmlNode> GetDetailsColumn(string url)
        {
            var web = new HtmlWeb();

            var htmlDoc = await web.LoadFromWebAsync(url);

            var detailsCol = htmlDoc.DocumentNode.Descendants().First(n => n.Id == "col-left").ChildNodes.First(n => n.Id == "col-content");

            return detailsCol;
        }

        private void GetOddsAndFillData(HtmlNode detailsCol, SingleBet matchPart, SingleBetResult matchPartResult)
        {
            matchPart.Result = matchPartResult;
            var odds = GetOddsList(detailsCol).ToList();

            matchPart.OddsAverage = new SingleBetOdds
            {
                Hosts = odds.Select(x => x.Hosts).Average(),
                Draw = odds.Select(x => x.Draw).Average(),
                Guests = odds.Select(x => x.Guests).Average()
            };
        }

        private IEnumerable<SingleBetOdds> GetOddsList(HtmlNode detailsCol)
        {
            var oddsRows = detailsCol.Descendants().First(n => n.HasClass("table-main")).Descendants().Where(n => n.Name == "tr" && n.HasClass("lo") && n.HasChildNodes).ToList();

            var oddsList = new List<SingleBetOdds>(oddsRows.Count());

            foreach (var oddsRow in oddsRows)
            {
                var hostsOdds = oddsRow.FirstChild.NextSibling;
                var drawOdds = hostsOdds.NextSibling;
                var guestsOdds = drawOdds.NextSibling;

                oddsList.Add(new SingleBetOdds
                {
                    Hosts = decimal.Parse(hostsOdds.InnerText, CultureInfo.InvariantCulture),
                    Draw = decimal.Parse(drawOdds.InnerText, CultureInfo.InvariantCulture),
                    Guests = decimal.Parse(guestsOdds.InnerText, CultureInfo.InvariantCulture)
                });
            }

            return oddsList;
        }

        private MatchResultModel GetMatchResults(HtmlNode resultNode)
        {
            var regex = new Regex(@"^Final result (\d{1}:\d{1})( \((\d{1}:\d{1}), (\d{1}:\d{1})\))?");

            var match = regex.Match(resultNode.InnerText.TrimStart().TrimEnd());

            if (!match.Success)
            {
                throw new ArgumentException("Could not parse match results from details page!");
            }

            var fullTimeResultScoresString = match.Groups[1].Value.Split(':');

            if (match.Groups[2].Success)
            {
                var firstHalfResultScoresString = match.Groups[3].Value.Split(':');
                var secondHalfResultScoresString = match.Groups[4].Value.Split(':');

                return new MatchResultModel
                {
                    FullTime = (SingleBetResult)string.Compare(fullTimeResultScoresString[0], fullTimeResultScoresString[1], StringComparison.CurrentCulture),
                    FirstHalf = (SingleBetResult)string.Compare(firstHalfResultScoresString[0], firstHalfResultScoresString[1], StringComparison.CurrentCulture),
                    SecondHalf = (SingleBetResult)string.Compare(secondHalfResultScoresString[0], secondHalfResultScoresString[1], StringComparison.CurrentCulture)
                };
            }

            return new MatchResultModel
            {
                FullTime = (SingleBetResult)string.Compare(fullTimeResultScoresString[0], fullTimeResultScoresString[1], StringComparison.Ordinal),
                FirstHalf = SingleBetResult.Unknown,
                SecondHalf = SingleBetResult.Unknown
            };
        }

        private string[] GetTeamsNames(HtmlNode h1WithTeamsNames)
        {
            var regex = new Regex(@"^(.+) - (.+)");

            var match = regex.Match(h1WithTeamsNames.InnerText.TrimStart().TrimEnd());

            if (!match.Success)
            {
                throw new ArgumentException("Could not parse team names from details page!");
            }

            return new[] { match.Groups[1].Value, match.Groups[2].Value };
        }
    }
}
