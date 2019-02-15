using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using DataRepository.Models.Soccer;
using DataRepository.Services.Soccer;
using Main.Infrastructure.Enums;

namespace Prediction.LearningData.Soccer.Services
{
    public class SoccerLearningDataService : ISoccerLearningDataService
    {
        private readonly ISoccerRepositoryService<SoccerFullTimeMatchBetEntity> _soccerFullTimeRepositoryService;
        private readonly ISoccerRepositoryService<SoccerFirstHalfMatchBetEntity> _soccerFirstHalfRepositoryService;
        private readonly ISoccerRepositoryService<SoccerSecondHalfMatchBetEntity> _soccerSecondHalfRepositoryService;

        public SoccerLearningDataService(ISoccerRepositoryService<SoccerFullTimeMatchBetEntity> soccerFullTimeRepositoryService, ISoccerRepositoryService<SoccerFirstHalfMatchBetEntity> soccerFirstHalfRepositoryService, ISoccerRepositoryService<SoccerSecondHalfMatchBetEntity> soccerSecondHalfRepositoryService)
        {
            _soccerFullTimeRepositoryService = soccerFullTimeRepositoryService;
            _soccerFirstHalfRepositoryService = soccerFirstHalfRepositoryService;
            _soccerSecondHalfRepositoryService = soccerSecondHalfRepositoryService;
        }

        public string PrepareFullTimeDataAndGetResultsFilePath()
        {
            var matchBetsEntities = _soccerFullTimeRepositoryService.GetAll().Cast<SoccerBetEntityBase>().ToList();

            return CreateCsf(matchBetsEntities);
        }

        public string PrepareFullTimeDataAndGetResultsFilePath(Country country, string league)
        {
            var matchBetsEntities = _soccerFullTimeRepositoryService.GetAll().Where(m => m.Country == country && m.LeagueName.Equals(league, StringComparison.CurrentCultureIgnoreCase)).Cast<SoccerBetEntityBase>().ToList();

            return CreateCsf(matchBetsEntities);
        }

        public string PrepareFirstHalfDataAndGetResultsFilePath()
        {
            var matchBetsEntities = _soccerFirstHalfRepositoryService.GetAll().Cast<SoccerBetEntityBase>().ToList();

            return CreateCsf(matchBetsEntities);
        }

        public string PrepareFirstHalfDataAndGetResultsFilePath(Country country, string league)
        {
            var matchBetsEntities = _soccerFirstHalfRepositoryService.GetAll().Where(m => m.Country == country && m.LeagueName.Equals(league, StringComparison.CurrentCultureIgnoreCase)).Cast<SoccerBetEntityBase>().ToList();

            return CreateCsf(matchBetsEntities);
        }

        public string PrepareSecondHalfDataAndGetResultsFilePath()
        {
            var matchBetsEntities = _soccerSecondHalfRepositoryService.GetAll().Cast<SoccerBetEntityBase>().ToList();

            return CreateCsf(matchBetsEntities);
        }

        public string PrepareSecondHalfDataAndGetResultsFilePath(Country country, string league)
        {
            var matchBetsEntities = _soccerSecondHalfRepositoryService.GetAll().Where(m => m.Country == country && m.LeagueName.Equals(league, StringComparison.CurrentCultureIgnoreCase)).Cast<SoccerBetEntityBase>().ToList();

            return CreateCsf(matchBetsEntities);
        }

        private string CreateCsf(List<SoccerBetEntityBase> matchBetsEntities)
        {
            var fileName = $"{Path.GetTempFileName()}.csv";

            using (var writer = new StreamWriter(fileName, false))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(matchBetsEntities.Select(e => new
                {
                    e.OddsHosts,
                    e.OddsDraw,
                    e.OddsGuests,
                    e.Result
                }));
            }

            return fileName;
        }
    }
}
