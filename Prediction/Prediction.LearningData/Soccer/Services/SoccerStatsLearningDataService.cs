using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using DataRepository.Models.Soccer;
using DataRepository.Services.Soccer;
using Main.Infrastructure.Enums;
using Main.Infrastructure.Models.Soccer;
using Newtonsoft.Json;
using Prediction.LearningData.Soccer.Models;

namespace Prediction.LearningData.Soccer.Services
{
    public class SoccerStatsLearningDataService : ISoccerStatsLearningDataService
    {
        private readonly ISoccerStatsRepositoryService _soccerStatsRepository;

        public SoccerStatsLearningDataService(ISoccerStatsRepositoryService soccerStatsRepository)
        {
            _soccerStatsRepository = soccerStatsRepository;
        }

        public string PrepareLearningDataAndGetResultsFilePath(int matchesBeforeCount)
        {
            var data = _soccerStatsRepository.GetAll().ToList();

            return PrepareData(matchesBeforeCount, data);
        }

        public string PrepareLearningDataAndGetResultsFilePath(string teamName, int matchesBeforeCount)
        {
            var data = _soccerStatsRepository.GetAll().Where(s =>
                s.HostsTeam.Equals(teamName, StringComparison.CurrentCultureIgnoreCase) ||
                s.GuestsTeam.Equals(teamName, StringComparison.CurrentCultureIgnoreCase)).ToList();

            return PrepareData(matchesBeforeCount, data, teamName);
        }

        public string PrepareLearningDataAndGetResultsFilePath(string country, string league, int matchesBeforeCount)
        {
            var data = _soccerStatsRepository.GetAll().Where(s =>
                s.Country.Equals(country, StringComparison.CurrentCultureIgnoreCase) &&
                s.League.Equals(league, StringComparison.CurrentCultureIgnoreCase)).ToList();

            return PrepareData(matchesBeforeCount, data);
        }

        private string PrepareData(int matchesBeforeCount, List<SoccerMatchStatsEntity> data, string teamName = null)
        {
            var groupedByHosts = data.Where(t => string.IsNullOrEmpty(teamName) || t.HostsTeam == teamName).GroupBy(t => t.HostsTeam).Select(r => new { Team = r.Key, Stats = r.ToList() });
            var groupedByGuests = data.Where(t => string.IsNullOrEmpty(teamName) || t.GuestsTeam == teamName).GroupBy(t => t.GuestsTeam).Select(r => new { Team = r.Key, Stats = r.ToList() });

            var groupedByTeamName = groupedByHosts.Join(groupedByGuests, h => h.Team, g => g.Team, (h, g) => new TeamMatchesStatsModel { TeamName = h.Team, Stats = h.Stats.Concat(g.Stats).OrderBy(s => s.EventDate).ToList() }).ToList();

            var results = new List<SoccerStatsModel>();

            foreach (var team in groupedByTeamName)
            {
                results.AddRange(GetResultsForTeam(team, matchesBeforeCount));
            }

            return CreateCsv(results);
        }

        private IEnumerable<SoccerStatsModel> GetResultsForTeam(TeamMatchesStatsModel team, int matchesBeforeCount)
        {
            team.Stats = team.Stats.OrderBy(s => s.EventDate).ToList();

            var nextMatchesResults = new List<SoccerStatsModel>();

            var startIndex = matchesBeforeCount - 1;

            for (var i = startIndex; i < team.Stats.Count - 1; i++)
            {
                var lastMatchesStats = team.Stats.Skip(i - startIndex).Take(matchesBeforeCount).ToList();

                var statsLearningModels = ConvertToLearningData(team.TeamName, lastMatchesStats).ToList();

                nextMatchesResults.Add(item: new SoccerStatsModel
                {
                    Result = (int)GetResultValue(team.TeamName, team.Stats[i + 1]),
                    BallPossession = statsLearningModels.Sum(s => s.BallPossession),
                    AttacksOnGoal = statsLearningModels.Sum(s => s.AttacksOnGoal),
                    ShotsOnGoal = statsLearningModels.Sum(s => s.ShotsOnGoal),
                    ShotsOutGoal = statsLearningModels.Sum(s => s.ShotsOutGoal),
                    Corners = statsLearningModels.Sum(s => s.Corners),
                    AccuratePasses = statsLearningModels.Sum(s => s.AccuratePasses),
                    Blocks = statsLearningModels.Sum(s => s.Blocks),
                    ResultPoints = statsLearningModels.Sum(s => s.ResultPoints),
                });
            }

            return nextMatchesResults;
        }

        private StatsTeamMatchResult GetResultValue(string teamTeamName, SoccerMatchStatsEntity teamStat)
        {
            if (teamTeamName == teamStat.HostsTeam)
            {
                return (StatsTeamMatchResult)(teamStat.Result == SingleBetResult.Hosts ? 2 : teamStat.Result == SingleBetResult.Guests ? 0 : 1);
            }

            return (StatsTeamMatchResult)(teamStat.Result == SingleBetResult.Guests ? 2 : teamStat.Result == SingleBetResult.Hosts ? 0 : 1);

        }

        private IEnumerable<SoccerStatsModel> ConvertToLearningData(string teamName, List<SoccerMatchStatsEntity> data)
        {
            var results = new List<SoccerStatsModel>(data.Count);

            foreach (var soccerMatchStatsEntity in data)
            {
                var stats = JsonConvert.DeserializeObject<List<SoccerMatchStatsRow>>(soccerMatchStatsEntity.StatsJson);

                var teamStats = Enumerable.Empty<SoccerTeamStatsRowModel>();

                if (soccerMatchStatsEntity.HostsTeam == teamName)
                {
                    teamStats = stats.Select(s => new SoccerTeamStatsRowModel { StatsName = s.StatsName, StatsValue = s.HostsStatsValue }).ToList();
                }
                else
                {
                    teamStats = stats.Select(s => new SoccerTeamStatsRowModel { StatsName = s.StatsName, StatsValue = s.GuestsStatsValue }).ToList();
                }

                results.Add(new SoccerStatsModel(soccerMatchStatsEntity.Result, soccerMatchStatsEntity.HostsTeam == teamName, teamStats));
            }

            return results;
        }

        private string CreateCsv(List<SoccerStatsModel> learningModels)
        {
            var fileName = $"{Path.GetTempFileName()}.csv";

            using (var writer = new StreamWriter(fileName, false))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.Delimiter = ",";
                csv.Configuration.CultureInfo = CultureInfo.GetCultureInfo("en-US");

                csv.WriteRecords(learningModels);
            }

            return fileName;
        }
    }
}
