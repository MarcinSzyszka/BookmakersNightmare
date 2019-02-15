using System;
using System.Threading.Tasks;
using Main.Infrastructure.Enums;
using OddsData.Infrastructure.Models;
using OddsData.Infrastructure.Services;

namespace OddsData.Source
{
    public class OddsDataSourceService : IOddsDataSourceService
    {
        private readonly IOddsDataService _oddsDataService;

        public OddsDataSourceService(IOddsDataService oddsDataService)
        {
            _oddsDataService = oddsDataService;
        }

        public async Task<GetOddsDataResult> GetData(Country country, string leagueName, DateTime? fromDate)
        {
            try
            {
                var result = await _oddsDataService.GetResults(country, leagueName, fromDate);

                return new GetOddsDataResult(true, country, leagueName, result);
            }
            catch (Exception e)
            {
                return new GetOddsDataResult(false, e.Message);
            }
        }
    }
}
