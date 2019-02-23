using Microsoft.ML.Data;

namespace Prediction.Training.Soccer.Models
{
    public class StatsResult
    {
        [Column("0")]
        public float Result { get; set; }

        [Column("1")]
        public float BallPossession { get; set; }

        [Column("2")]
        public float AttacksOnGoal { get; set; }

        [Column("3")]
        public float ShotsOnGoal { get; set; }

        [Column("4")]
        public float ShotsOutGoal { get; set; }

        [Column("5")]
        public float Corners { get; set; }

        [Column("6")]
        public float Passes { get; set; }

        [Column("7")]
        public float AccuratePasses { get; set; }

        [Column("8")]
        public float Blocks { get; set; }

        [Column("9")]
        public float Points { get; set; }
    }
}
