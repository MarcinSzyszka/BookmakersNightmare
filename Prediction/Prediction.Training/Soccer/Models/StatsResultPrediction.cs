using Microsoft.ML.Data;

namespace Prediction.Training.Soccer.Models
{
    internal class StatsResultPrediction
    {
        [ColumnName("Score")]
        public float Result = -100;
    }
}
