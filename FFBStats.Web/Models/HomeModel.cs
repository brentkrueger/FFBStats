using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FFBStats.Web.Models
{
    public class HomeModel
    {
        public IEnumerable<HighScoreLowScoreYear> HighScoreLowScoreYears;

        public int SelectedYear;
        public IEnumerable<SelectListItem> AvailableYearsSelectListItems;
    }
}