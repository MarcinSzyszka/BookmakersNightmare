using System.Collections.Generic;
using Main.Infrastructure.Enums;

namespace OddsData.Infrastructure.Models
{
    public class SingleBet
    {
        public SingleBetResult Result { get; set; }

        public IEnumerable<SingleBetOdds> Odds { get; set; }
    }
}
