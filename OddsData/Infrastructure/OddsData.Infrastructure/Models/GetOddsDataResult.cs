using System.Collections.Generic;

namespace OddsData.Infrastructure.Models
{
    public class GetOddsDataResult
    {
        public GetOddsDataResult(bool success, IEnumerable<MatchBet> data, string message)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public bool Success { get; }

        public IEnumerable<MatchBet> Data { get; }

        public string Message { get; }
    }
}
