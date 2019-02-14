using System.Threading.Tasks;
using OddsData.Infrastructure.Enums;
using OddsData.Infrastructure.Models;

namespace OddsData.Source
{
    public interface IOddsDataSourceService
    {
        Task<GetOddsDataResult> GetData(Country country, string leagueName);
    }
}