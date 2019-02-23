using Microsoft.ML;
using Microsoft.ML.Core.Data;

namespace Prediction.Training.Soccer.Models
{
    public class TrainingResult
    {
        public MLContext MlContext { get; set; }

        public ITransformer Model { get; set; }
    }
}
