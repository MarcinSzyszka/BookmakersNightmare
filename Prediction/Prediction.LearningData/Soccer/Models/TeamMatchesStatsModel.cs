using System.Collections.Generic;
using DataRepository.Models.Soccer;

namespace Prediction.LearningData.Soccer.Models
{
    internal class TeamMatchesStatsModel
    {
        public string TeamName { get; set; }

        public List<SoccerMatchStatsEntity> Stats { get; set; }
    }
}
