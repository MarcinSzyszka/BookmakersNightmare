using System.Threading.Tasks;
using Main.Infrastructure.Enums;

namespace Main.DataManager.Soccer.Odds
{
    public interface ISoccerOddsDataService
    {
        Task UpdateResultsData(Country country, string leagueName);
    }
}