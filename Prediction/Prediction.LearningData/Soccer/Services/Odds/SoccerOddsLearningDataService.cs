using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using DataRepository.Models.Soccer.Odds;
using DataRepository.Services.Soccer.Odds;
using Main.Infrastructure.Enums;
using Prediction.LearningData.Soccer.Models;

namespace Prediction.LearningData.Soccer.Services.Odds
{
    public class SoccerOddsLearningDataService : ISoccerOddsLearningDataService
    {
        private readonly ISoccerOddsRepositoryService<SoccerFullTimeMatchBetEntity> _soccerFullTimeRepositoryService;
        private readonly ISoccerOddsRepositoryService<SoccerFirstHalfMatchBetEntity> _soccerFirstHalfRepositoryService;
        private readonly ISoccerOddsRepositoryService<SoccerSecondHalfMatchBetEntity> _soccerSecondHalfRepositoryService;

        public SoccerOddsLearningDataService(ISoccerOddsRepositoryService<SoccerFullTimeMatchBetEntity> soccerFullTimeRepositoryService, ISoccerOddsRepositoryService<SoccerFirstHalfMatchBetEntity> soccerFirstHalfRepositoryService, ISoccerOddsRepositoryService<SoccerSecondHalfMatchBetEntity> soccerSecondHalfRepositoryService)
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
                csv.Configuration.Delimiter = ",";
                csv.Configuration.CultureInfo = CultureInfo.GetCultureInfo("en-US");

                csv.WriteRecords(matchBetsEntities.Select(e => new MatchBetResult
                {
                    OddsHosts = e.OddsHosts,
                    OddsDraw = e.OddsDraw,
                    OddsGuests = e.OddsGuests,
                    Result = (int)e.Result
                }));
            }

            return fileName;
        }
    }
}
