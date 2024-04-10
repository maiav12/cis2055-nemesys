using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nemesys.Models.Contexts;
using Nemesys.Models.Interfaces;

namespace Nemesys.Models.Repositories
{
    public class NemesysRepository : INemesysRepository
    {
        private readonly AppDbContext _appDbContext;

        public NemesysRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<NearMissReport> GetAllNearMissReports()
        {
            return _appDbContext.NearMissReports
                .Include(r => r.Investigation)
                .OrderByDescending(r => r.DateOfReport)
                .ToList();
        }

        public NearMissReport GetNearMissReportById(int nearMissReportId)
        {
            return _appDbContext.NearMissReports
                .Include(r => r.Investigation)
                .FirstOrDefault(r => r.Id == nearMissReportId);
        }

        public void AddNearMissReport(NearMissReport report)
        {
            _appDbContext.NearMissReports.Add(report);
            _appDbContext.SaveChanges();
        }

        public void UpdateNearMissReport(NearMissReport report)
        {
            _appDbContext.NearMissReports.Update(report);
            _appDbContext.SaveChanges();
        }

        public void DeleteNearMissReport(int nearMissReportId)
        {
            var report = _appDbContext.NearMissReports.Find(nearMissReportId);
            if (report != null)
            {
                _appDbContext.NearMissReports.Remove(report);
                _appDbContext.SaveChanges();
            }
        }

        public IEnumerable<Investigation> GetAllInvestigations()
        {
            return _appDbContext.Investigations.ToList();
        }

        public Investigation GetInvestigationById(int investigationId)
        {
            return _appDbContext.Investigations.Find(investigationId);
        }

        public void AddInvestigation(Investigation investigation)
        {
            _appDbContext.Investigations.Add(investigation);
            _appDbContext.SaveChanges();
        }

        public void UpdateInvestigation(Investigation investigation)
        {
            _appDbContext.Investigations.Update(investigation);
            _appDbContext.SaveChanges();
        }

        public void DeleteInvestigation(int investigationId)
        {
            var investigation = _appDbContext.Investigations.Find(investigationId);
            if (investigation != null)
            {
                _appDbContext.Investigations.Remove(investigation);
                _appDbContext.SaveChanges();
            }
        }
    }
}
