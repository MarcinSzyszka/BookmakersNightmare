using System.Collections.Generic;
using OddsData.Infrastructure.Enums;

namespace OddsData.Infrastructure.Models
{
    public class GetOddsDataResult
    {
        public GetOddsDataResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public GetOddsDataResult(bool success, IEnumerable<MatchBet> data)
        {
            Success = success;
            Data = data;
        }

        public GetOddsDataResult(bool success, Country country, string leagueName, IEnumerable<MatchBet> data)
        {
            Success = success;
            Country = country;
            LeagueName = leagueName;
            Data = data;
        }

        public bool Success { get; }

        public Country Country { get; }

        public string LeagueName { get; }

        public IEnumerable<MatchBet> Data { get; }

        public string Message { get; }
    }
}
