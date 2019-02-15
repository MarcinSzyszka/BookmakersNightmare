using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Main.Infrastructure.Enums;
using OddsData.Infrastructure.Models;

namespace OddsData.Infrastructure.Services
{
    public interface IOddsDataService
    {
        Task<IEnumerable<MatchBet>> GetResults(Country country, string leagueName, DateTime? fromDate);
    }
}
