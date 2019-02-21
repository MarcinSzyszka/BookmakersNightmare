using System;
using Microsoft.ML;
using Microsoft.ML.Data;
using Prediction.Training.Soccer.Odds.Models;

namespace Prediction.Training.Soccer.Odds
{
    public class SoccerOddsTrainingService : ISoccerOddsTrainingService
    {
        private readonly MLContext _mlContext;

        public SoccerOddsTrainingService()
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
                        new TextLoader.Column("OddsHosts", DataKind.R4, 0),
                        new TextLoader.Column("OddsDraw", DataKind.R4, 1),
                        new TextLoader.Column("OddsGuests", DataKind.R4, 2),
                        new TextLoader.Column("Result", DataKind.R4, 3),
                    }
            }
            );

            var dataView = textLoader.Read(trainingDataPath);

            var pipeline = _mlContext.Transforms.CopyColumns(inputColumnName: "Result", outputColumnName: "Label")
                .Append(_mlContext.Transforms.Concatenate("Features", "OddsHosts", "OddsDraw", "OddsGuests"))
                .Append(_mlContext.Regression.Trainers.FastTree());

            var model = pipeline.Fit(dataView);

            var predictionFunction = model.CreatePredictionEngine<MatchBetResult, MatchBetResultPrediction>(_mlContext);

            var taxiTripSample = new MatchBetResult
            {
                OddsHosts = 2.12f,
                OddsDraw = 3.39f,
                OddsGuests = 3.74f
            };

            var prediction = predictionFunction.Predict(taxiTripSample);

            Console.WriteLine($"Predicted result: {prediction.Result}");
        }
    }
}
