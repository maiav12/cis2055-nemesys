using System.Collections.Generic;

namespace Nemesys.Models.Interfaces
{
    public interface INemesysRepository
    {
        IEnumerable<NearMissReport> GetAllNearMissReports();
        NearMissReport GetNearMissReportById(int nearMissReportId);

        void AddNearMissReport(NearMissReport report);
        void UpdateNearMissReport(NearMissReport report);
        void DeleteNearMissReport(int nearMissReportId);

        IEnumerable<Investigation> GetAllInvestigations();
        Investigation GetInvestigationById(int investigationId);

        void AddInvestigation(Investigation investigation);
        void UpdateInvestigation(Investigation investigation);
        void DeleteInvestigation(int investigationId);
    }
}
