using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Prediction.LearningData.Soccer.Services;
using Prediction.Predicting.Soccer.Models;
using Prediction.Training.Soccer;
using Prediction.Training.Soccer.Models;

namespace Prediction.Predicting.Soccer.Services
{
    public class SoccerStatsPredictionService : ISoccerStatsPredictionService
    {
        private readonly ISoccerTeamLastStatsDataService _soccerTeamLastStatsDataService;
        private readonly ISoccerStatsLearningDataService _soccerStatsLearningDataService;
        private readonly ISoccerStatsTrainingService _soccerStatsTrainingService;

        public SoccerStatsPredictionService(ISoccerTeamLastStatsDataService soccerTeamLastStatsDataService, ISoccerStatsLearningDataService soccerStatsLearningDataService, ISoccerStatsTrainingService soccerStatsTrainingService)
        {
            _soccerTeamLastStatsDataService = soccerTeamLastStatsDataService;
            _soccerStatsLearningDataService = soccerStatsLearningDataService;
            _soccerStatsTrainingService = soccerStatsTrainingService;
        }

        public List<TeamResultPrediction> PredictResultsDependsOnTeamResults(List<string> teamsNames, int matchesToAnalyzeCount)
        {
            var result = new List<TeamResultPrediction>(teamsNames.Count());

            foreach (var teamName in teamsNames)
            {
                var trainingResult = Train(teamName, matchesToAnalyzeCount);
                
                result.Add(Predict(teamName, matchesToAnalyzeCount, trainingResult));
            }

            return result;
        }

        public List<TeamResultPrediction> PredictResultsDependsOnAllResults(List<string> teamsNames, int matchesToAnalyzeCount)
        {
            var trainingResult = Train(matchesToAnalyzeCount);

            var result = new List<TeamResultPrediction>(teamsNames.Count());

            foreach (var teamName in teamsNames)
            {
                result.Add(Predict(teamName, matchesToAnalyzeCount, trainingResult));
            }

            return result;
        }

        private TeamResultPrediction Predict(string teamName, int matchesToAnalyzeCount, TrainingResult trainingResult)
        {
            var lastMatchesStats = _soccerTeamLastStatsDataService.GetTeamLastStats(teamName, matchesToAnalyzeCount);

            var statsResult = new StatsResult
            {
                BallPossession = lastMatchesStats.Sum(s => s.BallPossession),
                AttacksOnGoal = lastMatchesStats.Sum(s => s.AttacksOnGoal),
                ShotsOnGoal = lastMatchesStats.Sum(s => s.ShotsOnGoal),
                ShotsOutGoal = lastMatchesStats.Sum(s => s.ShotsOutGoal),
                Corners = lastMatchesStats.Sum(s => s.Corners),
                Passes = lastMatchesStats.Sum(s => s.Passes),
                AccuratePasses = lastMatchesStats.Sum(s => s.AccuratePasses),
                Blocks = lastMatchesStats.Sum(s => s.Blocks),
                Points = lastMatchesStats.Sum(s => s.ResultPoints)
            };

            var predictionFunction = trainingResult.Model.CreatePredictionEngine<StatsResult, StatsResultPrediction>(trainingResult.MlContext);

            var prediction = predictionFunction.Predict(statsResult);

            return new TeamResultPrediction { TeamName = teamName, Result = prediction.Result };
        }

        private TrainingResult Train(int matchesToAnalyzeCount)
        {
            var learningDataPath = _soccerStatsLearningDataService.PrepareLearningDataAndGetResultsFilePath(matchesToAnalyzeCount);
            var mlModel = _soccerStatsTrainingService.Train(learningDataPath);

            return mlModel;
        }

        private TrainingResult Train(string teamName, int matchesToAnalyzeCount)
        {
            var learningDataPath = _soccerStatsLearningDataService.PrepareLearningDataAndGetResultsFilePath(teamName, matchesToAnalyzeCount);
            var mlModel = _soccerStatsTrainingService.Train(learningDataPath);

            return mlModel;
        }
    }
}
