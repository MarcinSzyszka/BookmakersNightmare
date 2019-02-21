using Microsoft.ML.Data;

namespace Prediction.Training.Soccer.Odds.Models
{
    public class MatchBetResult
    {
        [Column("0")]
        public float OddsHosts;

        [Column("1")]
        public float OddsDraw;

        [Column("2")]
        public float OddsGuests;

        [Column("3")]
        public int Result;
    }
}
