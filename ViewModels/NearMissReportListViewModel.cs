using System.Collections.Generic;
using Nemesys.Models;

namespace Nemesys.ViewModels
{
    public class NearMissReportListViewModel
    {
        public List<NearMissReport> NearMissReports { get; set; }
        public List<NearMissReport> FilteredNearMissReports { get; set; }
        public int TotalEntries { get; set; }
    }
}