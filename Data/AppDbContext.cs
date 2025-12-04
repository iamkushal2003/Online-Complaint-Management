using Microsoft.EntityFrameworkCore;
using ComplaintSystem.Models;

namespace ComplaintSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<ComplaintAssignment> ComplaintAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // unique constraint on email
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // cascade delete behaviour or restrict as needed
            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.User)
                .WithMany(u => u.Complaints)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

