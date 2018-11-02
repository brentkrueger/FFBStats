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
            var url = "https://fantasysports.yahooapis.com/fantasy/v2/game/nfl";
            return _yahooWebRequestComposer.GetAsync(url, token);
        }
    }
}