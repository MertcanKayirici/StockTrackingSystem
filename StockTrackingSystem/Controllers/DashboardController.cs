using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Data;

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
            var totalProducts = await _context.Products.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();
            var totalSuppliers = await _context.Suppliers.CountAsync();
            var criticalStockCount = await _context.Products.CountAsync(x => x.StockQuantity <= x.CriticalStockLevel && x.StockQuantity > 0);
            var passiveProductCount = await _context.Products.CountAsync(x => !x.IsActive);
            var totalMovements = await _context.StockMovements.CountAsync();

            var latestProducts = await _context.Products
                .Include(x => x.Category)
                .OrderByDescending(x => x.CreatedDate)
                .Take(5)
                .ToListAsync();

            var latestMovements = await _context.StockMovements
                .Include(x => x.Product)
                .OrderByDescending(x => x.MovementDate)
                .Take(6)
                .ToListAsync();

            var categoryData = await _context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    ProductCount = _context.Products.Count(p => p.CategoryId == c.Id)
                })
                .OrderByDescending(x => x.ProductCount)
                .ToListAsync();

            var movementSummary = await _context.StockMovements
                .GroupBy(x => x.MovementType)
                .Select(g => new
                {
                    MovementType = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalSuppliers = totalSuppliers;
            ViewBag.CriticalStockCount = criticalStockCount;
            ViewBag.PassiveProductCount = passiveProductCount;
            ViewBag.TotalMovements = totalMovements;

            ViewBag.LatestProducts = latestProducts;
            ViewBag.LatestMovements = latestMovements;

            ViewBag.CategoryLabels = categoryData.Select(x => x.CategoryName).ToList();
            ViewBag.CategoryCounts = categoryData.Select(x => x.ProductCount).ToList();

            ViewBag.MovementLabels = movementSummary
                .Select(x => x.MovementType == "In" ? "Giriş" : "Çıkış")
                .ToList();

            ViewBag.MovementCounts = movementSummary
                .Select(x => x.Count)
                .ToList();

            return View();
        }
    }
}