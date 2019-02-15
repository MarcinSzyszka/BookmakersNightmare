using Main.Infrastructure.Enums;

namespace Prediction.LearningData.Soccer.Services
{
    public interface ISoccerLearningDataService
    {
        string PrepareFirstHalfDataAndGetResultsFilePath();
        string PrepareFirstHalfDataAndGetResultsFilePath(Country country, string league);
        string PrepareFullTimeDataAndGetResultsFilePath();
        string PrepareFullTimeDataAndGetResultsFilePath(Country country, string league);
        string PrepareSecondHalfDataAndGetResultsFilePath();
        string PrepareSecondHalfDataAndGetResultsFilePath(Country country, string league);
    }
}