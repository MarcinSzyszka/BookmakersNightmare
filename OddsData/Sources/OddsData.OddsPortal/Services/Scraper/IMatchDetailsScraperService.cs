using System;
using System.Threading.Tasks;
using OddsData.Infrastructure.Models;
using OpenQA.Selenium;

namespace OddsData.OddsPortal.Services.Scraper
{
    public interface IMatchDetailsScraperService
    {
        Task<MatchBet> GetMatchBetDetails(IWebDriver driver, string url, DateTime? fromDate);
    }
}