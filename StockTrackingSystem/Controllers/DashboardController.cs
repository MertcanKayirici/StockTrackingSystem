using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Data;
using System.Text.Json;

namespace StockTrackingSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var startOfYear = new DateTime(today.Year, 1, 1);
            var last7Days = today.AddDays(-6);

            var totalProducts = await _context.Products.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();
            var totalSuppliers = await _context.Suppliers.CountAsync();
            var totalMovements = await _context.StockMovements.CountAsync();

            var activeProducts = await _context.Products.CountAsync(x => x.IsActive);
            var passiveProducts = await _context.Products.CountAsync(x => !x.IsActive);
            var criticalStockCount = await _context.Products.CountAsync(x => x.StockQuantity <= x.CriticalStockLevel && x.StockQuantity > 0);
            var outOfStockCount = await _context.Products.CountAsync(x => x.StockQuantity == 0);

            var stockValueData = await _context.Products
                .Select(x => new
                {
                    x.UnitPrice,
                    x.StockQuantity
                })
                .ToListAsync();

            var totalStockValue = stockValueData.Sum(x => x.UnitPrice * x.StockQuantity);

            var latestProducts = await _context.Products
                .Include(x => x.Category)
                .OrderByDescending(x => x.CreatedDate)
                .Take(6)
                .ToListAsync();

            var latestMovements = await _context.StockMovements
                .Include(x => x.Product)
                .OrderByDescending(x => x.MovementDate)
                .Take(8)
                .ToListAsync();

            var lowStockProducts = await _context.Products
                .Include(x => x.Category)
                .Where(x => x.StockQuantity <= x.CriticalStockLevel)
                .OrderBy(x => x.StockQuantity)
                .ThenBy(x => x.Name)
                .Take(6)
                .ToListAsync();

            var categoryChartData = await _context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    Count = _context.Products.Count(p => p.CategoryId == c.Id)
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            var movementChartData = await _context.StockMovements
                .GroupBy(x => x.MovementType)
                .Select(g => new
                {
                    Type = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

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

            var yearlyMovementData = await _context.StockMovements
                .GroupBy(x => x.MovementDate.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ToListAsync();

            var monthNames = new[]
            {
        "", "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
        "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
    };

            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalSuppliers = totalSuppliers;
            ViewBag.TotalMovements = totalMovements;
            ViewBag.ActiveProducts = activeProducts;
            ViewBag.PassiveProducts = passiveProducts;
            ViewBag.CriticalStockCount = criticalStockCount;
            ViewBag.OutOfStockCount = outOfStockCount;
            ViewBag.TotalStockValue = totalStockValue;

            ViewBag.LatestProducts = latestProducts;
            ViewBag.LatestMovements = latestMovements;
            ViewBag.LowStockProducts = lowStockProducts;

            ViewBag.CategoryLabels = JsonSerializer.Serialize(categoryChartData.Select(x => x.Name));
            ViewBag.CategoryCounts = JsonSerializer.Serialize(categoryChartData.Select(x => x.Count));

            ViewBag.MovementLabels = JsonSerializer.Serialize(
                movementChartData.Select(x => x.Type == "In" ? "Giriş" : "Çıkış")
            );
            ViewBag.MovementCounts = JsonSerializer.Serialize(movementChartData.Select(x => x.Count));

            ViewBag.DailyMovementLabels = JsonSerializer.Serialize(
                recentDailyMovements.Select(x => x.Date.ToString("dd.MM"))
            );
            ViewBag.DailyMovementCounts = JsonSerializer.Serialize(recentDailyMovements.Select(x => x.Count));

            ViewBag.MonthlyMovementLabels = JsonSerializer.Serialize(
                monthlyMovementData.Select(x => monthNames[x.Month])
            );
            ViewBag.MonthlyMovementCounts = JsonSerializer.Serialize(
                monthlyMovementData.Select(x => x.Count)
            );

            ViewBag.YearlyMovementLabels = JsonSerializer.Serialize(
                yearlyMovementData.Select(x => x.Year.ToString())
            );
            ViewBag.YearlyMovementCounts = JsonSerializer.Serialize(
                yearlyMovementData.Select(x => x.Count)
            );

            return View();
        }
    }
}