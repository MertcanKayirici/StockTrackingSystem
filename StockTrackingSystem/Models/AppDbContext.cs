using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Models;

namespace StockTrackingSystem.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // =========================
        // DBSets
        // =========================

        // Category table
        public DbSet<Category> Categories { get; set; }

        // Product table
        public DbSet<Product> Products { get; set; }

        // Supplier table
        public DbSet<Supplier> Suppliers { get; set; }

        // Stock movement table
        public DbSet<StockMovement> StockMovements { get; set; }

        // Audit log table
        public DbSet<AuditLog> AuditLogs { get; set; }

        // =========================
        // MODEL CONFIGURATION
        // =========================

        // Configure entity relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category configuration
            modelBuilder.Entity<Category>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Product configuration - Name
            modelBuilder.Entity<Product>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            // Product configuration - Code
            modelBuilder.Entity<Product>()
                .Property(x => x.ProductCode)
                .IsRequired()
                .HasMaxLength(50);

            // Product configuration - Unit Type
            modelBuilder.Entity<Product>()
                .Property(x => x.UnitType)
                .IsRequired()
                .HasMaxLength(30);

            // Product configuration - Price
            modelBuilder.Entity<Product>()
                .Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            // Supplier configuration
            modelBuilder.Entity<Supplier>()
                .Property(x => x.CompanyName)
                .IsRequired()
                .HasMaxLength(150);

            // StockMovement configuration
            modelBuilder.Entity<StockMovement>()
                .Property(x => x.MovementType)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}