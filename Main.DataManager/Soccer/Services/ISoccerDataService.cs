using System.Threading.Tasks;
using Main.Infrastructure.Enums;

namespace Main.DataManager.Soccer.Services
{
    public interface ISoccerDataService
    {
        Task UpdateResultsData(Country country, string leagueName);
    }
}