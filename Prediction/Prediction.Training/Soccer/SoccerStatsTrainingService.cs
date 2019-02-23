using Microsoft.ML;
using Microsoft.ML.Data;
using Prediction.Training.Soccer.Models;

namespace Prediction.Training.Soccer
{
    public class SoccerStatsTrainingService : ISoccerStatsTrainingService
    {
        public TrainingResult Train(string trainingDataPath)
        {
            var mlContext = new MLContext(seed: 0);

            var textLoader = mlContext.Data.CreateTextLoader(new TextLoader.Arguments
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

            var pipeline = mlContext.Transforms.CopyColumns(inputColumnName: "Result", outputColumnName: "Label")
                .Append(mlContext.Transforms.Concatenate("Features",
                    "BallPossession", "AttacksOnGoal", "ShotsOnGoal", "ShotsOutGoal", "Corners", "Passes", "AccuratePasses", "Blocks", "Points"))
                .Append(mlContext.Regression.Trainers.FastTreeTweedie());

            var model = pipeline.Fit(dataView);
            
            return new TrainingResult
            {
                MlContext = mlContext,
                Model = model
            };
        }
    }
}
