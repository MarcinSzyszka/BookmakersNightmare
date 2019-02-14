namespace OddsData.Infrastructure.Models
{
    public class MatchBet
    {
        public string HostsTeamName { get; set; }

        public string GuestsTeamName { get; set; }

        public SingleBet FullTime { get; set; } = new SingleBet();

        public SingleBet FirstHalf { get; set; } = new SingleBet();

        public SingleBet SecondHalf { get; set; } = new SingleBet();
    }
}
