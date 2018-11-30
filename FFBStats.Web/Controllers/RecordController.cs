using System.Diagnostics;
using System.Threading.Tasks;
using FFBStats.Business;
using FFBStats.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FFBStats.Web.Controllers
{
    public class RecordController : Controller
    {
        private readonly IYahooFFBClient _yahooFfbClient;

        public RecordController(IYahooFFBClient yahooFfbClient)
        {
            _yahooFfbClient = yahooFfbClient;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeModel();
            try
            {
                //todo figure out  refresh token implementation
                if (User.Identity.IsAuthenticated)
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");

                    var maxScoreTeamWeek = _yahooFfbClient.GetMaxScoreCurrentYear(accessToken);
                    var minScoreTeamWeek = _yahooFfbClient.GetMinScoreCurrentYear(accessToken);

                    model.MaxScoreCurrentYearPoints = maxScoreTeamWeek.Points;
                    model.MaxScoreCurrentYearTeamName = maxScoreTeamWeek.TeamName;
                    model.MaxScoreCurrentYearWeek = maxScoreTeamWeek.Week;

                    model.MinScoreCurrentYearPoints = minScoreTeamWeek.Points;
                    model.MinScoreCurrentYearTeamName = minScoreTeamWeek.TeamName;
                    model.MinScoreCurrentYearWeek = minScoreTeamWeek.Week;

                    return View(model);
                }
            }
            catch { }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
