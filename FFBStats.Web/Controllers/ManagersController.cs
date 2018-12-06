using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFBStats.Business;
using Microsoft.AspNetCore.Mvc;
using YahooFantasyWrapper.Client;
using YahooFantasyWrapper.Models;

namespace FFBStats.Web.Controllers
{
    public class ManagersController : BaseController
    {
        private readonly IYahooFFBClient _yahooFfbClient;

        public ManagersController(IYahooAuthClient yahooAuthClient, IYahooFFBClient yahooFfbClient) : base(
            yahooAuthClient)
        {
            _yahooFfbClient = yahooFfbClient;
        }

        // GET
        public IActionResult Index()
        {
            return View(GetModel().Result);
        }

        public async Task<ManagersModel> GetModel()
        {
            var model = new ManagersModel()
            {
                ManagerAllTimeRecords = new List<ManagerAllTimeRecord>()
            };

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    model.ManagerAllTimeRecords = await GetManagerAllTimeRecords();
                    return model;
                }
            }
            catch
            {
            }

            return model;
        }

        private async Task<List<ManagerAllTimeRecord>> GetManagerAllTimeRecords()
        {
            var accessToken = await GetAccessToken();

            var managerAllTimeRecords = new List<ManagerAllTimeRecord>();

            var teams = _yahooFfbClient.GetAllTimeTeams(accessToken);
            var managerList = new List<Manager>();
            foreach (var team in teams)
            {
                foreach (var manager in team.ManagerList.Managers.Where(m=>!string.IsNullOrEmpty(m.Guid)))
                {
                    if (!managerList.Any(m => m.Guid.Equals(manager.Guid)))
                    {
                        managerList.Add(manager);
                    }
                }
            }

            foreach (var manager in managerList)
            {
                var wins = 0;
                var losses = 0;

                foreach (var team in teams)
                {
                    if (team.ManagerList.Managers.Where(m => !string.IsNullOrEmpty(m.Guid)).Any(m => m.Guid.Equals(manager.Guid)))
                    {
                        wins += Convert.ToInt32(team.TeamStandings.OutcomeTotals.Wins);
                        losses += Convert.ToInt32(team.TeamStandings.OutcomeTotals.Losses);
                    }
                }

                managerAllTimeRecords.Add(new ManagerAllTimeRecord()
                {
                    ManagerName = manager.Nickname,
                    Wins = wins,
                    Losses = losses,
                    WinningPercentage = Math.Round(((double)wins/((double)wins+(double)losses)),3)
                });
            }

            return managerAllTimeRecords.OrderByDescending(m=>m.WinningPercentage).ToList();
        }
        
    }
}
