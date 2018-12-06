using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FFBStats.Business;
using FFBStats.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using YahooFantasyWrapper.Client;
using YahooFantasyWrapper.Models;

namespace FFBStats.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IYahooFFBClient _yahooFfbClient;
        private readonly IYahooAuthClient _yahooAuthClient;

        public HomeController(IYahooFFBClient yahooFfbClient, IYahooAuthClient yahooAuthClient)
        {
            _yahooFfbClient = yahooFfbClient;
            _yahooAuthClient = yahooAuthClient;
        }

        public async Task<IActionResult> Index()
        {
            return View(GetHomeModel().Result);
        }

        public async Task<HomeModel> GetHomeModel()
        {
            var model = new HomeModel
            {
                AvailableYearsSelectListItems = GetAvailableYearsSelectListItems().OrderByDescending(m => m.Value)
            };

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var accessToken = await GetAccessToken();

                    var highScoreLowScoreYearList = new List<HighScoreLowScoreYear>();

                    foreach (var leagueId in LeagueIds.GetLeagueIds())
                    {
                        try
                        {
                            var year = leagueId.Key;
                            var lowHighScoresWeek = _yahooFfbClient
                                .GetHighLowScoreForYearAllTeams(year, accessToken);
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
                        catch (Exception ex) { }
                    }

                    model.HighScoreLowScoreYears = highScoreLowScoreYearList;
                    return model;
                }
            }
            catch { }

            return model;
        }

        private async Task<string> GetAccessToken()
        {
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var updatedAccessToken = await _yahooAuthClient.GetCurrentToken(refreshToken);

            _yahooAuthClient.Auth.AccessToken = updatedAccessToken;

            return updatedAccessToken;
        }

        private IEnumerable<SelectListItem> GetAvailableYearsSelectListItems()
        {
            var leagueIds = LeagueIds.GetLeagueIds().OrderBy(l => l.Key);
            var startingYear = leagueIds.First().Key;
            var endingYear = leagueIds.Last().Key;
            List<SelectListItem> yearSelectListItems = new List<SelectListItem>();
            for (int i = 0; i <= endingYear - startingYear; i++)
            {
                var yearString = (startingYear + i).ToString();
                yearSelectListItems.Add(new SelectListItem(yearString, yearString));
            }

            return yearSelectListItems;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult UpdateYear(int selectedYear)
        {
            return RedirectToAction("Index", "Record", new { year=selectedYear });
        }
    }
}
