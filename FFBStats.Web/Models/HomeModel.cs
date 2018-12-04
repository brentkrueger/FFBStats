using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FFBStats.Web.Models
{
    public class HomeModel
    {
        public string MaxScoreCurrentYearTeamName;
        public int MaxScoreCurrentYearWeek;
        public double MaxScoreCurrentYearPoints;

        public string MinScoreCurrentYearTeamName;
        public int MinScoreCurrentYearWeek;
        public double MinScoreCurrentYearPoints;

        public int SelectedYear;
        public IEnumerable<SelectListItem> AvailableYearsSelectListItems;
    }
}