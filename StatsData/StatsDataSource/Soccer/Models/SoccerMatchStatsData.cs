using System;
using System.Collections.Generic;
using Main.Infrastructure.Enums;
using Main.Infrastructure.Models;

namespace StatsDataSource.Soccer.Models
{
    public class SoccerMatchStatsData
    {
        public DateTime MatchDate { get; set; }

        public string HostsTeam { get; set; }

        public string GuestTeam { get; set; }

        public SingleBetResult MatchResult { get; set; }

        public List<SoccerMatchStatsRow> StatsRows { get; set; } = new List<SoccerMatchStatsRow>();
    }
}
