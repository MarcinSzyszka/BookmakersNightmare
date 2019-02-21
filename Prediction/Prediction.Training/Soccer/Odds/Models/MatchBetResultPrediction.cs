using Microsoft.ML.Data;

namespace Prediction.Training.Soccer.Odds.Models
{
    public class MatchBetResultPrediction
    {
        [ColumnName("Score")]
        public float Result = -100;
    }
}
