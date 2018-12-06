using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using YahooFantasyWrapper.Client;

namespace FFBStats.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly IYahooAuthClient _yahooAuthClient;

        public BaseController(IYahooAuthClient yahooAuthClient)
        {
            _yahooAuthClient = yahooAuthClient;
        }

        protected async Task<string> GetAccessToken()
        {
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var updatedAccessToken = await _yahooAuthClient.GetCurrentToken(refreshToken);

            _yahooAuthClient.Auth.AccessToken = updatedAccessToken;

            return updatedAccessToken;
        }
    }
}