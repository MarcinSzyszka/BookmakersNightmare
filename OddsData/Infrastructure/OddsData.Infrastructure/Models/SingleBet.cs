using System.Collections.Generic;

namespace OddsData.Infrastructure.Models
{
    public class SingleBet
    {
        public SingleBetResult Result { get; set; }

        public IEnumerable<SingleBetOdds> Odds { get; set; }
    }
}
