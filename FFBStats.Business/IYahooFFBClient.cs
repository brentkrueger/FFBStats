using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FFBStats.Business
{
    public interface IYahooFFBClient
    {
        /// <summary>
        /// List of ScoreTeamWeek objects, representing the whole league high score for the year, and whole league low score for the year.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        (ScoreTeamWeek LowScoreTeamWeek, ScoreTeamWeek HighScoreTeamWeek) GetHighLowScoreForYearWholeLeague(int year,
            string token);
    }

    public class ScoreTeamWeek
    {
        public double Points { get; set; }
        public int Week { get; set; }
        public string TeamName { get; set; }
    }
}