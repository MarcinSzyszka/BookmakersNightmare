﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Main.Infrastructure.Enums;
using OddsData.Infrastructure.Models;
using OddsData.OddsPortal.Enums;
using OpenQA.Selenium;

namespace OddsData.OddsPortal.Services.Scraper
{
    public class MatchDetailsScraperService : IMatchDetailsScraperService
    {
        public async Task<MatchBet> GetMatchBetDetails(IWebDriver driver, string url, DateTime? fromDate)
        {
            var detailsCol = await GetDetailsColumn(url);

            var teamsNames = GetTeamsNames(detailsCol.ChildNodes.FindFirst("h1"));

            var matchBet = new MatchBet
            {
                HostsTeamName = teamsNames[0],
                GuestsTeamName = teamsNames[1],
            };

            var resultNode = detailsCol.ChildNodes.First(n => n.Id == "event-status").FirstChild;

            var results = GetMatchResults(resultNode);

            if (results == null)
            {
                return null;
            }

            matchBet.MatchDate = GoToPageAndScrapData(url, matchBet.FullTime, results.FullTime, driver, MatchPart.FullTime).Value;

            if (fromDate.HasValue && fromDate >= matchBet.MatchDate)
            {
                return null;
            }

            if (results.FirstHalf > SingleBetResult.Unknown)
            {
                GoToPageAndScrapData($"{url}#1X2;3", matchBet.FirstHalf, results.FirstHalf, driver, MatchPart.FirstHalf);
            }

            if (results.SecondHalf > SingleBetResult.Unknown)
            {
                GoToPageAndScrapData($"{url}#1X2;4", matchBet.SecondHalf, results.SecondHalf, driver, MatchPart.SecondHalf);
            }

            return matchBet;
        }

        private DateTime GetMatchDate(HtmlNode dateParagraph)
        {
            if (!DateTime.TryParse(dateParagraph.InnerText.TrimStart().TrimEnd(), out var date))
            {
                var regex = new Regex(@"^\D+, (.+)");

                var match = regex.Match(dateParagraph.InnerText.TrimStart().TrimEnd());

                if (match.Success)
                {
                    DateTime.TryParse(match.Groups[1].Value, out date);
                }
            }

            return date;
        }

        private DateTime? GoToPageAndScrapData(string url, SingleBet matchPartBet, SingleBetResult matchPartResult, IWebDriver driver, MatchPart matchPart)
        {
            var htmlDoc = new HtmlDocument();

            if (string.IsNullOrEmpty(driver.Url) || driver.Url == "data:,")
            {
                driver.Url = url;
            }
            else
            {
                driver.Navigate().GoToUrl(url);
                driver.Navigate().Refresh();
            }

            htmlDoc.LoadHtml(driver.PageSource);

            var detailsCol = htmlDoc.DocumentNode.Descendants().First(n => n.Id == "col-left").ChildNodes.First(n => n.Id == "col-content");

            var matchPartsOddsList = detailsCol.Descendants("ul").First(n => n.HasClass("sub-menu") && n.HasClass("subactive"));

            if ((int)matchPart < matchPartsOddsList.ChildNodes.Count)
            {
                GetOddsAndFillData(detailsCol, matchPartBet, matchPartResult);

                return GetMatchDate(detailsCol.ChildNodes.FindFirst("p"));
            }

            return null;
        }

        private async Task<HtmlNode> GetDetailsColumn(string url)
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

            matchPart.Odds = odds.Select(o => new SingleBetOdds
            {
                Hosts = o.Hosts,
                Draw = o.Draw,
                Guests = o.Guests
            });
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
                return null;
            }

            var fullTimeResultScoresString = match.Groups[1].Value.Split(':');

            if (match.Groups[2].Success)
            {
                var firstHalfResultScoresString = match.Groups[3].Value.Split(':');
                var secondHalfResultScoresString = match.Groups[4].Value.Split(':');

                return new MatchResultModel
                {
                    FullTime = (SingleBetResult)fullTimeResultScoresString[0].CompareTo(fullTimeResultScoresString[1]),
                    FirstHalf = (SingleBetResult)firstHalfResultScoresString[0].CompareTo(firstHalfResultScoresString[1]),
                    SecondHalf = (SingleBetResult)secondHalfResultScoresString[0].CompareTo(secondHalfResultScoresString[1])
                };
            }

            return new MatchResultModel
            {
                FullTime = (SingleBetResult)fullTimeResultScoresString[0].CompareTo(fullTimeResultScoresString[1]),
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
