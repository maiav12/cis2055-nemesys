﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Nemesys.Models
{
    public enum UserRole
    {
        Admin,
        User
    }
    public class Investigation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Description of Investigation is required")]
        public string Description { get; set; }

        [Display(Name = "Date of Action")]
        public DateTime DateOfAction { get; set; }

        [Required(ErrorMessage = "Investigator's Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Investigator's Email")]
        public string InvestigatorEmail { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Investigator's Phone (Optional)")]
        public string? InvestigatorPhone { get; set; } // Nullable phone property

        [Display(Name = "Report Status")]
        public string ReportStatus { get; set; }

        // Foreign key property for associated NearMissReport
        public int NearMissReportId { get; set; }

        // Navigation property for associated NearMissReport
        public NearMissReport NearMissReport { get; set; }

        public UserRole Role { get; set; }


    }
}