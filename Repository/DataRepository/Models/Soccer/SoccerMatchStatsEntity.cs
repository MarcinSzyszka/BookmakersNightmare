using Main.Infrastructure.Enums;

namespace DataRepository.Models.Soccer
{
    public class SoccerMatchStatsEntity : BaseEntity
    {
        public string HostsTeam { get; set; }

        public string GuestsTeam { get; set; }

        public string Country { get; set; }

        public string League { get; set; }

        public SingleBetResult Result { get; set; }

        public string StatsJson { get; set; }
    }
}
