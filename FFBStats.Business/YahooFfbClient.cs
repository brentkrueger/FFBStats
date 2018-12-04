using System;
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
        private readonly int?[] _allWeekArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16};
        private const string MatchupStatus = "postevent";
        private Dictionary<int, string> GameIds;

        public YahooFFBClient(IYahooFantasyClient yahooFantasyClient)
        {
            _yahooFantasyClient = yahooFantasyClient;
        }

        private Dictionary<int,string> GetGameIds(string token)
        {
            if (GameIds == null)
            {
                GameIds = new Dictionary<int, string>();
                var leagueIds = LeagueIds.GetLeagueIds().OrderBy(l => l.Key);
                var startingYear = leagueIds.First().Key;
                var endingYear = leagueIds.Last().Key;
                List<int> years = new List<int>();
                for (int i = 0; i <= endingYear - startingYear; i++)
                {
                    years.Add(startingYear + i);
                }

                var games = _yahooFantasyClient.GameCollectionsManager.GetGames(new string[] { }, token, null,
                        new GameCollectionFilters()
                            {GameCodes = new[] {GameCode.nfl}, Seasons = years.ToArray()})
                    .Result;

                foreach (var game in games)
                {
                    GameIds.Add(Convert.ToInt32(game.Season), game.GameId);
                }
            }

            return GameIds;
        }

        private string GetLeagueKey(int year, string token)
        {
            var gameId = GetGameIds(token).First(g => g.Key.Equals(year));
            var leagueId = LeagueIds.GetLeagueIds().First(l => l.Key.Equals(year)).Value;
            return $"{gameId.Value}.l.{leagueId}";
        }

        public ScoreTeamWeek GetMaxScoreForYearAllTeams(int year, string token)
        {
            var league = _yahooFantasyClient.LeagueResourceManager.GetScoreboard(GetLeagueKey(year, token), token, _allWeekArray).Result;

            List<ScoreboardTeam> scoreBoardTeams= new List<ScoreboardTeam>();

            foreach (var matchup in league.Scoreboard.Matchups.Matchups.Where(m=>m.Status.ToLower().Equals(MatchupStatus)))
            {
                scoreBoardTeams.AddRange(matchup.Teams.Teams);
            }

            var max = scoreBoardTeams.OrderBy(t => t.TeamPoints.Total).Last();

            return new ScoreTeamWeek() {Points = max.TeamPoints.Total, TeamName = max.Name, Week = max.TeamPoints.Week};
        }

        public ScoreTeamWeek GetMinScoreForYearAllTeams(int year, string token)
        {
            var league = _yahooFantasyClient.LeagueResourceManager.GetScoreboard(GetLeagueKey(year, token), token, _allWeekArray).Result;

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