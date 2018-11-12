using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FFBStats.Business
{
    public interface IYahooFFBClient
    {
        IEnumerable<string> GetLeagues(string token);
        IEnumerable<string> GetGameIds(string token);
        ScoreYear GetMaxScoreAllTime(string token);
    }

    public class ScoreYear
    {
        public decimal Points { get; set; }
        public int Year { get; set; }
    }
}