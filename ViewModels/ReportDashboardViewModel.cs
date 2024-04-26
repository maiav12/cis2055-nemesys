using System.ComponentModel.DataAnnotations;

namespace Nemesys.ViewModels
{
    public class NemesysDashboardViewModel
    {
        [Display(Name = "Total Reports")]
        public int TotalReports { get; set; }

        [Display(Name = "Total Registered Users")]
        public int TotalRegisteredUsers { get; set; }
    }
}
