using System;
using DataRepository.Models.Soccer.Odds;
using Main.Infrastructure.Enums;

namespace DataRepository.Services.Soccer.Odds
{
    public interface ISoccerOddsRepositoryService<TSoccerEntity> : IRepositoryServiceBase<TSoccerEntity> where TSoccerEntity : SoccerBetEntityBase
    {
        DateTime? GetLatestEventDate(Country country, string leagueName);
    }
}
