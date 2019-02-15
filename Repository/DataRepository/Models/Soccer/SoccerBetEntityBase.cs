using Main.Infrastructure.Enums;

namespace DataRepository.Models.Soccer
{
    public abstract class SoccerBetEntityBase : BaseEntity
    {
        public Country Country { get; set; }

        public string LeagueName { get; set; }

        public string HostsTeam { get; set; }

        public string GuestsTeam { get; set; }

        public decimal OddsHosts { get; set; }

        public decimal OddsDraw { get; set; }

        public decimal OddsGuests { get; set; }

        public SingleBetResult Result { get; set; }
    }
}
