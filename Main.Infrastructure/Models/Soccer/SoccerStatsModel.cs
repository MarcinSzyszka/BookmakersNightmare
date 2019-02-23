using System.Collections.Generic;
using System.Linq;
using Main.Infrastructure.Enums;

namespace Main.Infrastructure.Models.Soccer
{
    public class SoccerStatsModel
    {
        public int Result { get; set; }

        public int BallPossession { get; set; }

        public int AttacksOnGoal { get; set; }

        public int ShotsOnGoal { get; set; }

        public int ShotsOutGoal { get; set; }

        public int Corners { get; set; }

        public int Passes { get; set; }

        public int AccuratePasses { get; set; }

        public int Blocks { get; set; }

        public int ResultPoints { get; set; }

        public SoccerStatsModel()
        {

        }

        public SoccerStatsModel(SingleBetResult result, bool hosts, IEnumerable<SoccerTeamStatsRowModel> stats)
        {
            var ballPossession = stats.FirstOrDefault(s => s.StatsName == "Posiadanie piłki");
            var attacksOnGoal = stats.FirstOrDefault(s => s.StatsName == "Sytuacje bramkowe");
            var shotsOnGoal = stats.FirstOrDefault(s => s.StatsName == "Strzały na bramkę");
            var shotsOutGoal = stats.FirstOrDefault(s => s.StatsName == "Strzały niecelne");
            var corners = stats.FirstOrDefault(s => s.StatsName == "Rzuty rożne");
            var passes = stats.FirstOrDefault(s => s.StatsName == "Podania");
            var accuratePasses = stats.FirstOrDefault(s => s.StatsName == "Podania celne");
            var blocks = stats.FirstOrDefault(s => s.StatsName == "Bloki");

            BallPossession = ballPossession?.StatsValue ?? 0;
            AttacksOnGoal = attacksOnGoal?.StatsValue ?? 0;
            ShotsOnGoal = shotsOnGoal?.StatsValue ?? 0;
            ShotsOutGoal = shotsOutGoal?.StatsValue ?? 0;
            Corners = corners?.StatsValue ?? 0;
            Passes = passes?.StatsValue ?? 0;
            AccuratePasses = accuratePasses?.StatsValue ?? 0;
            Blocks = blocks?.StatsValue ?? 0;
            if (result == SingleBetResult.Draw)
            {
                ResultPoints = 1;
            }
            else
            {
                ResultPoints = hosts ? result == SingleBetResult.Hosts ? 3 : 0
                    :
                    result == SingleBetResult.Guests ? 3 : 0;
            }
        }
    }
}
