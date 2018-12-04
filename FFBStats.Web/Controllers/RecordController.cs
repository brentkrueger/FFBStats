﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FFBStats.Business;
using FFBStats.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FFBStats.Web.Controllers
{
    public class RecordController : Controller
    {
        private readonly IYahooFFBClient _yahooFfbClient;

        public RecordController(IYahooFFBClient yahooFfbClient)
        {
            _yahooFfbClient = yahooFfbClient;
        }

        public async Task<IActionResult> Index(int? year)
        {
            return View(GetHomeModel(year).Result);
        }

        public async Task<HomeModel> GetHomeModel(int? year)
        {
            var model = new HomeModel
            {
                AvailableYearsSelectListItems = GetAvailableYearsSelectListItems().OrderByDescending(m => m.Value)
            };

            model.SelectedYear = year ?? DateTime.Now.Year;

            try
            {
                //todo figure out  refresh token implementation
                if (User.Identity.IsAuthenticated)
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");

                    var maxScoreTeamWeek = _yahooFfbClient.GetMaxScoreForYearAllTeams(model.SelectedYear, accessToken);
                    var minScoreTeamWeek = _yahooFfbClient.GetMinScoreForYearAllTeams(model.SelectedYear, accessToken);

                    model.MaxScoreCurrentYearPoints = maxScoreTeamWeek.Points;
                    model.MaxScoreCurrentYearTeamName = maxScoreTeamWeek.TeamName;
                    model.MaxScoreCurrentYearWeek = maxScoreTeamWeek.Week;

                    model.MinScoreCurrentYearPoints = minScoreTeamWeek.Points;
                    model.MinScoreCurrentYearTeamName = minScoreTeamWeek.TeamName;
                    model.MinScoreCurrentYearWeek = minScoreTeamWeek.Week;

                    return model;
                }
            }
            catch { }

            return model;
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