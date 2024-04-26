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
                // Load all investigations and create a list of InvestigationViewModel
                var investigationList = _nemesysRepository.GetAllInvestigations().Select(i => new InvestigationViewModel()
                {
                    Id = i.Id,
                    Description = i.Description,
                    DateOfAction = i.DateOfAction,
                    InvestigatorEmail = i.InvestigatorEmail,
                    InvestigatorPhone = i.InvestigatorPhone
                }).ToList();




                // Pass the lists into the view model
                var model = new EditNearMissReportViewModel()
                {
                    InvestigationList = investigationList,

                };

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
                    if (existingNearMissReport.UserId == currentUserId || User.IsInRole("Administrator"))
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

                        // Load all investigations and create a list of InvestigationViewModel
                        var investigationList = _nemesysRepository.GetAllInvestigations().Select(i => new InvestigationViewModel()
                        {
                            Id = i.Id,
                            Description = i.Description,
                            DateOfAction = i.DateOfAction,
                            InvestigatorEmail = i.InvestigatorEmail,
                            InvestigatorPhone = i.InvestigatorPhone
                        }).ToList();

                        // Attach to view model
                        model.InvestigationList = investigationList;

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
            try {
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
                } else return Forbid(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }
        }
    }
    }