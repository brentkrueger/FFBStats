using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using YahooFantasyWrapper.Client;
using YahooFantasyWrapper.Models;

namespace FFBStats.Business
{
    public class YahooFFBClient : IYahooFFBClient
    {
        private readonly IYahooFantasyClient _yahooFantasyClient;
        private readonly IConfiguration _configuration;
        private readonly string _leagueKey;
        private readonly int?[] _allWeekArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        private const string MatchupStatus = "postevent";

        public YahooFFBClient(IYahooFantasyClient yahooFantasyClient, IConfiguration configuration)
        {
            _yahooFantasyClient = yahooFantasyClient;
            _configuration = configuration;
            _leagueKey = _configuration["YahooFantasyFootballLeagueKey"];
        }
        
        public ScoreTeamWeek GetMaxScoreCurrentYear(string token)
        {
            var league = _yahooFantasyClient.LeagueResourceManager.GetScoreboard(_leagueKey, token, _allWeekArray).Result;

            List<ScoreboardTeam> scoreBoardTeams= new List<ScoreboardTeam>();

            foreach (var matchup in league.Scoreboard.Matchups.Matchups.Where(m=>m.Status.ToLower().Equals(MatchupStatus)))
            {
                scoreBoardTeams.AddRange(matchup.Teams.Teams);
            }

            var max = scoreBoardTeams.OrderBy(t => t.TeamPoints.Total).Last();

            return new ScoreTeamWeek() {Points = max.TeamPoints.Total, TeamName = max.Name, Week = max.TeamPoints.Week};
        }

        public ScoreTeamWeek GetMinScoreCurrentYear(string token)
        {
            var league = _yahooFantasyClient.LeagueResourceManager.GetScoreboard(_leagueKey, token, _allWeekArray).Result;

            List<ScoreboardTeam> scoreBoardTeams = new List<ScoreboardTeam>();

            foreach (var matchup in league.Scoreboard.Matchups.Matchups.Where(m => m.Status.ToLower().Equals(MatchupStatus)))
            {
                scoreBoardTeams.AddRange(matchup.Teams.Teams);
            }

            var min = scoreBoardTeams.OrderBy(t => t.TeamPoints.Total).First();

            return new ScoreTeamWeek() { Points = min.TeamPoints.Total, TeamName = min.Name, Week = min.TeamPoints.Week };
        }
    }
}