﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nemesys.Models;
using Nemesys.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NEMESYS.Controllers
{
    public class NearMissReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NearMissReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NearMissReport
        public async Task<IActionResult> Index()
        {
            return View(await _context.NearMissReports.ToListAsync());
        }

        // GET: NearMissReport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nearMissReport = await _context.NearMissReports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nearMissReport == null)
            {
                return NotFound();
            }

            return View(nearMissReport);
        }

        // GET: NearMissReport/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NearMissReport/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NearMissReport nearMissReport, IFormFile photoUrl)
        {
            if (ModelState.IsValid)
            {
                if (photoUrl != null)
                {
                    string fileName = Path.GetFileName(photoUrl.FileName);
                    string fileExtension = Path.GetExtension(fileName);
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/nearmissreports", uniqueFileName);

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await photoUrl.CopyToAsync(stream);
                    }

                    nearMissReport.PhotoUrl = $"/images/nearmissreports/{uniqueFileName}";
                }

                _context.Add(nearMissReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(nearMissReport);
        }

        // POST: NearMissReport/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DateSpotted,HazardType,ImageToUpload")] NearMissReport nearMissReport, IFormFile photoUrl)
        {
            if (id != nearMissReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (photoUrl != null)
                    {
                        string fileName = Path.GetFileName(photoUrl.FileName);
                        string fileExtension = Path.GetExtension(fileName);
                        string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                        string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/nearmissreports", uniqueFileName);

                        using (var stream = new FileStream(uploadPath, FileMode.Create))
                        {
                            await photoUrl.CopyToAsync(stream);
                        }

                        nearMissReport.PhotoUrl = $"/images/nearmissreports/{uniqueFileName}";
                    }

                    _context.Update(nearMissReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NearMissReportExists(nearMissReport.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(nearMissReport);
        }
        private bool NearMissReportExists(int id)
        {
            return _context.NearMissReports.Any(e => e.Id == id);
        }
    }
}