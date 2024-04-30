using System.ComponentModel.DataAnnotations;

namespace Nemesys.ViewModels
{
    public class ReportDashboardViewModel
    {
        [Display(Name = "Total Reports")]
        public int TotalReports { get; set; }

        [Display(Name = "Total Registered Users")]
        public int TotalRegisteredUsers { get; set; }
    }
}
