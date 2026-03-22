using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Models;

namespace StockTrackingSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Product>()
                .Property(x => x.ProductCode)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Product>()
                .Property(x => x.UnitType)
                .IsRequired()
                .HasMaxLength(30);

            modelBuilder.Entity<Product>()
                .Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Supplier>()
                .Property(x => x.CompanyName)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<StockMovement>()
                .Property(x => x.MovementType)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}