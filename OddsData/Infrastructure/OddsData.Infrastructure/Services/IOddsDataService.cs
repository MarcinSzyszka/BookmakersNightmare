using System.Collections.Generic;
using System.Threading.Tasks;
using OddsData.Infrastructure.Enums;
using OddsData.Infrastructure.Models;

namespace OddsData.Infrastructure.Services
{
    public interface IOddsDataService
    {
        Task<IEnumerable<MatchBet>> GetResults(Country country, string leagueName);
    }
}
