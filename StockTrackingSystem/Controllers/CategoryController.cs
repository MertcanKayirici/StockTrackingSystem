using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Data;
using StockTrackingSystem.Helpers;
using StockTrackingSystem.Models;

namespace StockTrackingSystem.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // INDEX (LIST)
        // =========================

        // Display categories with filtering, search and sorting
        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            string? statusFilter,
            DateTime? startDate,
            DateTime? endDate,
            string? sortOrder)
        {
            var query = _context.Categories.AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchText = search.Trim();

                query = query.Where(x =>
                    x.Name.Contains(searchText) ||
                    (x.Description != null && x.Description.Contains(searchText)));
            }

            // Status filter
            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                if (statusFilter == "active")
                    query = query.Where(x => x.IsActive);
                else if (statusFilter == "passive")
                    query = query.Where(x => !x.IsActive);
            }

            // Start date filter
            if (startDate.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= startDate.Value.Date);
            }

            // End date filter
            if (endDate.HasValue)
            {
                var end = endDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.CreatedDate <= end);
            }

            // Sorting
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

            // UI state
            ViewBag.Search = search;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SortOrder = sortOrder;

            // Summary data
            ViewBag.TotalCount = await _context.Categories.CountAsync();
            ViewBag.ActiveCount = await _context.Categories.CountAsync(x => x.IsActive);
            ViewBag.PassiveCount = await _context.Categories.CountAsync(x => !x.IsActive);
            ViewBag.FilteredCount = categories.Count;

            return View(categories);
        }

        // =========================
        // CREATE
        // =========================

        // Show create form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Handle create post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            category.CreatedDate = DateTime.Now;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            AuditLogHelper.AddLog(
                _context,
                "Create",
                "Category",
                category.Id,
                $"{category.Name} kategorisi eklendi."
            );

            TempData["SuccessMessage"] = "Kategori başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DETAILS
        // =========================

        // Show category details
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

        // =========================
        // EDIT
        // =========================

        // Show edit form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // Handle edit post
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

            AuditLogHelper.AddLog(
                _context,
                "Update",
                "Category",
                existingCategory.Id,
                $"{existingCategory.Name} kategorisi güncellendi."
            );

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE
        // =========================

        // Delete category
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return Json(new { success = false, message = "Kategori bulunamadı." });
            }

            if (category.Products != null && category.Products.Any())
            {
                return Json(new { success = false, message = "Bu kategoriye bağlı ürünler olduğu için silinemez." });
            }

            var categoryName = category.Name;
            var categoryId = category.Id;

            _context.Categories.Remove(category);

            AuditLogHelper.AddLog(
                _context,
                "Delete",
                "Category",
                categoryId,
                $"{categoryName} kategorisi silindi."
            );

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

        // =========================
        // STATUS TOGGLE
        // =========================

        // Toggle active/passive status
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return Json(new { success = false, message = "Kategori bulunamadı." });
            }

            category.IsActive = !category.IsActive;

            AuditLogHelper.AddLog(
                _context,
                "StatusChange",
                "Category",
                category.Id,
                $"{category.Name} kategorisi {(category.IsActive ? "aktif" : "pasif")} yapıldı."
            );

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