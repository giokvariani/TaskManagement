using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.Domain.Entities;

namespace TaskManagement.Infrastructure.Persistence.DataLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(x => x.AssignedIssues)
                .WithOne(x => x.Assignee)
                .HasForeignKey(x => x.AssigneeId);

            modelBuilder.Entity<User>().HasMany(x => x.ReportedIssues)
                .WithOne(x => x.Reporter)
                .HasForeignKey(x => x.ReporterId);

            base.OnModelCreating(modelBuilder); 
        }
    }
}
