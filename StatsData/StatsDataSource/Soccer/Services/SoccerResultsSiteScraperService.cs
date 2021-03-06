﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using StatsDataSource.Soccer.Models;

namespace StatsDataSource.Soccer.Services
{
    public class SoccerResultsSiteScraperService : ISoccerResultsSiteScraperService
    {
        private readonly string _mainUrl;

        private readonly ISoccerMatchStatsSiteScrapperService _soccerMatchStatsSiteScrapperService;

        public SoccerResultsSiteScraperService(ISoccerMatchStatsSiteScrapperService soccerMatchStatsSiteScrapperService)
        {
            _soccerMatchStatsSiteScrapperService = soccerMatchStatsSiteScrapperService;
            _mainUrl = $"{ConfigurationManager.AppSettings["FlashScoreUrl"]}";
        }

        public async Task<IEnumerable<SoccerMatchStatsData>> GetLeagueResults(string country, string league, DateTime? afterDate)
        {
            var webHtml = new HtmlDocument();

            using (var webDriver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory)
            {
                Url = $"{_mainUrl}/pilka-nozna/{country}/{league}/wyniki"
            })
            {
                await ClickLoadMoreResultsIfAvailable(webDriver);

                webHtml.LoadHtml(webDriver.PageSource);
            }

            var resultsRows = webHtml.DocumentNode.Descendants("tr").Where(tr => tr.HasClass("stage-finished")).ToList();

            var matchesIds = new List<string>(resultsRows.Count);

            foreach (var row in resultsRows)
            {
                var date = GetMatchDate(row);

                if (afterDate.HasValue && afterDate.Value > date)
                {
                    continue;
                }

                matchesIds.Add(row.Id.Replace("g_1_", string.Empty));
            }

            var matchesUrls = matchesIds.Select(id => $"{_mainUrl}/mecz/{id}/#statystyki-meczu;0").ToList();

            var divider = 4;

            var bulkParts = matchesUrls.Count / divider;

            var tasksList = new List<Task<IEnumerable<SoccerMatchStatsData>>>(divider + 1);

            for (var i = 0; i < divider + 1; i++)
            {
                var urlsPart = matchesUrls.Skip(i * bulkParts).Take(bulkParts).ToList();

                tasksList.Add(Task.Run(() => _soccerMatchStatsSiteScrapperService.ScrapMatchesStats(urlsPart, afterDate)));
            }

            var results = await Task.WhenAll(tasksList.ToArray());

            return results.SelectMany(r => r).ToList();
        }

        private DateTime GetMatchDate(HtmlNode row)
        {
            var timeCell = row.ChildNodes.First(n => n.HasClass("time"));

            var regex = new Regex(@"^\d{1,2}\.\d{1,2}\.");

            var matchResult = regex.Match(timeCell.InnerText.TrimStart().TrimEnd());

            var date = DateTime.MinValue;

            if (matchResult.Success)
            {
                date = DateTime.Parse(matchResult.Value + DateTime.Now.Year);
            }

            return date;
        }

        private async Task ClickLoadMoreResultsIfAvailable(ChromeDriver webDriver)
        {
            var loadMoreElem = GetMoreResultsElement(webDriver);

            while (loadMoreElem != null)
            {
                ((IJavaScriptExecutor)webDriver).ExecuteScript("window.scrollTo(0,document.body.scrollHeight);");
                loadMoreElem.Click();

                await Task.Delay(5000);

                loadMoreElem = GetMoreResultsElement(webDriver);
            }
        }

        private IWebElement GetMoreResultsElement(ChromeDriver webDriver)
        {
            try
            {
                return webDriver.FindElement(By.LinkText("Pokaż więcej meczów"));

            }
            catch
            {
                return null;
            }
        }
    }
}
