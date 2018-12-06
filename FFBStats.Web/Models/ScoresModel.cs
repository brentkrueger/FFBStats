﻿using System.Collections.Generic;

namespace FFBStats.Web
{
    public class HighScoreLowScoreYear
    {
        public string HighScoreTeamName;
        public string HighScoreManagerName;
        public int HighScoreWeek;
        public double HighScorePoints;

        public string LowScoreTeamName;
        public string LowScoreManagerName;
        public int LowScoreWeek;
        public double LowScorePoints;

        public int Year;
    }

    public class ScoresModel
    {
        public IEnumerable<HighScoreLowScoreYear> HighScoreLowScoreYears;
    }
}