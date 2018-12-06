using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FFBStats.Business
{
    public interface IYahooFFBClient
    {
        (ScoreTeamWeek LowScoreTeamWeek, ScoreTeamWeek HighScoreTeamWeek) GetHighLowScoreForYearAllTeams(int year,
            string token);
    }

    public class ScoreTeamWeek
    {
        public double Points { get; set; }
        public int Week { get; set; }
        public string TeamName { get; set; }
    }
}