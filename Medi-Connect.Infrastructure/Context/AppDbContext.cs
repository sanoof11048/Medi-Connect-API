using Medi_Connect.Domain.Common;
using Medi_Connect.Domain.Models.Admin;
using Medi_Connect.Domain.Models.Other;
using Medi_Connect.Domain.Models.PatientDetails;
using Medi_Connect.Domain.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static QuestPDF.Helpers.Colors;

namespace Medi_Connect.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<NurseProfile> HomeNurses { get; set; }
        public DbSet<FoodLog> FoodLogs { get; set; }
        public DbSet<MedicationLog> MedicationLogs { get; set; }
        public DbSet<Vital> Vitals { get; set; }
        public DbSet<NurseRequest> NurseRequests { get; set; }
        public DbSet<NurseAssignment> NurseAssignments { get; set; }

        public DbSet<CareTypeRate> CareTypeRates { get; set; }
        public DbSet<NursePayment> NursePayments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                .Property(p => p.CareType)
                .HasConversion<string>();

            modelBuilder.Entity<Patient>()
                .Property(p => p.ServiceType)
                .HasConversion<string>();


            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.HomeNurse)
                .WithMany(u => u.PatientsAsHomeNurse)
                .HasForeignKey(p => p.HomeNurseId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Relative)
                .WithMany(u => u.PatientsAsRelative)
                .HasForeignKey(p => p.RelativeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NurseProfile>()
                .HasOne(np => np.User)
                .WithOne(u => u.NurseProfile)
                .HasForeignKey<NurseProfile>(np => np.HomeNurseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NurseAssignment>()
                .HasOne(na => na.Patient)
                .WithOne(p => p.NurseAssignment)
                .HasForeignKey<NurseAssignment>(na => na.PatientId)
                .OnDelete(DeleteBehavior.Restrict);




            modelBuilder.Entity<NurseAssignment>()
                .HasOne(na => na.Nurse)
                .WithMany()
                .HasForeignKey(na => na.NurseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NurseAssignment>()
                .HasOne(na => na.Request)
                .WithMany()
                .HasForeignKey(na => na.RequestId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<NurseAssignment>()
                .HasOne(na => na.Patient)
                .WithOne(p => p.NurseAssignment)
                .HasForeignKey<NurseAssignment>(na => na.PatientId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<NursePayment>()
     .HasOne(p => p.PaidBy)
     .WithMany()
     .HasForeignKey(p => p.PaidById)
     .OnDelete(DeleteBehavior.Restrict); // ✅ Critical to avoid cascade path error

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var userId = GetCurrentUserId();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = userId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = userId;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        entry.Entity.DeletedBy = userId;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var id) ? id : Guid.Empty;

        }
    }
}
