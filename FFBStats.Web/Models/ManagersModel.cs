using System.Collections.Generic;

namespace FFBStats.Web
{
    public class ManagerAllTimeRecord
    {
        public string ManagerName;
        public int Wins;
        public int Losses;
        public double WinningPercentage;
    }

    public class ManagersModel
    {
        public IEnumerable<ManagerAllTimeRecord> ManagerAllTimeRecords;
    }
}