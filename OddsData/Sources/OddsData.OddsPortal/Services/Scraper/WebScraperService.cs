using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OddsData.Infrastructure.Models;
using OpenQA.Selenium.Chrome;

namespace OddsData.OddsPortal.Services.Scraper
{
    public class WebScraperService : IWebScraperService
    {
        private readonly IMatchDetailsScraperService _matchDetailsScrapper;

        public WebScraperService(IMatchDetailsScraperService matchDetailsScrapper)
        {
            _matchDetailsScrapper = matchDetailsScrapper;
        }

        public async Task<IEnumerable<MatchBet>> GetMatchBetsWithResultsInLatestSeason(string url, CountryLeague countryLeague)
        {
            var countryLeagueResultsUrl = $"{url}/soccer/{countryLeague.Country}/{countryLeague.League}/results/".ToLower();

            var htmlDoc = GetHtmlDoc(countryLeagueResultsUrl);

            var pagesCount = 1;

            var paginationNode = htmlDoc.DocumentNode.Descendants("div").FirstOrDefault(n => n.Id == "pagination");
            if (paginationNode != null)
            {
                pagesCount = paginationNode.ChildNodes.Select(n => n.GetAttributeValue("x-page", 1)).OrderBy(x => x).Distinct().Count();
            }
            var matchBetsResults = new List<MatchBet>();

            matchBetsResults.AddRange(await GetMatchesDetails(url, htmlDoc));

            for (int i = 1; i < pagesCount; i++)
            {
                htmlDoc = GetHtmlDoc($"{countryLeagueResultsUrl}#/page/{i}/");

                matchBetsResults.AddRange(await GetMatchesDetails(url, htmlDoc));
            }

            return matchBetsResults;
        }

        private async Task<IEnumerable<MatchBet>> GetMatchesDetails(string url, HtmlDocument htmlDoc)
        {
            var tournamentTable = htmlDoc.DocumentNode.Descendants("table").FirstOrDefault(n => n.Id == "tournamentTable");

            if (tournamentTable == null)
            {
                throw new ArgumentNullException("Could not find the results for specified country and league. Check if country and league are correct.");
            }

            var resultsRows = GetResultRows(tournamentTable);

            if (!resultsRows.Any())
            {
                throw new ArgumentNullException("There is no data for the latest season");
            }

            return await GetDetailsFromRows(url, resultsRows);
        }

        private static HtmlDocument GetHtmlDoc(string countryLeagueResultsUrl)
        {
            var htmlDoc = new HtmlDocument();

            using (var driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory)
            {
                Url = countryLeagueResultsUrl
            })
            {
                htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(driver.PageSource);
            }

            return htmlDoc;
        }

        private List<HtmlNode> GetResultRows(HtmlNode tournamentTable)
        {
            return tournamentTable.Descendants("tr").Where(n => n.HasClass("deactivate")).ToList();
        }

        private async Task<IEnumerable<MatchBet>> GetDetailsFromRows(string url, List<HtmlNode> resultsRows)
        {
            var tasks = new List<Task<MatchBet>>();
            //var result = new List<MatchBet>();

            foreach (var row in resultsRows)
            {
                var matchDetailsUrl = row.ChildNodes.First(n => n.HasClass("table-participant")).ChildNodes.First().GetAttributeValue("href", null);

                tasks.Add(_matchDetailsScrapper.GetMatchBetDetails($"{url}{matchDetailsUrl}"));
            }

            return await Task.WhenAll(tasks.ToArray());
        }
    }
}
