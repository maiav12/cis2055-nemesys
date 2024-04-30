using Microsoft.AspNetCore.Mvc;
using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;

namespace Nemesys.Controllers

    {
        public class HallOfFameController : Controller
        {
            private readonly INemesysRepository _nemesysRepository;

            public HallOfFameController(INemesysRepository nemesysRepository)
            {
                _nemesysRepository = nemesysRepository;
            }

            public IActionResult Index()
            {
                try
                {
                    // Get the current year
                    int currentYear = DateTime.Now.Year;

                    // Retrieve all near miss reports for the current year
                    var reportsForCurrentYear = _nemesysRepository.GetAllNearMissReports()
                        .Where(r => r.DateOfReport.Year == currentYear);

                    // Group reports by reporter and count the number of reports made by each reporter
                    var reporterRanking = reportsForCurrentYear
                        .GroupBy(r => r.ReporterEmail)
                        .Select(g => new ReporterRankingViewModel
                        {
                            ReporterEmail = g.Key,
                            NumberOfReports = g.Count()
                        })
                        .OrderByDescending(r => r.NumberOfReports)
                        .ToList();

                    return View(reporterRanking);
                }
                catch (Exception ex)
                {
                    // Handle exception
                    return View("Error");
                }
            }
        }
    }
