using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Data;
using StockTrackingSystem.Helpers;
using StockTrackingSystem.Models;

namespace StockTrackingSystem.Controllers
{
    public class StockMovementController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor
        public StockMovementController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // INDEX
        // =========================

        // Display stock movements with filters
        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            int? productId,
            string? movementType,
            DateTime? startDate,
            DateTime? endDate)
        {
            var query = _context.StockMovements
                .Include(x => x.Product)
                .ThenInclude(x => x!.Category)
                .AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchText = search.Trim();

                query = query.Where(x =>
                    (x.Product != null && x.Product.Name.Contains(searchText)) ||
                    (x.ReferenceCode != null && x.ReferenceCode.Contains(searchText)) ||
                    (x.Description != null && x.Description.Contains(searchText)));
            }

            // Product filter
            if (productId.HasValue && productId.Value > 0)
            {
                query = query.Where(x => x.ProductId == productId.Value);
            }

            // Movement type filter
            if (!string.IsNullOrWhiteSpace(movementType))
            {
                query = query.Where(x => x.MovementType == movementType);
            }

            // Start date filter
            if (startDate.HasValue)
            {
                query = query.Where(x => x.MovementDate >= startDate.Value.Date);
            }

            // End date filter
            if (endDate.HasValue)
            {
                var end = endDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.MovementDate <= end);
            }

            var movements = await query
                .OrderByDescending(x => x.MovementDate)
                .ThenByDescending(x => x.Id)
                .ToListAsync();

            // UI state
            ViewBag.Search = search;
            ViewBag.ProductId = productId;
            ViewBag.MovementType = movementType;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            // Product dropdown
            ViewBag.Products = await _context.Products
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            return View(movements);
        }

        // =========================
        // CREATE
        // =========================

        // Show create form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadProductsAsync();

            return View(new StockMovement
            {
                MovementType = "In",
                MovementDate = DateTime.Now,
                CreatedDate = DateTime.Now
            });
        }

        // Handle create post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockMovement movement)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == movement.ProductId);

            if (product == null)
            {
                ModelState.AddModelError("ProductId", "Seçilen ürün bulunamadı.");
            }

            if (movement.MovementType != "In" && movement.MovementType != "Out")
            {
                ModelState.AddModelError("MovementType", "Geçersiz hareket tipi.");
            }

            // Stock validation for OUT operation
            if (product != null && movement.MovementType == "Out" && product.StockQuantity < movement.Quantity)
            {
                ModelState.AddModelError("Quantity", "Çıkış işlemi için yeterli stok yok.");
            }

            if (!ModelState.IsValid)
            {
                await LoadProductsAsync();
                return View(movement);
            }

            // Apply movement to product
            ApplyMovementToProduct(product!, movement);

            product!.UpdatedDate = DateTime.Now;

            // Update unit price if provided
            if (movement.UnitPrice.HasValue && movement.UnitPrice.Value > 0)
            {
                product.UnitPrice = movement.UnitPrice.Value;
            }

            movement.CreatedDate = DateTime.Now;

            // Audit log
            AuditLogHelper.AddLog(
                _context,
                movement.MovementType == "In" ? "StockIn" : "StockOut",
                "StockMovement",
                movement.Id,
                $"{product.Name} için {movement.Quantity} adet {(movement.MovementType == "In" ? "giriş" : "çıkış")} hareketi oluşturuldu."
            );

            _context.StockMovements.Add(movement);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Stok hareketi başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DETAILS
        // =========================

        // Show movement details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var movement = await _context.StockMovements
                .Include(x => x.Product)
                .ThenInclude(x => x!.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movement == null)
                return NotFound();

            return View(movement);
        }

        // =========================
        // EDIT
        // =========================

        // Show edit form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movement = await _context.StockMovements
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movement == null)
                return NotFound();

            await LoadProductsAsync();
            return View(movement);
        }

        // Handle edit post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StockMovement movement)
        {
            var existingMovement = await _context.StockMovements
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == movement.Id);

            if (existingMovement == null)
                return NotFound();

            var oldProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == existingMovement.ProductId);
            var newProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == movement.ProductId);

            if (newProduct == null)
            {
                ModelState.AddModelError("ProductId", "Seçilen ürün bulunamadı.");
            }

            if (movement.MovementType != "In" && movement.MovementType != "Out")
            {
                ModelState.AddModelError("MovementType", "Geçersiz hareket tipi.");
            }

            if (oldProduct == null)
            {
                ModelState.AddModelError("", "Eski ürün kaydı bulunamadı.");
            }

            if (!ModelState.IsValid)
            {
                await LoadProductsAsync();
                return View(movement);
            }

            // Revert old movement
            RevertMovementFromProduct(oldProduct!, existingMovement);

            // Validate stock before applying new movement
            if (movement.MovementType == "Out" && newProduct!.StockQuantity < movement.Quantity)
            {
                ApplyMovementToProduct(oldProduct!, existingMovement);

                ModelState.AddModelError("Quantity", "Düzenleme sonrası çıkış işlemi için yeterli stok yok.");
                await LoadProductsAsync();
                return View(movement);
            }

            // Apply new movement
            ApplyMovementToProduct(newProduct!, movement);

            if (movement.UnitPrice.HasValue && movement.UnitPrice.Value > 0)
            {
                newProduct.UnitPrice = movement.UnitPrice.Value;
            }

            oldProduct!.UpdatedDate = DateTime.Now;
            newProduct!.UpdatedDate = DateTime.Now;
            movement.CreatedDate = existingMovement.CreatedDate;

            // Audit log
            AuditLogHelper.AddLog(
                _context,
                "Update",
                "StockMovement",
                movement.Id,
                $"{newProduct.Name} için stok hareketi güncellendi."
            );

            _context.StockMovements.Update(movement);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Stok hareketi başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE
        // =========================

        // Delete stock movement
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var movement = await _context.StockMovements
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movement == null)
            {
                return Json(new { success = false, message = "Hareket kaydı bulunamadı." });
            }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == movement.ProductId);

            if (product == null)
            {
                return Json(new { success = false, message = "Bağlı ürün bulunamadı." });
            }

            // Revert movement effect before delete
            RevertMovementFromProduct(product, movement);

            if (product.StockQuantity < 0)
            {
                ApplyMovementToProduct(product, movement);
                return Json(new { success = false, message = "Bu hareket silinirse stok geçersiz hale geliyor." });
            }

            product.UpdatedDate = DateTime.Now;

            _context.StockMovements.Remove(movement);

            // Audit log
            AuditLogHelper.AddLog(
                _context,
                "Delete",
                "StockMovement",
                movement.Id,
                $"{product.Name} için {movement.Quantity} adet {(movement.MovementType == "In" ? "giriş" : "çıkış")} hareketi silindi."
            );

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Stok hareketi başarıyla silindi." });
        }

        // =========================
        // HELPERS
        // =========================

        // Load product dropdown data
        private async Task LoadProductsAsync()
        {
            ViewBag.Products = await _context.Products
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();
        }

        // Apply stock movement to product
        private static void ApplyMovementToProduct(Product product, StockMovement movement)
        {
            if (movement.MovementType == "In")
            {
                product.StockQuantity += movement.Quantity;
            }
            else
            {
                product.StockQuantity -= movement.Quantity;
            }
        }

        // Revert stock movement from product
        private static void RevertMovementFromProduct(Product product, StockMovement movement)
        {
            if (movement.MovementType == "In")
            {
                product.StockQuantity -= movement.Quantity;
            }
            else
            {
                product.StockQuantity += movement.Quantity;
            }
        }

        // =========================
        // API
        // =========================

        // Get product stock info (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetProductStockInfo(int productId)
        {
            var product = await _context.Products
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Ürün bulunamadı."
                });
            }

            return Json(new
            {
                success = true,
                productId = product.Id,
                name = product.Name,
                currentStock = product.StockQuantity,
                criticalStock = product.CriticalStockLevel,
                unitType = product.UnitType,
                categoryName = product.Category != null ? product.Category.Name : "-",
                unitPrice = product.UnitPrice
            });
        }
    }
}