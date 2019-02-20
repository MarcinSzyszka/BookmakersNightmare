using System;
using System.Collections.Generic;
using Main.Infrastructure.Enums;
using StatsDataSource.Models;

namespace StatsDataSource.Soccer.Models
{
    public class SoccerMatchStatsData
    {
        public DateTime MatchDate { get; set; }

        public string HostsTeam { get; set; }

        public string GuestTeam { get; set; }

        public SingleBetResult MatchResult { get; set; }

        public List<StatsRow> StatsRows { get; set; } = new List<StatsRow>();
    }
}
