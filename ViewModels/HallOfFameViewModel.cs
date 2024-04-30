using System.Collections.Generic;

namespace Nemesys.ViewModels
{
    public class HallOfFameViewModel
    {
        public List<ReporterRankingViewModel> ReporterRankings { get; set; }
    }

    public class ReporterRankingViewModel
    {
        public string ReporterEmail { get; set; }
        public int NumberOfReports { get; set; }
    }
}
