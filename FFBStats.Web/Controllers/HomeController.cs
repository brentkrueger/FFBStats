using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YahooFantasyWrapper.Client;

namespace FFBStats.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IYahooAuthClient yahooAuthClient) : base(yahooAuthClient)
        {
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
