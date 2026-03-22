using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Data;
using StockTrackingSystem.Models;
using StockTrackingSystem.Helpers;
using StockTrackingSystem.Services.Export;

namespace StockTrackingSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IExcelExportService _excelExportService;
        private readonly IPdfExportService _pdfExportService;

        // Constructor
        public ProductController(
            AppDbContext context,
            IWebHostEnvironment environment,
            IExcelExportService excelExportService,
            IPdfExportService pdfExportService)
        {
            _context = context;
            _environment = environment;
            _excelExportService = excelExportService;
            _pdfExportService = pdfExportService;
        }

        // =========================
        // INDEX
        // =========================

        // Display product list with filters, search and sorting
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
                .Include(x => x.Supplier)
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchText = search.Trim();
                query = query.Where(x =>
                    x.Name.Contains(searchText) ||
                    x.ProductCode.Contains(searchText) ||
                    (x.Description != null && x.Description.Contains(searchText)));
            }

            // Apply category filter
            if (categoryId.HasValue && categoryId.Value > 0)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                if (statusFilter == "active")
                    query = query.Where(x => x.IsActive);
                else if (statusFilter == "passive")
                    query = query.Where(x => !x.IsActive);
            }

            // Apply stock filter
            if (!string.IsNullOrWhiteSpace(stockFilter))
            {
                if (stockFilter == "critical")
                    query = query.Where(x => x.StockQuantity <= x.CriticalStockLevel && x.StockQuantity > 0);
                else if (stockFilter == "normal")
                    query = query.Where(x => x.StockQuantity > x.CriticalStockLevel);
                else if (stockFilter == "out")
                    query = query.Where(x => x.StockQuantity == 0);
            }

            // Apply start date filter
            if (startDate.HasValue)
                query = query.Where(x => x.CreatedDate >= startDate.Value.Date);

            // Apply end date filter
            if (endDate.HasValue)
            {
                var end = endDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.CreatedDate <= end);
            }

            // Apply sorting
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

            // Store current filter values for UI
            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.StockFilter = stockFilter;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SortOrder = sortOrder;

            // Store summary data
            ViewBag.TotalCount = await _context.Products.CountAsync();
            ViewBag.ActiveCount = await _context.Products.CountAsync(x => x.IsActive);
            ViewBag.PassiveCount = await _context.Products.CountAsync(x => !x.IsActive);
            ViewBag.CriticalCount = await _context.Products.CountAsync(x => x.StockQuantity <= x.CriticalStockLevel && x.StockQuantity > 0);
            ViewBag.FilteredCount = products.Count;

            // Load active categories for filter dropdown
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

        // =========================
        // CREATE
        // =========================

        // Show create form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View(new Product
            {
                IsActive = true,
                UnitType = "Adet",
                CriticalStockLevel = 5
            });
        }

        // Handle create post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
        {
            // Check duplicate product code
            if (await _context.Products.AnyAsync(x => x.ProductCode == product.ProductCode))
            {
                ModelState.AddModelError("ProductCode", "Bu ürün kodu zaten kullanılıyor.");
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return View(product);
            }

            // Save uploaded product image
            if (imageFile != null && imageFile.Length > 0)
            {
                product.ImageUrl = await SaveImageAsync(imageFile);
            }

            product.CreatedDate = DateTime.Now;
            product.UpdatedDate = null;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Add audit log after creation
            AuditLogHelper.AddLog(
                _context,
                "Create",
                "Product",
                product.Id,
                $"{product.Name} adlı ürün eklendi."
            );

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ürün başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DETAILS
        // =========================

        // Show product details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.Supplier)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // =========================
        // EDIT
        // =========================

        // Show edit form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            await LoadDropdownsAsync();
            return View(product);
        }

        // Handle edit post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile? imageFile, bool removeImage = false)
        {
            var existingProduct = await _context.Products.FindAsync(product.Id);

            if (existingProduct == null)
                return NotFound();

            // Check duplicate product code except current record
            if (await _context.Products.AnyAsync(x => x.ProductCode == product.ProductCode && x.Id != product.Id))
            {
                ModelState.AddModelError("ProductCode", "Bu ürün kodu zaten kullanılıyor.");
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                product.ImageUrl = existingProduct.ImageUrl;
                return View(product);
            }

            // Update editable fields
            existingProduct.Name = product.Name;
            existingProduct.ProductCode = product.ProductCode;
            existingProduct.Description = product.Description;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.SupplierId = product.SupplierId;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.CriticalStockLevel = product.CriticalStockLevel;
            existingProduct.UnitType = product.UnitType;
            existingProduct.IsActive = product.IsActive;
            existingProduct.UpdatedDate = DateTime.Now;

            // Remove existing image if requested
            if (removeImage && !string.IsNullOrWhiteSpace(existingProduct.ImageUrl))
            {
                DeleteImageFile(existingProduct.ImageUrl);
                existingProduct.ImageUrl = null;
            }

            // Replace existing image with new uploaded file
            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(existingProduct.ImageUrl))
                    DeleteImageFile(existingProduct.ImageUrl);

                existingProduct.ImageUrl = await SaveImageAsync(imageFile);
            }

            // Add audit log after update
            AuditLogHelper.AddLog(
                _context,
                "Update",
                "Product",
                existingProduct.Id,
                $"{existingProduct.Name} adlı ürün güncellendi."
            );

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ürün başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));

        }

        // =========================
        // DELETE
        // =========================

        // Delete product and related stock movements
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(x => x.StockMovements)
                .FirstOrDefaultAsync(x => x.Id == id);

            var productName = product.Name;
            var productId = product.Id;

            if (product == null)
            {
                return Json(new { success = false, message = "Ürün bulunamadı." });
            }

            // Delete product image file if exists
            if (!string.IsNullOrWhiteSpace(product.ImageUrl))
            {
                DeleteImageFile(product.ImageUrl);
            }

            // Add audit log before deletion
            AuditLogHelper.AddLog(
                _context,
                "Delete",
                "Product",
                productId,
                $"{productName} adlı ürün silindi."
            );

            _context.StockMovements.RemoveRange(product.StockMovements ?? Enumerable.Empty<StockMovement>());
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Ürün başarıyla silindi." });
        }

        // =========================
        // STATUS TOGGLE
        // =========================

        // Toggle product active/passive status
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return Json(new { success = false, message = "Ürün bulunamadı." });
            }

            product.IsActive = !product.IsActive;
            product.UpdatedDate = DateTime.Now;

            // Add audit log for status change
            AuditLogHelper.AddLog(
                _context,
                "StatusChange",
                "Product",
                product.Id,
                $"{product.Name} adlı ürün {(product.IsActive ? "aktif" : "pasif")} yapıldı."
            );

            await _context.SaveChangesAsync();

            var totalCount = await _context.Products.CountAsync();
            var activeCount = await _context.Products.CountAsync(x => x.IsActive);
            var passiveCount = await _context.Products.CountAsync(x => !x.IsActive);
            var criticalCount = await _context.Products.CountAsync(x => x.StockQuantity <= x.CriticalStockLevel && x.StockQuantity > 0);

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

        // =========================
        // EXPORT
        // =========================

        [HttpGet]
        public async Task<IActionResult> Export(
            string format,
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
                .Include(x => x.Supplier)
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchText = search.Trim();
                query = query.Where(x =>
                    x.Name.Contains(searchText) ||
                    x.ProductCode.Contains(searchText) ||
                    (x.Description != null && x.Description.Contains(searchText)));
            }

            // Apply category filter
            if (categoryId.HasValue && categoryId.Value > 0)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                if (statusFilter == "active")
                    query = query.Where(x => x.IsActive);
                else if (statusFilter == "passive")
                    query = query.Where(x => !x.IsActive);
            }

            // Apply stock filter
            if (!string.IsNullOrWhiteSpace(stockFilter))
            {
                if (stockFilter == "critical")
                    query = query.Where(x => x.StockQuantity <= x.CriticalStockLevel && x.StockQuantity > 0);
                else if (stockFilter == "normal")
                    query = query.Where(x => x.StockQuantity > x.CriticalStockLevel);
                else if (stockFilter == "out")
                    query = query.Where(x => x.StockQuantity == 0);
            }

            // Apply start date filter
            if (startDate.HasValue)
                query = query.Where(x => x.CreatedDate >= startDate.Value.Date);

            // Apply end date filter
            if (endDate.HasValue)
            {
                var end = endDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.CreatedDate <= end);
            }

            // Apply sorting
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

            var products = await query.AsNoTracking().ToListAsync();

            if (string.Equals(format, "excel", StringComparison.OrdinalIgnoreCase))
            {
                var content = _excelExportService.ExportProductsToExcel(products);

                AuditLogHelper.AddLog(
                    _context,
                    "Export",
                    "Product",
                    0,
                    $"Ürün listesi Excel olarak dışa aktarıldı. Kayıt sayısı: {products.Count}"
                );

                await _context.SaveChangesAsync();

                return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"products-report-{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }

            if (string.Equals(format, "pdf", StringComparison.OrdinalIgnoreCase))
            {
                var content = _pdfExportService.ExportProductsToPdf(products);

                AuditLogHelper.AddLog(
                    _context,
                    "Export",
                    "Product",
                    0,
                    $"Ürün listesi PDF olarak dışa aktarıldı. Kayıt sayısı: {products.Count}"
                );

                await _context.SaveChangesAsync();

                return File(
                    content,
                    "application/pdf",
                    $"products-report-{DateTime.Now:yyyyMMddHHmmss}.pdf");
            }

            return BadRequest("Geçersiz export formatı.");
        }

        // =========================
        // HELPERS
        // =========================

        // Load category and supplier dropdown data
        private async Task LoadDropdownsAsync()
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

            ViewBag.Suppliers = await _context.Suppliers
                .Where(x => x.IsActive)
                .OrderBy(x => x.CompanyName)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.CompanyName
                })
                .ToListAsync();
        }

        // Save uploaded image file and return relative path
        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "img", "products");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Sadece jpg, jpeg, png ve webp uzantılı dosyalar yüklenebilir.");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return $"/img/products/{fileName}";
        }

        // Delete image file from physical storage
        private void DeleteImageFile(string imageUrl)
        {
            var cleanPath = imageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_environment.WebRootPath, cleanPath);

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }
    }
}