using Medi_Connect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<PatientProfile> Patients { get; set; }
        public DbSet<NurseProfile> NurseProfiles { get; set; }
        public DbSet<RelativeProfile> RelativeProfiles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasOne(u => u.NurseProfile)
                .WithOne(n => n.User)
                .HasForeignKey<NurseProfile>(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RelativeProfile>()
                .HasOne(rp => rp.Patient)
                .WithMany(p => p.LinkedRelatives)
                .HasForeignKey(rp => rp.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RelativeProfile>()
                .HasOne(rp => rp.User)
                .WithOne(u => u.PatientRelative)
                .HasForeignKey<RelativeProfile>(rp => rp.RelativeId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<NurseProfile>()
                .HasOne(np => np.User)
                .WithOne(u => u.NurseProfile)
                .HasForeignKey<NurseProfile>(np => np.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PatientProfile>()
                .HasOne(p => p.Relative)
                .WithMany(u => u.Patients)
                .HasForeignKey(p => p.CreatedByRelativeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RelativeProfile>()
                .HasOne(rp => rp.Patient)
                .WithMany(p => p.LinkedRelatives)
                .HasForeignKey(rp => rp.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
