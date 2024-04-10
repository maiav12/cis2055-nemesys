using System;
using System.ComponentModel.DataAnnotations;

namespace Nemesys.Models
{
    public class NearMissReport
    {
        public int Id { get; set; }

        [Display(Name = "Date of Report")]
        public DateTime DateOfReport { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Display(Name = "Date and Time Spotted")]
        public DateTime DateTimeSpotted { get; set; }

        [Required(ErrorMessage = "Type of Hazard is required")]
        [EnumDataType(typeof(HazardType), ErrorMessage = "Invalid Hazard Type")]
        public HazardType TypeOfHazard { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public ReportStatus Status { get; set; } = ReportStatus.Open;

        [Required(ErrorMessage = "Reporter's Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Reporter's Email")]
        public string ReporterEmail { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Reporter's Phone (Optional)")]
        public string ReporterPhone { get; set; }

        [Display(Name = "Optional Photo")]
        public string PhotoUrl { get; set; }

        public int Upvotes { get; set; }

        // Navigation property for investigation
        public Investigation Investigation { get; set; }
    }

    public enum HazardType
    {
        UnsafeAct,
        Condition,
        Equipment,
        Structure
    }

    public enum ReportStatus
    {
        Open,
        BeingInvestigated,
        NoActionRequired,
        Closed
    }
}
