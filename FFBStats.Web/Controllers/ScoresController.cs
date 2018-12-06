using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFBStats.Business;
using Microsoft.AspNetCore.Mvc;
using YahooFantasyWrapper.Client;
namespace FFBStats.Web.Controllers
{
    public class ScoresController : BaseController
    {
        private readonly IYahooFFBClient _yahooFfbClient;

        public ScoresController(IYahooAuthClient yahooAuthClient, IYahooFFBClient yahooFfbClient) : base(
            yahooAuthClient)
        {
            _yahooFfbClient = yahooFfbClient;
        }

        // GET
        public IActionResult Index()
        {
            return View(GetModel().Result);
        }

        public async Task<ScoresModel> GetModel()
        {
            var model = new ScoresModel
            {
                HighScoreLowScoreYears = new List<HighScoreLowScoreYear>()
            };

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    model.HighScoreLowScoreYears = await GetHighLowScoresList();
                    return model;
                }
            }
            catch
            {
            }

            return model;
        }

        private async Task<List<HighScoreLowScoreYear>> GetHighLowScoresList()
        {
            var accessToken = await GetAccessToken();

            var highScoreLowScoreYearList = new List<HighScoreLowScoreYear>();

            foreach (var leagueId in LeagueIds.GetLeagueIds())
            {
                try
                {
                    var year = leagueId.Key;
                    var lowHighScoresWeek = _yahooFfbClient
                        .GetHighLowScoreForYearWholeLeague(year, accessToken);
                    var lowScore = lowHighScoresWeek.LowScoreTeamWeek;
                    var highScore = lowHighScoresWeek.HighScoreTeamWeek;
                    highScoreLowScoreYearList.Add(new HighScoreLowScoreYear()
                    {
                        Year = year,
                        HighScorePoints = highScore.Points,
                        HighScoreWeek = highScore.Week,
                        HighScoreTeamName = highScore.TeamName,
                        LowScorePoints = lowScore.Points,
                        LowScoreWeek = lowScore.Week,
                        LowScoreTeamName = lowScore.TeamName
                    });
                }
                catch (Exception ex)
                {
                }
            }

            return highScoreLowScoreYearList;
        }
    }
}
