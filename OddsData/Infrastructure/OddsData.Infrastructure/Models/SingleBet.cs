using System.Collections.Generic;
using Main.Infrastructure.Enums;

namespace OddsData.Infrastructure.Models
{
    public class SingleBet
    {
        public SingleBetResult Result { get; set; } = SingleBetResult.Unknown;

        public IEnumerable<SingleBetOdds> Odds { get; set; } = new List<SingleBetOdds>();
    }
}
