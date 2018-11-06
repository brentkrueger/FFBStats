using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FFBStats.Business
{
    public class YahooFfbClient : IYahooFFBClient
    {
        private readonly IYahooWebRequestComposer _yahooWebRequestComposer;

        public YahooFfbClient(IYahooWebRequestComposer yahooWebRequestComposer)
        {
            _yahooWebRequestComposer = yahooWebRequestComposer;
        }

        public Task<string> GetLeagues(string token)
        {
            var url = "https://fantasysports.yahooapis.com/fantasy/v2/leagues";
            return _yahooWebRequestComposer.GetAsync(url, token);
        }

        public Task<string> GetGameIds(string token)
        {
            var startingYear = 2001;
            var endingYear = DateTime.Now.Year;
            List<int> years = new List<int>();
            for (int i = 0; i <= endingYear - startingYear; i++)
            {
                years.Add(startingYear+i);
            }

            var commaJoinedYears = string.Join(",", years);
            var url = $"https://fantasysports.yahooapis.com/fantasy/v2/games;game_codes=nfl;seasons={commaJoinedYears}";
            return _yahooWebRequestComposer.GetAsync(url, token);
        }
    }
}