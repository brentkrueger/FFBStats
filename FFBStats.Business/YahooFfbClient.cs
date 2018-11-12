using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YahooFantasyWrapper.Client;

namespace FFBStats.Business
{
    public class YahooFFBClient : IYahooFFBClient
    {
        private readonly IYahooWebRequestComposer _yahooWebRequestComposer;
        private readonly IYahooFantasyClient _yahooFantasyClient;

        public YahooFFBClient(IYahooWebRequestComposer yahooWebRequestComposer, IYahooFantasyClient yahooFantasyClient)
        {
            _yahooWebRequestComposer = yahooWebRequestComposer;
            _yahooFantasyClient = yahooFantasyClient;
        }

        public IEnumerable<string> GetLeagues(string token)
        {
            var url = "https://fantasysports.yahooapis.com/fantasy/v2/leagues";
            var result = _yahooWebRequestComposer.GetAsync(url, token);
            return new List<string>();
        }

        public IEnumerable<string> GetGameIds(string token)
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
            var xmlResult = _yahooWebRequestComposer.GetAsync(url, token).Result;

            var test = _yahooFantasyClient.UserResourceManager.GetUser(token);

            return new List<string>();
        }

        private static T DeserializeIntoObject<T>(string xmlResult)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader rdr = new StringReader(xmlResult);
            T fantasyContent = (T) serializer.Deserialize(rdr);
            return fantasyContent;
        }

        public ScoreYear GetMaxScoreAllTime(string token)
        {
            var gameIDs = GetGameIds(token);

            foreach (var gameID in gameIDs)
            {
                
            }

            return new ScoreYear() { Year = 2016, Points = (decimal) 242.2};
        }
    }
}