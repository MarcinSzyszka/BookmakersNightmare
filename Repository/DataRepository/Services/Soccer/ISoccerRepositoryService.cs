using System;
using DataRepository.Models.Soccer;
using Main.Infrastructure.Enums;

namespace DataRepository.Services.Soccer
{
    public interface ISoccerRepositoryService<TSoccerEntity> : IRepositoryServiceBase<TSoccerEntity> where TSoccerEntity : SoccerBetEntityBase
    {
        DateTime? GetLatestEventDate(Country country, string leagueName);
    }
}
