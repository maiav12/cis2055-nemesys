using System;
using System.ComponentModel.DataAnnotations;

namespace Nemesys.ViewModels
{
    public class InvestigationViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Date of Action")]
        public DateTime DateOfAction { get; set; }

        [Display(Name = "Investigator Email")]
        public string InvestigatorEmail { get; set; }

        [Display(Name = "Investigator Phone")]
        public string InvestigatorPhone { get; set; }
    }
}
