using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Data;
using System.Text.Json;

namespace StockTrackingSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // INDEX
        // =========================

        // Display dashboard data, statistics, charts and latest records
        public async Task<IActionResult> Index()
        {
            // Define date ranges
            var today = DateTime.Today;
            var startOfYear = new DateTime(today.Year, 1, 1);
            var last7Days = today.AddDays(-6);

            // General counts
            var totalProducts = await _context.Products.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();
            var totalSuppliers = await _context.Suppliers.CountAsync();
            var totalMovements = await _context.StockMovements.CountAsync();

            // Product status counts
            var activeProducts = await _context.Products.CountAsync(x => x.IsActive);
            var passiveProducts = await _context.Products.CountAsync(x => !x.IsActive);
            var criticalStockCount = await _context.Products.CountAsync(x => x.StockQuantity <= x.CriticalStockLevel && x.StockQuantity > 0);
            var outOfStockCount = await _context.Products.CountAsync(x => x.StockQuantity == 0);

            // Calculate total stock value
            var stockValueData = await _context.Products
                .Select(x => new
                {
                    x.UnitPrice,
                    x.StockQuantity
                })
                .ToListAsync();

            var totalStockValue = stockValueData.Sum(x => x.UnitPrice * x.StockQuantity);

            // Get latest created products
            var latestProducts = await _context.Products
                .Include(x => x.Category)
                .OrderByDescending(x => x.CreatedDate)
                .Take(6)
                .ToListAsync();

            // Get latest stock movements
            var latestMovements = await _context.StockMovements
                .Include(x => x.Product)
                .OrderByDescending(x => x.MovementDate)
                .Take(8)
                .ToListAsync();

            // Get low stock products
            var lowStockProducts = await _context.Products
                .Include(x => x.Category)
                .Where(x => x.StockQuantity <= x.CriticalStockLevel)
                .OrderBy(x => x.StockQuantity)
                .ThenBy(x => x.Name)
                .Take(6)
                .ToListAsync();

            // Prepare category chart data
            var categoryChartData = await _context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    Count = _context.Products.Count(p => p.CategoryId == c.Id)
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            // Prepare movement type chart data
            var movementChartData = await _context.StockMovements
                .GroupBy(x => x.MovementType)
                .Select(g => new
                {
                    Type = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            // Prepare recent daily movement chart data
            var recentDailyMovements = await _context.StockMovements
                .Where(x => x.MovementDate >= last7Days)
                .GroupBy(x => x.MovementDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            // Prepare monthly movement chart data
            var monthlyMovementData = await _context.StockMovements
                .Where(x => x.MovementDate >= startOfYear)
                .GroupBy(x => x.MovementDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToListAsync();

            // Prepare yearly movement chart data
            var yearlyMovementData = await _context.StockMovements
                .GroupBy(x => x.MovementDate.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ToListAsync();

            // Define month names
            var monthNames = new[]
            {
                "", "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
                "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
            };

            // Transfer summary data to ViewBag
            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalSuppliers = totalSuppliers;
            ViewBag.TotalMovements = totalMovements;
            ViewBag.ActiveProducts = activeProducts;
            ViewBag.PassiveProducts = passiveProducts;
            ViewBag.CriticalStockCount = criticalStockCount;
            ViewBag.OutOfStockCount = outOfStockCount;
            ViewBag.TotalStockValue = totalStockValue;

            // Transfer latest lists to ViewBag
            ViewBag.LatestProducts = latestProducts;
            ViewBag.LatestMovements = latestMovements;
            ViewBag.LowStockProducts = lowStockProducts;

            // Transfer category chart data to ViewBag
            ViewBag.CategoryLabels = JsonSerializer.Serialize(categoryChartData.Select(x => x.Name));
            ViewBag.CategoryCounts = JsonSerializer.Serialize(categoryChartData.Select(x => x.Count));

            // Transfer movement type chart data to ViewBag
            ViewBag.MovementLabels = JsonSerializer.Serialize(
                movementChartData.Select(x => x.Type == "In" ? "Giriş" : "Çıkış")
            );
            ViewBag.MovementCounts = JsonSerializer.Serialize(movementChartData.Select(x => x.Count));

            // Transfer daily movement chart data to ViewBag
            ViewBag.DailyMovementLabels = JsonSerializer.Serialize(
                recentDailyMovements.Select(x => x.Date.ToString("dd.MM"))
            );
            ViewBag.DailyMovementCounts = JsonSerializer.Serialize(recentDailyMovements.Select(x => x.Count));

            // Transfer monthly movement chart data to ViewBag
            ViewBag.MonthlyMovementLabels = JsonSerializer.Serialize(
                monthlyMovementData.Select(x => monthNames[x.Month])
            );
            ViewBag.MonthlyMovementCounts = JsonSerializer.Serialize(
                monthlyMovementData.Select(x => x.Count)
            );

            // Transfer yearly movement chart data to ViewBag
            ViewBag.YearlyMovementLabels = JsonSerializer.Serialize(
                yearlyMovementData.Select(x => x.Year.ToString())
            );
            ViewBag.YearlyMovementCounts = JsonSerializer.Serialize(
                yearlyMovementData.Select(x => x.Count)
            );

            // Get latest audit logs
            var latestAuditLogs = await _context.AuditLogs
                .OrderByDescending(x => x.CreatedDate)
                .Take(8)
                .ToListAsync();

            // Transfer audit logs to ViewBag
            ViewBag.LatestAuditLogs = latestAuditLogs;

            return View();
        }
    }
}