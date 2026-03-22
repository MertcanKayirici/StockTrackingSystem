using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Data;
using StockTrackingSystem.Models;

namespace StockTrackingSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            int? categoryId,
            string? statusFilter,
            string? stockFilter,
            DateTime? startDate,
            DateTime? endDate,
            string? sortOrder)
        {
            var query = _context.Products
                .Include(x => x.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchText = search.Trim();

                query = query.Where(x =>
                    x.Name.Contains(searchText) ||
                    (x.Description != null && x.Description.Contains(searchText)));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                if (statusFilter == "active")
                    query = query.Where(x => x.IsActive);
                else if (statusFilter == "passive")
                    query = query.Where(x => !x.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(stockFilter))
            {
                if (stockFilter == "critical")
                    query = query.Where(x => x.StockQuantity <= x.CriticalStockLevel);
                else if (stockFilter == "normal")
                    query = query.Where(x => x.StockQuantity > x.CriticalStockLevel);
                else if (stockFilter == "out")
                    query = query.Where(x => x.StockQuantity == 0);
            }

            if (startDate.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                var end = endDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.CreatedDate <= end);
            }

            query = sortOrder switch
            {
                "name_asc" => query.OrderBy(x => x.Name),
                "name_desc" => query.OrderByDescending(x => x.Name),
                "price_asc" => query.OrderBy(x => x.UnitPrice),
                "price_desc" => query.OrderByDescending(x => x.UnitPrice),
                "stock_asc" => query.OrderBy(x => x.StockQuantity),
                "stock_desc" => query.OrderByDescending(x => x.StockQuantity),
                "date_asc" => query.OrderBy(x => x.CreatedDate),
                _ => query.OrderByDescending(x => x.CreatedDate)
            };

            var products = await query.ToListAsync();

            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.StockFilter = stockFilter;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SortOrder = sortOrder;

            ViewBag.TotalCount = await _context.Products.CountAsync();
            ViewBag.ActiveCount = await _context.Products.CountAsync(x => x.IsActive);
            ViewBag.PassiveCount = await _context.Products.CountAsync(x => !x.IsActive);
            ViewBag.CriticalCount = await _context.Products.CountAsync(x => x.StockQuantity <= x.CriticalStockLevel);
            ViewBag.FilteredCount = products.Count;

            ViewBag.Categories = await _context.Categories
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.Name)
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    })
                    .ToListAsync();

                return View(product);
            }

            product.CreatedDate = DateTime.Now;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ürün başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return Json(new { success = false, message = "Ürün bulunamadı." });
            }

            product.IsActive = !product.IsActive;
            await _context.SaveChangesAsync();

            var totalCount = await _context.Products.CountAsync();
            var activeCount = await _context.Products.CountAsync(x => x.IsActive);
            var passiveCount = await _context.Products.CountAsync(x => !x.IsActive);
            var criticalCount = await _context.Products.CountAsync(x => x.StockQuantity <= x.CriticalStockLevel);

            return Json(new
            {
                success = true,
                isActive = product.IsActive,
                totalCount,
                activeCount,
                passiveCount,
                criticalCount
            });
        }
    }
}