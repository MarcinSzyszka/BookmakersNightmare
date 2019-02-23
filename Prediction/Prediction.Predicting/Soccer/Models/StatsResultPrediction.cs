using Microsoft.ML.Data;

namespace Prediction.Predicting.Soccer.Models
{
    internal class StatsResultPrediction
    {
        [ColumnName("Score")]
        public float Result = -100;
    }
}
