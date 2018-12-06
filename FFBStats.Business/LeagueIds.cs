using System.Collections.Generic;

namespace FFBStats.Business
{
    public class LeagueIds
    {
        public static Dictionary<int, string> GetLeagueIds()
        {
            var results = new Dictionary<int, string>
            {
                //{2005, "197638"},
                //{2006, "190794"},
                //{2007, "31995"},
                //{2008, "86942"},
                //{2009, "92682"},
                //{2010, "17989"},
                //{2011, "62813"},
                //{2012, "69135"},
                //{2013, "74087"},
                //{2014, "132839"},
                //{2015, "76486"},
                {2016, "125852"},
                {2017, "9339"},
                {2018, "214893"}
            };
            return results;
        }
    }
}