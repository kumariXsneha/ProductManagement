using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Models;

namespace ProductManagement.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; } // for Auth and JWT
        public DbSet<Category> Categories { get; set; }   // NEW table

        // Fluent API: extra configuration EF reads when building the model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Relationship: Product has ONE Category, Category has MANY Products
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);   // can't delete a category that has products

            // 2. Seed data: rows inserted automatically by the migration
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Laptops, phones and gadgets" },
                new Category { Id = 2, Name = "Accessories", Description = "Mouse, keyboard, cables" },
                new Category { Id = 3, Name = "Furniture", Description = "Office chairs and desks" }
            );

            // Users: no two users with the same username or email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}