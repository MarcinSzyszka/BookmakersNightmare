using Main.Infrastructure.Enums;

namespace Prediction.Training.Soccer.Models
{
    public class MatchBetResult
    {
        public decimal OddsHosts { get; set; }

        public decimal OddsDraw { get; set; }

        public decimal OddsGuests { get; set; }

        public SingleBetResult Result { get; set; }
    }
}
