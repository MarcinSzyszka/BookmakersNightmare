using System;
using System.Threading.Tasks;
using OddsData.Infrastructure.Enums;
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

        public async Task<GetOddsDataResult> GetData(Country country, string leagueName)
        {
            try
            {
                var result = await _oddsDataService.GetResults(country, leagueName);

                return new GetOddsDataResult(true, result, "Success!");
            }
            catch (Exception e)
            {
                return new GetOddsDataResult(false, null, e.Message);
            }
        }
    }
}
