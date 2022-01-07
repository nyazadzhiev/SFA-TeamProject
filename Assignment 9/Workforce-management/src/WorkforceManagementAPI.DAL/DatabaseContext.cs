using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.DAL
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration _config;

        public DatabaseContext(IConfiguration config) : base()
        {
            _config = config;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TimeOff> Requests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer(_config.GetConnectionString("Default"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupUserConfiguration(modelBuilder);
            SetupTeamConfiguration(modelBuilder);
            SetupRequestsConfiguration(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void SetupRequestsConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeOff>().HasKey(t => t.Id);
            modelBuilder.Entity<TimeOff>().Property(t => t.Reason).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<TimeOff>().Property(t => t.Status).IsRequired();
            modelBuilder.Entity<TimeOff>().Property(t => t.Type).IsRequired();
            modelBuilder.Entity<TimeOff>().Property(t => t.StartDate).IsRequired();
            modelBuilder.Entity<TimeOff>().Property(t => t.EndDate).IsRequired();
            modelBuilder.Entity<TimeOff>().HasOne(t => t.Creator).WithMany().HasForeignKey(t => t.CreatorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TimeOff>().HasOne(t => t.Modifier).WithMany().HasForeignKey(t => t.ModifierId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TimeOff>().HasMany(t => t.Reviewers).WithMany(u => u.Requests);
        }

        private static void SetupTeamConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().HasKey(t => t.Id);
            modelBuilder.Entity<Team>().Property(t => t.Title).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Team>().HasOne(t => t.TeamLeader).WithMany().HasForeignKey(t => t.TeamLeaderId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Team>().HasOne(t => t.Creator).WithMany().HasForeignKey(t => t.CreatorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Team>().HasOne(t => t.Modifier).WithMany().HasForeignKey(t => t.ModifierId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Team>().HasMany<User>(t => t.Users).WithMany(u => u.Teams);
        }

        private static void SetupUserConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().HasMany<Team>(u => u.Teams).WithMany(t => t.Users);
            modelBuilder.Entity<User>().HasMany<TimeOff>(u => u.Requests).WithMany(r => r.Reviewers);
        }
    }
}
