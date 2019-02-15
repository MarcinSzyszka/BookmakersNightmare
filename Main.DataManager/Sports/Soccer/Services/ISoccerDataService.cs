using System.Threading.Tasks;
using Main.Infrastructure.Enums;

namespace Main.DataManager.Sports.Soccer.Services
{
    public interface ISoccerDataService
    {
        Task UpdateResultsData(Country country, string leagueName);
    }
}