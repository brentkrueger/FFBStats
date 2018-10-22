using System.Diagnostics;
using FFBStats.Business;
using FFBStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FFBStats.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherRetrievalService _weatherRetrievalService;

        public HomeController(IWeatherRetrievalService weatherRetrievalService)
        {
            _weatherRetrievalService = weatherRetrievalService;
        }

        public IActionResult Index()
        {
            return View();
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
