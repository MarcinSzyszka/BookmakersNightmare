using System.Threading.Tasks;
using OddsData.Infrastructure.Models;

namespace OddsData.OddsPortal.Services.Scraper
{
    public interface IMatchDetailsScraperService
    {
        Task<MatchBet> GetMatchBetDetails(string url);
    }
}