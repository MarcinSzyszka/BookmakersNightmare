using Prediction.Training.Soccer.Models;

namespace Prediction.Training.Soccer
{
    public interface ISoccerStatsTrainingService
    {
        TrainingResult Train(string trainingDataPath);
    }
}