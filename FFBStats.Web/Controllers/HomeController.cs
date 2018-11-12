using System.Diagnostics;
using System.Threading.Tasks;
using FFBStats.Business;
using FFBStats.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using YahooFantasyWrapper.Client;

namespace FFBStats.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IYahooFFBClient _yahooFfbClient;

        public HomeController(IYahooFFBClient yahooFfbClient)
        {
            _yahooFfbClient = yahooFfbClient;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeModel();
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    var maxScoreAllTime = _yahooFfbClient.GetMaxScoreAllTime(accessToken);
                    model.MaxScoreAllTime = maxScoreAllTime.Points;
                    model.MaxScoreAllTimeYear = maxScoreAllTime.Year;
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
