using System;
using Microsoft.ML;
using Microsoft.ML.Data;
using Prediction.Training.Soccer.Stats.Models;

namespace Prediction.Training.Soccer.Stats
{
    public class SoccerStatsTrainingService : ISoccerStatsTrainingService
    {
        private readonly MLContext _mlContext;

        public SoccerStatsTrainingService()
        {
            _mlContext = new MLContext(seed: 0);
        }

        public void Train(string trainingDataPath)
        {
            var textLoader = _mlContext.Data.CreateTextLoader(new TextLoader.Arguments
            {
                Separators = new[] { ',' },
                HasHeader = true,
                Column = new[]
                    {
                        new TextLoader.Column("Result", DataKind.R4, 0),
                        new TextLoader.Column("HostsBallPossession", DataKind.R4, 1),
                        new TextLoader.Column("HostsAttacksOnGoal", DataKind.R4, 2),
                        new TextLoader.Column("HostsShotsOnGoal", DataKind.R4, 3),
                        new TextLoader.Column("HostsShotsOutGoal", DataKind.R4, 4),
                        new TextLoader.Column("HostsCorners", DataKind.R4, 5),
                        new TextLoader.Column("HostsAccuratePasses", DataKind.R4, 6),
                        new TextLoader.Column("HostsBlocks", DataKind.R4, 7),
                        new TextLoader.Column("GuestsBallPossession", DataKind.R4, 8),
                        new TextLoader.Column("GuestsAttacksOnGoal", DataKind.R4, 9),
                        new TextLoader.Column("GuestsShotsOnGoal", DataKind.R4, 10),
                        new TextLoader.Column("GuestsShotsOutGoal", DataKind.R4, 11),
                        new TextLoader.Column("GuestsCorners", DataKind.R4, 12),
                        new TextLoader.Column("GuestsAccuratePasses", DataKind.R4, 13),
                        new TextLoader.Column("GuestsBlocks", DataKind.R4, 14)
                    }
            }
            );

            var dataView = textLoader.Read(trainingDataPath);

            var pipeline = _mlContext.Transforms.CopyColumns(inputColumnName: "Result", outputColumnName: "Label")
                .Append(_mlContext.Transforms.Concatenate("Features",
                    "HostsBallPossession", "HostsAttacksOnGoal", "HostsShotsOnGoal", "HostsShotsOutGoal", "HostsCorners", "HostsAccuratePasses", "HostsBlocks",
                                        "GuestsBallPossession", "GuestsAttacksOnGoal", "GuestsShotsOnGoal", "GuestsShotsOutGoal", "GuestsCorners", "GuestsAccuratePasses", "GuestsBlocks"))
                .Append(_mlContext.Regression.Trainers.FastTree());

            var model = pipeline.Fit(dataView);

            var predictionFunction = model.CreatePredictionEngine<StatsResult, StatsResultPrediction>(_mlContext);

            var taxiTripSample = new StatsResult
            {
                HostsBallPossession = 153,
                HostsAttacksOnGoal = 36,
                HostsShotsOnGoal = 21,
                HostsShotsOutGoal = 15,
                HostsCorners = 26,
                HostsAccuratePasses = 1259,
                HostsBlocks = 54,
                GuestsBallPossession = 147,
                GuestsAttacksOnGoal = 24,
                GuestsShotsOnGoal = 19,
                GuestsShotsOutGoal = 5,
                GuestsCorners = 11,
                GuestsAccuratePasses = 1139,
                GuestsBlocks = 48
                };

            var prediction = predictionFunction.Predict(taxiTripSample);

            Console.WriteLine($"Predicted result: {prediction.Result}");
        }
    }
}
