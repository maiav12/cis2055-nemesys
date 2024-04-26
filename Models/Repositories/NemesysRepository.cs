using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nemesys.Models.Contexts;
using Nemesys.Models.Interfaces;

namespace Nemesys.Models.Repositories
{
    public class NemesysRepository : INemesysRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly ILogger<NemesysRepository> _logger;

        public NemesysRepository(ApplicationDbContext appDbContext, ILogger<NemesysRepository> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

		public IEnumerable<NearMissReport> GetAllNearMissReports()
		{
            try 

			{//Using Eager loading with Include
                return _appDbContext.NearMissReports.OrderBy(r => r.DateOfReport); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
		}

		public NearMissReport GetNearMissReportById(int nearMissReportId)
		{
            try 
			{ return _appDbContext.NearMissReports
                .FirstOrDefault(r => r.Id == nearMissReportId); 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
		}

		public void AddNearMissReport(NearMissReport report)
        {
            try 
			
                
            {
                _appDbContext.NearMissReports.Add(report);
                _appDbContext.SaveChanges(); 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
			}



        public void UpdateNearMissReport(NearMissReport nearMissReport)
        {
            try 
           { 
                var existingReport = _appDbContext.NearMissReports.SingleOrDefault(r => r.Id == nearMissReport.Id);
                if (existingReport != null)
                {
                    existingReport.Title = nearMissReport.Title;
                    existingReport.DateOfReport = nearMissReport.DateOfReport;
                    existingReport.Location = nearMissReport.Location;
                    existingReport.DateAndTimeSpotted = nearMissReport.DateAndTimeSpotted;
                    existingReport.TypeOfHazard = nearMissReport.TypeOfHazard;
                    existingReport.Description = nearMissReport.Description;
                    existingReport.Status = nearMissReport.Status;
                    existingReport.ReporterEmail = nearMissReport.ReporterEmail;
                    existingReport.ReporterPhone = nearMissReport.ReporterPhone;
                    existingReport.OptionalPhoto = nearMissReport.OptionalPhoto;
                    existingReport.Upvotes = nearMissReport.Upvotes;

                    _appDbContext.Entry(existingReport).State = EntityState.Modified;
                    _appDbContext.SaveChanges();
                } 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public void DeleteNearMissReport(int nearMissReportId)
        {
            try 
            {
                var report = _appDbContext.NearMissReports.Find(nearMissReportId);
                if (report != null)
                {
                    _appDbContext.NearMissReports.Remove(report);
                    _appDbContext.SaveChanges();
                } 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex );
                throw;
            }
        }

        public IEnumerable<Investigation> GetAllInvestigations()
        {
            try 
            { 
                return _appDbContext.Investigations.ToList(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public Investigation GetInvestigationById(int investigationId)
        {
            try 
            { 
                return _appDbContext.Investigations.Find(investigationId); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public void AddInvestigation(Investigation investigation)
        {
            try 
            {
                _appDbContext.Investigations.Add(investigation);
                _appDbContext.SaveChanges(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public void UpdateInvestigation(Investigation investigation)
        {
            try 
            {
                _appDbContext.Investigations.Update(investigation);
                _appDbContext.SaveChanges(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public void DeleteInvestigation(int investigationId)
        {
            try
            { 
            var investigation = _appDbContext.Investigations.Find(investigationId);
                if (investigation != null)
                {
                    _appDbContext.Investigations.Remove(investigation);
                    _appDbContext.SaveChanges();
                } 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
