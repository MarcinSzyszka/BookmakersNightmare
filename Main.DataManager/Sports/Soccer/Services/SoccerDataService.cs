using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataRepository.Models.Soccer;
using DataRepository.Services.Soccer;
using Main.Infrastructure.Enums;
using OddsData.Source;

namespace Main.DataManager.Sports.Soccer.Services
{
    public class SoccerDataService : ISoccerDataService
    {
        private readonly ISoccerRepositoryService<SoccerFullTimeMatchBetEntity> _soccerFullTimeRepositoryService;
        private readonly ISoccerRepositoryService<SoccerFirstHalfMatchBetEntity> _soccerFirstHalfRepositoryService;
        private readonly ISoccerRepositoryService<SoccerSecondHalfMatchBetEntity> _soccerSecondHalfRepositoryService;

        private readonly IOddsDataSourceService _oddsDataSourceService;

        public SoccerDataService(ISoccerRepositoryService<SoccerFullTimeMatchBetEntity> soccerFullTimeRepositoryService, ISoccerRepositoryService<SoccerFirstHalfMatchBetEntity> soccerFirstHalfRepositoryService, ISoccerRepositoryService<SoccerSecondHalfMatchBetEntity> soccerSecondHalfRepositoryService, IOddsDataSourceService oddsDataSourceService)
        {
            _soccerFullTimeRepositoryService = soccerFullTimeRepositoryService;
            _soccerFirstHalfRepositoryService = soccerFirstHalfRepositoryService;
            _soccerSecondHalfRepositoryService = soccerSecondHalfRepositoryService;
            _oddsDataSourceService = oddsDataSourceService;
        }

        public async Task UpdateResultsData(Country country, string leagueName)
        {
            var latestDownloadedEventDate = _soccerFullTimeRepositoryService.GetLatestEventDate(country, leagueName);

            var results = await _oddsDataSourceService.GetData(country, leagueName, latestDownloadedEventDate);

            InsertFullTimeResults(country, leagueName, results);
            InsertFirstHalfResults(country, leagueName, results);
            InsertSecondHalfResults(country, leagueName, results);
        }

        private void InsertFullTimeResults(Country country, string leagueName, OddsData.Infrastructure.Models.GetOddsDataResult results)
        {
            var fullTimeResults = results.Data.Where(d => d.FullTime.Result > SingleBetResult.Unknown).Select(d => new { d.HostsTeamName, d.GuestsTeamName, d.MatchDate, d.FullTime }).ToList();

            var fullTimeEntities = new List<SoccerFullTimeMatchBetEntity>();

            foreach (var fullTimeResult in fullTimeResults)
            {
                fullTimeEntities.AddRange(fullTimeResult.FullTime.Odds.Select(odds => new SoccerFullTimeMatchBetEntity
                {
                    Country = country,
                    LeagueName = leagueName,
                    HostsTeam = fullTimeResult.HostsTeamName,
                    GuestsTeam = fullTimeResult.GuestsTeamName,
                    EventDate = fullTimeResult.MatchDate,
                    Result = fullTimeResult.FullTime.Result,
                    OddsDraw = odds.Draw,
                    OddsHosts = odds.Hosts,
                    OddsGuests = odds.Guests
                }));
            }

            _soccerFullTimeRepositoryService.Insert(fullTimeEntities);
        }

        private void InsertFirstHalfResults(Country country, string leagueName, OddsData.Infrastructure.Models.GetOddsDataResult results)
        {
            var fullTimeResults = results.Data.Where(d => d.FirstHalf.Result > SingleBetResult.Unknown).Select(d => new { d.HostsTeamName, d.GuestsTeamName, d.MatchDate, d.FullTime }).ToList();

            var fullTimeEntities = new List<SoccerFirstHalfMatchBetEntity>();

            foreach (var fullTimeResult in fullTimeResults)
            {
                fullTimeEntities.AddRange(fullTimeResult.FullTime.Odds.Select(odds => new SoccerFirstHalfMatchBetEntity
                {
                    Country = country,
                    LeagueName = leagueName,
                    HostsTeam = fullTimeResult.HostsTeamName,
                    GuestsTeam = fullTimeResult.GuestsTeamName,
                    EventDate = fullTimeResult.MatchDate,
                    Result = fullTimeResult.FullTime.Result,
                    OddsDraw = odds.Draw,
                    OddsHosts = odds.Hosts,
                    OddsGuests = odds.Guests
                }));
            }

            _soccerFirstHalfRepositoryService.Insert(fullTimeEntities);
        }

        private void InsertSecondHalfResults(Country country, string leagueName, OddsData.Infrastructure.Models.GetOddsDataResult results)
        {
            var secondHalfResults = results.Data.Where(d => d.SecondHalf.Result > SingleBetResult.Unknown).Select(d => new { d.HostsTeamName, d.GuestsTeamName, d.MatchDate, d.FullTime }).ToList();

            var fullTimeEntities = new List<SoccerSecondHalfMatchBetEntity>();

            foreach (var fullTimeResult in secondHalfResults)
            {
                fullTimeEntities.AddRange(fullTimeResult.FullTime.Odds.Select(odds => new SoccerSecondHalfMatchBetEntity
                {
                    Country = country,
                    LeagueName = leagueName,
                    HostsTeam = fullTimeResult.HostsTeamName,
                    GuestsTeam = fullTimeResult.GuestsTeamName,
                    EventDate = fullTimeResult.MatchDate,
                    Result = fullTimeResult.FullTime.Result,
                    OddsDraw = odds.Draw,
                    OddsHosts = odds.Hosts,
                    OddsGuests = odds.Guests
                }));
            }

            _soccerSecondHalfRepositoryService.Insert(fullTimeEntities);
        }
    }
}
