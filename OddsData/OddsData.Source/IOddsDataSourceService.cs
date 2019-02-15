using System;
using System.Threading.Tasks;
using Main.Infrastructure.Enums;
using OddsData.Infrastructure.Models;

namespace OddsData.Source
{
    public interface IOddsDataSourceService
    {
        Task<GetOddsDataResult> GetData(Country country, string leagueName, DateTime? fromDate);
    }
}