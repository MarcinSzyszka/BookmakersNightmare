using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using DataRepository.Models.Soccer.Stats;
using DataRepository.Services.Soccer.Stats;
using Main.Infrastructure.Models;
using Newtonsoft.Json;
using Prediction.LearningData.Soccer.Models;

namespace Prediction.LearningData.Soccer.Services.Stats
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

            return PrepareData(matchesBeforeCount, data);
        }

        public string PrepareLearningDataAndGetResultsFilePath(string country, string league, int matchesBeforeCount)
        {
            var data = _soccerStatsRepository.GetAll().Where(s =>
                s.Country.Equals(country, StringComparison.CurrentCultureIgnoreCase) &&
                s.League.Equals(league, StringComparison.CurrentCultureIgnoreCase)).ToList();

            return PrepareData(matchesBeforeCount, data);
        }

        private string PrepareData(int matchesBeforeCount, List<SoccerMatchStatsEntity> data)
        {
            var groupedByHosts = data.GroupBy(t => t.HostsTeam).Select(r => new { Team = r.Key, Stats = r.ToList() });
            var groupedByGuests = data.GroupBy(t => t.GuestsTeam).Select(r => new { Team = r.Key, Stats = r.ToList() });

            var groupedByTeamName = groupedByHosts.Join(groupedByGuests, h => h.Team, g => g.Team, (h, g) => new TeamMatchesStatsModel { TeamName = h.Team, Stats = h.Stats.Concat(g.Stats).OrderBy(s => s.EventDate).ToList() }).ToList();

            var results = new List<StatsLearningModel>();

            foreach (var team in groupedByTeamName)
            {
                results.AddRange(GetResultsForTeam(team, matchesBeforeCount));
            }

            return CreateCsv(results);
        }

        private IEnumerable<StatsLearningModel> GetResultsForTeam(TeamMatchesStatsModel team, int matchesBeforeCount)
        {
            team.Stats = team.Stats.OrderBy(s => s.EventDate).ToList();

            var nextMatchesResults = new List<StatsLearningModel>();

            var startIndex = matchesBeforeCount - 1;

            for (var i = startIndex; i < team.Stats.Count - 1; i++)
            {
                var lastMatchesStats = team.Stats.Skip(i - startIndex).Take(matchesBeforeCount).ToList();

                var statsLearningModels = ConvertToLearningData(lastMatchesStats).ToList();

                nextMatchesResults.Add(new StatsLearningModel
                {
                    Result = (int)team.Stats[i + 1].Result,
                    HostsBallPossession = statsLearningModels.Sum(s => s.HostsBallPossession),
                    GuestsBallPossession = statsLearningModels.Sum(s => s.GuestsBallPossession),
                    HostsAttacksOnGoal = statsLearningModels.Sum(s => s.HostsAttacksOnGoal),
                    GuestsAttacksOnGoal = statsLearningModels.Sum(s => s.GuestsAttacksOnGoal),
                    HostsShotsOnGoal = statsLearningModels.Sum(s => s.HostsShotsOnGoal),
                    GuestsShotsOnGoal = statsLearningModels.Sum(s => s.GuestsShotsOnGoal),
                    HostsShotsOutGoal = statsLearningModels.Sum(s => s.HostsShotsOutGoal),
                    GuestsShotsOutGoal = statsLearningModels.Sum(s => s.GuestsShotsOutGoal),
                    HostsCorners = statsLearningModels.Sum(s => s.HostsCorners),
                    GuestsCorners = statsLearningModels.Sum(s => s.GuestsCorners),
                    HostsAccuratePasses = statsLearningModels.Sum(s => s.HostsAccuratePasses),
                    GuestsAccuratePasses = statsLearningModels.Sum(s => s.GuestsAccuratePasses),
                    HostsBlocks = statsLearningModels.Sum(s => s.HostsBlocks),
                    GuestsBlocks = statsLearningModels.Sum(s => s.GuestsBlocks),
                    //HostsResultPoints = statsLearningModels.Sum(s => s.HostsResultPoints),
                    //GuestsResultPoints = statsLearningModels.Sum(s => s.GuestsResultPoints)
                });
            }

            return nextMatchesResults;
        }

        private IEnumerable<StatsLearningModel> ConvertToLearningData(List<SoccerMatchStatsEntity> data)
        {
            var results = new List<StatsLearningModel>(data.Count);

            foreach (var soccerMatchStatsEntity in data)
            {
                var learningDataModel = new StatsLearningModel
                {
                    Result = (int)soccerMatchStatsEntity.Result
                };

                var stats = JsonConvert.DeserializeObject<List<SoccerMatchStatsRow>>(soccerMatchStatsEntity.StatsJson);

                var ballPossession = stats.FirstOrDefault(s => s.StatsName == "Posiadanie piłki");
                var attacksOnGoal = stats.FirstOrDefault(s => s.StatsName == "Sytuacje bramkowe");
                var shotsOnGoal = stats.FirstOrDefault(s => s.StatsName == "Strzały na bramkę");
                var shotsOutGoal = stats.FirstOrDefault(s => s.StatsName == "Strzały niecelne");
                var corners = stats.FirstOrDefault(s => s.StatsName == "Rzuty rożne");
                var accuratePasses = stats.FirstOrDefault(s => s.StatsName == "Podania celne");
                var blocks = stats.FirstOrDefault(s => s.StatsName == "Bloki");

                learningDataModel.HostsBallPossession = ballPossession?.HostsStatsValue ?? 0;
                learningDataModel.GuestsBallPossession = ballPossession?.GuestsStatsValue ?? 0;

                learningDataModel.HostsAttacksOnGoal = attacksOnGoal?.HostsStatsValue ?? 0;
                learningDataModel.GuestsAttacksOnGoal = attacksOnGoal?.GuestsStatsValue ?? 0;

                learningDataModel.HostsShotsOnGoal = shotsOnGoal?.HostsStatsValue ?? 0;
                learningDataModel.GuestsShotsOnGoal = shotsOnGoal?.GuestsStatsValue ?? 0;

                learningDataModel.HostsShotsOutGoal = shotsOutGoal?.HostsStatsValue ?? 0;
                learningDataModel.GuestsShotsOutGoal = shotsOutGoal?.GuestsStatsValue ?? 0;

                learningDataModel.HostsCorners = corners?.HostsStatsValue ?? 0;
                learningDataModel.GuestsCorners = corners?.GuestsStatsValue ?? 0;

                learningDataModel.HostsAccuratePasses = accuratePasses?.HostsStatsValue ?? 0;
                learningDataModel.GuestsAccuratePasses = accuratePasses?.GuestsStatsValue ?? 0;

                learningDataModel.HostsBlocks = blocks?.HostsStatsValue ?? 0;
                learningDataModel.GuestsBlocks = blocks?.GuestsStatsValue ?? 0;

                //learningDataModel.HostsResultPoints = (SingleBetResult)learningDataModel.Result == SingleBetResult.Hosts ? 3 : (SingleBetResult)learningDataModel.Result == SingleBetResult.Draw ? 1 : 0;;
                //learningDataModel.GuestsResultPoints = (SingleBetResult)learningDataModel.Result == SingleBetResult.Guests ? 3 : (SingleBetResult)learningDataModel.Result == SingleBetResult.Draw ? 1 : 0; ;

                results.Add(learningDataModel);
            }

            return results;
        }

        private string CreateCsv(List<StatsLearningModel> learningModels)
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
