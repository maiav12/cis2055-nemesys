

using Microsoft.EntityFrameworkCore;

namespace Nemesys.Models.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // Constructor passes options to base DbContext
        }

        // DbSet for Near Miss Report entity
        public DbSet<NearMissReport> NearMissReports { get; set; }

        // DbSet for Investigation entity
        public DbSet<Investigation> Investigations { get; set; }

        // Override OnModelCreating to configure data seeding
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure seeding for Near Miss Report entities
            modelBuilder.Entity<NearMissReport>().HasData(
                new NearMissReport
                {
                    Id = 1,
                    DateOfReport = DateTime.UtcNow,
                    Location = "Example Location",
                    DateTimeSpotted = DateTime.UtcNow,
                    TypeOfHazard = HazardType.UnsafeAct,
                    Description = "Example Description",
                    Status = ReportStatus.Open,
                    ReporterEmail = "example@email.com",
                    // Additional properties...
                }
                // Additional Near Miss Reports...
            );

            // Configure seeding for Investigation entities
            modelBuilder.Entity<Investigation>().HasData(
                new Investigation
                {
                    Id = 1,
                    NearMissReportId = 1, // Corresponds to the Near Miss Report above
                    Description = "Example Investigation Description",
                    DateOfAction = DateTime.UtcNow,
                    InvestigatorEmail = "investigator@email.com",
                    // Additional properties...
                }
                // Additional Investigations...
            );
        }
    }
}
