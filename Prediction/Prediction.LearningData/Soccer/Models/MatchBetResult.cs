namespace Prediction.LearningData.Soccer.Models
{
    public class MatchBetResult
    {
        public decimal OddsHosts { get; set; }

        public decimal OddsDraw { get; set; }

        public decimal OddsGuests { get; set; }

        public int Result { get; set; }
    }
}
