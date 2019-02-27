using System.Collections.Generic;
using Prediction.Predicting.Soccer.Models;

namespace Prediction.Predicting.Soccer.Services
{
    public interface ISoccerStatsPredictionService
    {
        List<TeamResultPrediction> PredictResultsDependsOnAllResults(List<string> teamsNames, int matchesToAnalyzeCount);
        List<TeamResultPrediction> PredictResultsDependsOnTeamResults(List<string> teamsNames, int matchesToAnalyzeCount);
    }
}