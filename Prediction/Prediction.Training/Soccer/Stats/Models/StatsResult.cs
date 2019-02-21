using Microsoft.ML.Data;

namespace Prediction.Training.Soccer.Stats.Models
{
    internal class StatsResult
    {
        [Column("0")]
        public float Result { get; set; }

        [Column("1")]
        public float HostsBallPossession { get; set; }

        [Column("2")]
        public float HostsAttacksOnGoal { get; set; }

        [Column("3")]
        public float HostsShotsOnGoal { get; set; }

        [Column("4")]
        public float HostsShotsOutGoal { get; set; }

        [Column("5")]
        public float HostsCorners { get; set; }

        [Column("6")]
        public float HostsAccuratePasses { get; set; }

        [Column("7")]
        public float HostsBlocks { get; set; }

        [Column("8")]
        public float GuestsBallPossession { get; set; }

        [Column("9")]
        public float GuestsAttacksOnGoal { get; set; }

        [Column("10")]
        public float GuestsShotsOnGoal { get; set; }

        [Column("11")]
        public float GuestsShotsOutGoal { get; set; }

        [Column("12")]
        public float GuestsCorners { get; set; }

        [Column("13")]
        public float GuestsAccuratePasses { get; set; }

        [Column("14")]
        public float GuestsBlocks { get; set; }
    }
}
