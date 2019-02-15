using Main.Infrastructure.Enums;

namespace DataRepository.Models.Soccer
{
    public class SoccerFullTimeMatchBetEntity : SoccerBetEntityBase
    {
        public decimal OddsHosts { get; set; }

        public decimal OddsDraw { get; set; }

        public decimal OddsGuests { get; set; }

        public SingleBetResult Result { get; set; }
    }
}
