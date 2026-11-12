using Microsoft.EntityFrameworkCore;
using StudentPortal.Models;

namespace StudentPortal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Starting data, so the table isn't empty the first time
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, Name = "Kumari", Course = "C#", Marks = 92, City = "Patna" },
                new Student { Id = 2, Name = "Rahul", Course = "SQL", Marks = 78, City = "Delhi" },
                new Student { Id = 3, Name = "Priya", Course = "MVC", Marks = 85, City = "Pune" }
            );
        }
    }
}