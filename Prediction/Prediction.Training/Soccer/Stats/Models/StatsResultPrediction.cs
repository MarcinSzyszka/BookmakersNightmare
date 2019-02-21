using Microsoft.ML.Data;

namespace Prediction.Training.Soccer.Stats.Models
{
    internal class StatsResultPrediction
    {
        [ColumnName("Score")]
        public float Result = -100;
    }
}
