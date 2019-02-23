using System;
using Microsoft.ML;
using Microsoft.ML.Data;
using Prediction.Training.Soccer.Models;

namespace Prediction.Training.Soccer
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
                        new TextLoader.Column("BallPossession", DataKind.R4, 1),
                        new TextLoader.Column("AttacksOnGoal", DataKind.R4, 2),
                        new TextLoader.Column("ShotsOnGoal", DataKind.R4, 3),
                        new TextLoader.Column("ShotsOutGoal", DataKind.R4, 4),
                        new TextLoader.Column("Corners", DataKind.R4, 5),
                        new TextLoader.Column("Passes", DataKind.R4, 6),
                        new TextLoader.Column("AccuratePasses", DataKind.R4, 7),
                        new TextLoader.Column("Blocks", DataKind.R4, 8),
                        new TextLoader.Column("Points", DataKind.R4, 9)
                    }
            }
            );

            var dataView = textLoader.Read(trainingDataPath);

            var pipeline = _mlContext.Transforms.CopyColumns(inputColumnName: "Result", outputColumnName: "Label")
                .Append(_mlContext.Transforms.Concatenate("Features",
                    "BallPossession", "AttacksOnGoal", "ShotsOnGoal", "ShotsOutGoal", "Corners", "Passes", "AccuratePasses", "Blocks", "Points"))
                .Append(_mlContext.Regression.Trainers.FastTreeTweedie());

            var model = pipeline.Fit(dataView);

            var predictionFunction = model.CreatePredictionEngine<StatsResult, StatsResultPrediction>(_mlContext);

            var taxiTripSample = new StatsResult
            {
                BallPossession = 138+43 - 42,
                AttacksOnGoal = 26 + 12-14,
                ShotsOnGoal = 10 + 6-5,
                ShotsOutGoal = 14+3-7,
                Corners = 8+6-3,
                Passes = 0,
                AccuratePasses = 900+290-220,
                Blocks = 20+15-23,
                Points = 5 + 3 -1
            };

            var prediction = predictionFunction.Predict(taxiTripSample);

            Console.WriteLine($"Predicted result: {prediction.Result}");
        }
    }
}
