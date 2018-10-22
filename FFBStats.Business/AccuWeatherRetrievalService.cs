using Microsoft.Extensions.Configuration;

namespace FFBStats.Business
{
    public class AccuWeatherRetrievalService : IWeatherRetrievalService
    {
        private readonly IConfiguration _config;

        public AccuWeatherRetrievalService(IConfiguration config)
        {
            _config = config;
        }

        public string GetWeatherByIPAddress()
        {
            return _config["AccuWeatherAPIKey"];
        }
        
    }
}