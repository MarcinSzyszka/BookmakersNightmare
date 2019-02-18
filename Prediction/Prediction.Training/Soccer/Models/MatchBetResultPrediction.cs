using Microsoft.ML.Data;

namespace Prediction.Training.Soccer.Models
{
    public class MatchBetResultPrediction
    {
        [ColumnName("Score")]
        public float Result = -100;
    }
}
