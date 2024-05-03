using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using Nemesys.Models.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Nemesys.Controllers
{
    public class NearMissReportController : Controller
    {
        private readonly INemesysRepository _nemesysRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<NearMissReportController> _logger;


        public NearMissReportController(INemesysRepository nemesysRepository, UserManager<IdentityUser> userManager, ILogger<NearMissReportController> logger)
        {
            _nemesysRepository = nemesysRepository;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var allNearMissReports = _nemesysRepository.GetAllNearMissReports().OrderByDescending(b => b.DateOfReport);

                var model = new NearMissReportListViewModel()
                {
                    TotalEntries = allNearMissReports.Count(),
                    NearMissReports = allNearMissReports
                        .OrderByDescending(b => b.DateOfReport)
                        .Select(b => new NearMissReportViewModel
                        {
                            Id = b.Id,
                            Title = b.Title ?? "",
                            DateOfReport = b.DateOfReport,
                            Location = b.Location ?? "",
                            DateAndTimeSpotted = b.DateAndTimeSpotted,
                            TypeOfHazard = b.TypeOfHazard,
                            Description = b.Description,
                            Status = b.Status,
                            ReporterEmail = b.ReporterEmail,
                            ReporterPhone = b.ReporterPhone,
                            OptionalPhoto = b.OptionalPhoto,
                            Upvotes = b.Upvotes,

                            Investigation = new InvestigationViewModel()
                            {

                                Id = b.Id,
                                Description = "",
                                DateOfAction = DateTime.Now,
                                InvestigatorEmail = "",
                                InvestigatorPhone = ""
                            },

                            Reporter = new ReporterViewModel()
                            {
                                Id = b.UserId,
                                Name = (_userManager.FindByIdAsync(b.UserId).Result != null) ?
                                    _userManager.FindByIdAsync(b.UserId).Result.UserName : "Anonymous"
                            }
                        })
                        .ToList() // Explicitly convert to List<NearMissReportViewModel>
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }
        }


        public IActionResult Details(int id)
        {
            try
            {


                var nearMissReport = _nemesysRepository.GetNearMissReportById(id);
                if (nearMissReport == null)
                    return NotFound();
                else
                {
                    var model = new NearMissReportViewModel()
                    {
                        Id = nearMissReport.Id,
                        Title = nearMissReport.Title,
                        DateOfReport = nearMissReport.DateOfReport,
                        Location = nearMissReport.Location,
                        DateAndTimeSpotted = nearMissReport.DateAndTimeSpotted,
                        TypeOfHazard = nearMissReport.TypeOfHazard,
                        Description = nearMissReport.Description,
                        Status = nearMissReport.Status,
                        ReporterEmail = nearMissReport.ReporterEmail,
                        ReporterPhone = nearMissReport.ReporterPhone,
                        OptionalPhoto = nearMissReport.OptionalPhoto,
                        Upvotes = nearMissReport.Upvotes,

                        Investigation = new InvestigationViewModel()
                        {

                            Id = nearMissReport.Id,
                            Description = "",
                            DateOfAction = DateTime.Now,
                            InvestigatorEmail = "",
                            InvestigatorPhone = ""
                        },

                        Reporter = new ReporterViewModel()
                        {
                            Id = nearMissReport.UserId,
                            Name = (_userManager.FindByIdAsync(nearMissReport.UserId).Result != null) ?
                        _userManager.FindByIdAsync(nearMissReport.UserId).Result.UserName : "Anonymous"
                        }
                    };

                    return View(model);
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error");
            }
        }
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                var model = new EditNearMissReportViewModel();

                // Pass the view model to the view
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }
        }


        [Authorize]
        [HttpPost]
        public IActionResult Create([Bind("Title, Location, DateAndTimeSpotted, TypeOfHazard, Description, ReporterEmail, ReporterPhone, OptionalPhoto")] EditNearMissReportViewModel newNearMissReport)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string fileName = "";
                    if (newNearMissReport.OptionalPhoto != null && newNearMissReport.OptionalPhoto.Length > 0)
                    {
                        var extension = "." + newNearMissReport.OptionalPhoto.FileName.Split('.').Last();
                        fileName = Guid.NewGuid().ToString() + extension;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "nearmissreports", fileName);
                        using (var bits = new FileStream(path, FileMode.Create))
                        {
                            newNearMissReport.OptionalPhoto.CopyTo(bits);
                        }
                    }

                    NearMissReport nearMissReport = new NearMissReport()
                    {
                        Title = newNearMissReport.Title,
                        DateOfReport = DateTime.Now, // Set the current date and time
                        Location = newNearMissReport.Location,
                        DateAndTimeSpotted = newNearMissReport.DateAndTimeSpotted,
                        TypeOfHazard = newNearMissReport.TypeOfHazard,
                        Description = newNearMissReport.Description,
                        ReporterEmail = newNearMissReport.ReporterEmail,
                        ReporterPhone = newNearMissReport.ReporterPhone,
                        OptionalPhoto = fileName, // Assign the file name here
                        Upvotes = newNearMissReport.Upvotes,
                        UserId = _userManager.GetUserId(User)
                    };
                    // Persist to the repository
                    _nemesysRepository.AddNearMissReport(nearMissReport);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newNearMissReport);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }
        }




        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var existingNearMissReport = _nemesysRepository.GetNearMissReportById(id);
                if (existingNearMissReport != null)
                {
                    var currentUserId = _userManager.GetUserId(User);
                    if (existingNearMissReport.UserId == currentUserId || User.IsInRole("Admin"))
                        
                    {
                        var model = new EditNearMissReportViewModel()
                        {
                            Id = existingNearMissReport.Id,
                            DateOfReport = existingNearMissReport.DateOfReport,
                            Location = existingNearMissReport.Location,
                            DateAndTimeSpotted = existingNearMissReport.DateAndTimeSpotted,
                            TypeOfHazard = existingNearMissReport.TypeOfHazard,
                            Description = existingNearMissReport.Description,
                            ReporterEmail = existingNearMissReport.ReporterEmail,
                            ReporterPhone = existingNearMissReport.ReporterPhone,
                            Upvotes = existingNearMissReport.Upvotes
                        };

                        // If the user is an admin, allow modification of report status
                        if (User.IsInRole("Admin"))
                        {
                            // Load all report status options
                            var statusOptions = Enum.GetValues(typeof(ReportStatus)).Cast<ReportStatus>().ToList();
                            model.StatusOptions = statusOptions;
                        }

                        return View(model);
                    }
                    else
                    {
                        return Forbid(); // or redirect to an error page or to the Index page
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }
        }


        [HttpPost]
        [Authorize]
        public IActionResult Edit([FromRoute] int id, EditNearMissReportViewModel updatedNearMissReport)
        {
            try
            {
                var modelToUpdate = _nemesysRepository.GetNearMissReportById(id);
                if (modelToUpdate == null)
                {
                    return NotFound();
                }
                var currentUserId = _userManager.GetUserId(User);
                if (modelToUpdate.UserId == currentUserId)
                {
                    if (ModelState.IsValid)
                    {
                        string optionalPhoto = "";
                        // Check if a new photo is uploaded
                        if (updatedNearMissReport.OptionalPhoto != null)
                        {
                            string fileName = "";
                            // Process the uploaded photo and update the model
                            var extension = "." + updatedNearMissReport.OptionalPhoto.FileName.Split('.')[updatedNearMissReport.OptionalPhoto.FileName.Split('.').Length - 1];
                            fileName = Guid.NewGuid().ToString() + extension;
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "nearmissreports", fileName);
                            using (var bits = new FileStream(path, FileMode.Create))
                            {
                                updatedNearMissReport.OptionalPhoto.CopyTo(bits);
                            }
                            modelToUpdate.OptionalPhoto = "/images/nearmissreports/" + fileName;
                        }
                        if (User.IsInRole("Admin"))
                        {
                            // Set the status to the value selected by the admin
                            modelToUpdate.Status = updatedNearMissReport.SelectedStatus.ToString();
                        }
                        else

                            modelToUpdate.OptionalPhoto = optionalPhoto;

                        // Update other properties of the model
                        modelToUpdate.Title = updatedNearMissReport.Title;
                        modelToUpdate.DateOfReport = updatedNearMissReport.DateOfReport;
                        modelToUpdate.Location = updatedNearMissReport.Location;
                        modelToUpdate.DateAndTimeSpotted = updatedNearMissReport.DateAndTimeSpotted;
                        modelToUpdate.TypeOfHazard = updatedNearMissReport.TypeOfHazard;
                        modelToUpdate.Description = updatedNearMissReport.Description;

                        modelToUpdate.ReporterEmail = updatedNearMissReport.ReporterEmail;
                        modelToUpdate.ReporterPhone = updatedNearMissReport.ReporterPhone;
                        modelToUpdate.Upvotes = updatedNearMissReport.Upvotes;
                        modelToUpdate.UserId = _userManager.GetUserId(User);
                        _nemesysRepository.UpdateNearMissReport(modelToUpdate);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(updatedNearMissReport);
                    }
                }
                else return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateInvestigation(int id)
        {
            var model = new InvestigationViewModel();
            // Optionally, you can pass the ID of the near miss report to the view model if needed
            model.NearMissReportId = id;
            return View(model);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public IActionResult CreateInvestigation(InvestigationViewModel model)
        {
            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Map view model to entity
                var investigation = new Investigation
                {
                    Description = model.Description,
                    DateOfAction = model.DateOfAction,
                    InvestigatorEmail = model.InvestigatorEmail,
                    InvestigatorPhone = model.InvestigatorPhone,
                    NearMissReportId = model.NearMissReportId // Assuming you have a property to link the investigation to the report
                };

                // Add investigation to repository
                _nemesysRepository.AddInvestigation(investigation);

                return Ok(); // Return success status
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, "An error occurred while creating the investigation."); // Return error status
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateStatus(int reportId, string status)
        {
            try
            {
                // Get the near miss report from repository
                var nearMissReport = _nemesysRepository.GetNearMissReportById(reportId);
                if (nearMissReport == null)
                {
                    return NotFound(); // Return not found status if report not found
                }

                // Update the status
                nearMissReport.Status = status;

                // Save changes
                _nemesysRepository.UpdateNearMissReport(nearMissReport);

                return RedirectToAction("Details", new { id = reportId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, "An error occurred while updating the report status."); // Return error status
            }
        }


    }
}