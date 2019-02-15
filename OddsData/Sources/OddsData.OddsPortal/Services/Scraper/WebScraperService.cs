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
        private string _countryLeagueResultsUrl;
        private string _baseUrl;
        private ChromeDriver _driver;
        private DateTime? _fromDate;

        public WebScraperService(IMatchDetailsScraperService matchDetailsScrapper)
        {
            _matchDetailsScrapper = matchDetailsScrapper;
        }

        public async Task<IEnumerable<MatchBet>> GetMatchBetsWithResultsInLatestSeason(string baseUrl, CountryLeague countryLeague, DateTime? fromDate)
        {
            InitWebDriver();
            _baseUrl = baseUrl;
            _countryLeagueResultsUrl = $"{baseUrl}/soccer/{countryLeague.Country}/{countryLeague.League}/results/".ToLower();
            _fromDate = fromDate;

            var pagesCount = GetResultsPagesCount();

            var matchBetsResults = new List<MatchBet>();

            for (var i = 1; i <= pagesCount; i++)
            {
                matchBetsResults.AddRange(await GetMatchesDetails(i));
            }

            return matchBetsResults;
        }

        private int GetResultsPagesCount()
        {
            var htmlDoc = GetHtmlDoc(_countryLeagueResultsUrl);

            var pagesCount = 1;

            var paginationNode = htmlDoc.DocumentNode.Descendants("div").FirstOrDefault(n => n.Id == "pagination");
            if (paginationNode != null)
            {
                pagesCount = paginationNode.ChildNodes.Select(n => n.GetAttributeValue("x-page", 1)).OrderBy(x => x).Distinct().Count();
            }

            return pagesCount;
        }

        private async Task<IEnumerable<MatchBet>> GetMatchesDetails(int page)
        {
            var htmlDoc = GetHtmlDoc($"{_countryLeagueResultsUrl}#/page/{page}/");

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

            return await GetDetailsFromRows(resultsRows);
        }

        private List<HtmlNode> GetResultRows(HtmlNode tournamentTable)
        {
            return tournamentTable.Descendants("tr").Where(n => n.HasClass("deactivate")).ToList();
        }

        private async Task<IEnumerable<MatchBet>> GetDetailsFromRows(List<HtmlNode> resultsRows)
        {
            var tasks = new List<Task<IEnumerable<MatchBet>>>();

            var bulkParts = resultsRows.Count / Environment.ProcessorCount;

            for (var i = 0; i <= Environment.ProcessorCount; i++)
            {
                tasks.Add(Task.Run(async () => await GetMatchDetails(resultsRows.Take(bulkParts))));
            }

            var tasksResults = await Task.WhenAll(tasks.ToArray());

            return tasksResults.SelectMany(m => m);
        }

        private async Task<IEnumerable<MatchBet>> GetMatchDetails(IEnumerable<HtmlNode> rows)
        {
            var result = new List<MatchBet>(rows.Count());

            using (var webDriver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory))
            {
                foreach (var row in rows)
                {
                    var matchDetailsUrl = row.ChildNodes.First(n => n.HasClass("table-participant")).ChildNodes.First().GetAttributeValue("href", null);

                    var matchDetails = await _matchDetailsScrapper.GetMatchBetDetails(webDriver, $"{_baseUrl}{matchDetailsUrl}", _fromDate);
                    
                    if (matchDetails == null)
                    {
                        break;
                    }

                    result.Add(matchDetails);
                }
            }

            return result.Where(r => r != null);
        }

        private HtmlDocument GetHtmlDoc(string url)
        {
            _driver.Url = url;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(_driver.PageSource);

            return htmlDoc;
        }

        private void InitWebDriver()
        {
            _driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory);
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
