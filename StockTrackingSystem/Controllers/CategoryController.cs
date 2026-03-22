using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Data;
using StockTrackingSystem.Models;

namespace StockTrackingSystem.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            string? statusFilter,
            DateTime? startDate,
            DateTime? endDate,
            string? sortOrder)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchText = search.Trim();

                query = query.Where(x =>
                    x.Name.Contains(searchText) ||
                    (x.Description != null && x.Description.Contains(searchText)));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                if (statusFilter == "active")
                    query = query.Where(x => x.IsActive);
                else if (statusFilter == "passive")
                    query = query.Where(x => !x.IsActive);
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
                "date_asc" => query.OrderBy(x => x.CreatedDate),
                _ => query.OrderByDescending(x => x.CreatedDate)
            };

            var categories = await query
                .Select(x => new Category
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SortOrder = sortOrder;

            ViewBag.TotalCount = await _context.Categories.CountAsync();
            ViewBag.ActiveCount = await _context.Categories.CountAsync(x => x.IsActive);
            ViewBag.PassiveCount = await _context.Categories.CountAsync(x => !x.IsActive);
            ViewBag.FilteredCount = categories.Count;

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            category.CreatedDate = DateTime.Now;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kategori başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.Categories
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            var existingCategory = await _context.Categories.FindAsync(category.Id);

            if (existingCategory == null)
                return NotFound();

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
            existingCategory.IsActive = category.IsActive;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Kategori bulunamadı."
                });
            }

            if (category.Products != null && category.Products.Any())
            {
                return Json(new
                {
                    success = false,
                    message = "Bu kategoriye bağlı ürünler olduğu için silinemez."
                });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            var totalCount = await _context.Categories.CountAsync();
            var activeCount = await _context.Categories.CountAsync(x => x.IsActive);
            var passiveCount = await _context.Categories.CountAsync(x => !x.IsActive);

            return Json(new
            {
                success = true,
                message = "Kategori başarıyla silindi.",
                totalCount,
                activeCount,
                passiveCount
            });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Kategori bulunamadı."
                });
            }

            category.IsActive = !category.IsActive;
            await _context.SaveChangesAsync();

            var totalCount = await _context.Categories.CountAsync();
            var activeCount = await _context.Categories.CountAsync(x => x.IsActive);
            var passiveCount = await _context.Categories.CountAsync(x => !x.IsActive);

            return Json(new
            {
                success = true,
                isActive = category.IsActive,
                totalCount,
                activeCount,
                passiveCount,
                message = category.IsActive ? "Kategori aktif yapıldı." : "Kategori pasif yapıldı."
            });
        }
    }
}