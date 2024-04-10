using System.Collections.Generic;
using Nemesys.Models; 
namespace Nemesys.ViewModels
{
    public class NearMissReportListViewModel
    {
        public int TotalEntries { get; set; }
        public IEnumerable<NearMissReport> NearMissReports { get; set; }
    }
}
