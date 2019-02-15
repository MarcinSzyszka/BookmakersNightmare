﻿using System;
using DataRepository.Models.Soccer;
using Main.Infrastructure.Enums;
using SQLite;

namespace DataRepository.Services.Soccer
{
    public class SoccerRepositoryService<TSoccerEntity> : RepositoryServiceBase<TSoccerEntity>, ISoccerRepositoryService<TSoccerEntity> where TSoccerEntity : SoccerBetEntityBase, new()
    {
        public DateTime? GetLatestEventDate(Country country, string leagueName)
        {
            using (var db = new SQLiteConnection(DbPath))
            {
                return db.Table<SoccerFullTimeMatchBetEntity>().Where(e => e.Country == country && e.LeagueName == leagueName).OrderByDescending(e => e.EventDate).FirstOrDefault()?.EventDate;
            }
        }
    }
}
