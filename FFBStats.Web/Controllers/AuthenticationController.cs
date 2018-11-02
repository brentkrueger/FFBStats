using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FFBStats.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public async Task<IActionResult> HandleExternalLogin()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");




            return Redirect("~/");
        }
    }
}