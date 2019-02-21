namespace Prediction.LearningData.Soccer.Models
{
    internal class StatsLearningModel
    {
        public int Result { get; set; }
          
        public int BallPossession { get; set; }
              
        public int AttacksOnGoal { get; set; }
            
        public int ShotsOnGoal { get; set; }
            
        public int ShotsOutGoal { get; set; }
            
        public int Corners { get; set; }

        public int Passes { get; set; }

        public int AccuratePasses { get; set; }
            
        public int Blocks { get; set; }
            
        public int ResultPoints { get; set; }
    }
}
