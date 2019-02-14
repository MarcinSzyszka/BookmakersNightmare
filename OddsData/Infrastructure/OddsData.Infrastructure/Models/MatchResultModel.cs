namespace OddsData.Infrastructure.Models
{
    public class MatchResultModel
    {
        public SingleBetResult FullTime { get; set; }

        public SingleBetResult FirstHalf { get; set; }

        public SingleBetResult SecondHalf { get; set; }
    }
}
