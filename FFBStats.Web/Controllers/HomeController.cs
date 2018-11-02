using System.Diagnostics;
using FFBStats.Business;
using FFBStats.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FFBStats.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IYahooFFBClient _yahooFfbClient;

        public HomeController(IConfiguration Configuration, IYahooFFBClient yahooFfbClient)
        {
            _configuration = Configuration;
            _yahooFfbClient = yahooFfbClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignInWithYahoo(string code)
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("HandleExternalLogin", "Authentication")
            };

            return Challenge(authenticationProperties);
        }

        [HttpGet]
        public IActionResult GetLeagues()
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            var gameIds = _yahooFfbClient.GetGameIds(accessToken);
            var leagues = _yahooFfbClient.GetLeagues(accessToken);
            var leaguesResult = leagues.Result;
            return Redirect("~/");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
