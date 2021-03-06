﻿namespace Prediction.LearningData.Soccer.Services
{
    public interface ISoccerStatsLearningDataService
    {
        string PrepareLearningDataAndGetResultsFilePath(int matchesBeforeCount);
        string PrepareLearningDataAndGetResultsFilePath(string teamName, int matchesBeforeCount);
        string PrepareLearningDataAndGetResultsFilePath(string country, string league, int matchesBeforeCount);
    }
}