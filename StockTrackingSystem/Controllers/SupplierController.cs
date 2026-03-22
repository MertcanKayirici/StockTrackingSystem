using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Data;
using StockTrackingSystem.Helpers;
using StockTrackingSystem.Models;

namespace StockTrackingSystem.Controllers
{
    public class SupplierController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor
        public SupplierController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // INDEX
        // =========================

        // Display supplier list with filters and sorting
        [HttpGet]
        public async Task<IActionResult> Index(
            string? search,
            string? statusFilter,
            DateTime? startDate,
            DateTime? endDate,
            string? sortOrder)
        {
            var query = _context.Suppliers.AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchText = search.Trim();

                query = query.Where(x =>
                    x.CompanyName.Contains(searchText) ||
                    (x.ContactName != null && x.ContactName.Contains(searchText)) ||
                    (x.Email != null && x.Email.Contains(searchText)) ||
                    (x.Phone != null && x.Phone.Contains(searchText)));
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
                "name_asc" => query.OrderBy(x => x.CompanyName),
                "name_desc" => query.OrderByDescending(x => x.CompanyName),
                "date_asc" => query.OrderBy(x => x.CreatedDate),
                _ => query.OrderByDescending(x => x.CreatedDate)
            };

            var suppliers = await query.ToListAsync();

            // UI state
            ViewBag.Search = search;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SortOrder = sortOrder;

            // Summary data
            ViewBag.TotalCount = await _context.Suppliers.CountAsync();
            ViewBag.ActiveCount = await _context.Suppliers.CountAsync(x => x.IsActive);
            ViewBag.PassiveCount = await _context.Suppliers.CountAsync(x => !x.IsActive);
            ViewBag.FilteredCount = suppliers.Count;

            return View(suppliers);
        }

        // =========================
        // CREATE
        // =========================

        // Show create form
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Supplier
            {
                IsActive = true
            });
        }

        // Handle create post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return View(supplier);
            }

            supplier.CreatedDate = DateTime.Now;
            supplier.UpdatedDate = null;

            _context.Suppliers.Add(supplier);

            // Audit log
            AuditLogHelper.AddLog(
                _context,
                "Create",
                "Supplier",
                supplier.Id,
                $"{supplier.CompanyName} tedarikçisi eklendi."
            );

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Tedarikçi başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DETAILS
        // =========================

        // Show supplier details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _context.Suppliers
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // =========================
        // EDIT
        // =========================

        // Show edit form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // Handle edit post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return View(supplier);
            }

            var existingSupplier = await _context.Suppliers.FindAsync(supplier.Id);

            if (existingSupplier == null)
                return NotFound();

            existingSupplier.CompanyName = supplier.CompanyName;
            existingSupplier.ContactName = supplier.ContactName;
            existingSupplier.Phone = supplier.Phone;
            existingSupplier.Email = supplier.Email;
            existingSupplier.Address = supplier.Address;
            existingSupplier.IsActive = supplier.IsActive;
            existingSupplier.UpdatedDate = DateTime.Now;

            // Audit log
            AuditLogHelper.AddLog(
                _context,
                "Update",
                "Supplier",
                existingSupplier.Id,
                $"{existingSupplier.CompanyName} tedarikçisi güncellendi."
            );

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Tedarikçi başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE
        // =========================

        // Delete supplier
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _context.Suppliers
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (supplier == null)
            {
                return Json(new { success = false, message = "Tedarikçi bulunamadı." });
            }

            // Prevent delete if linked products exist
            if (supplier.Products != null && supplier.Products.Any())
            {
                return Json(new
                {
                    success = false,
                    message = "Bu tedarikçiye bağlı ürünler olduğu için silinemez."
                });
            }

            var supplierName = supplier.CompanyName;
            var supplierId = supplier.Id;

            _context.Suppliers.Remove(supplier);

            // Audit log
            AuditLogHelper.AddLog(
                _context,
                "Delete",
                "Supplier",
                supplierId,
                $"{supplierName} tedarikçisi silindi."
            );

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Tedarikçi başarıyla silindi." });
        }

        // =========================
        // STATUS TOGGLE
        // =========================

        // Toggle supplier active/passive status
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return Json(new { success = false, message = "Tedarikçi bulunamadı." });
            }

            supplier.IsActive = !supplier.IsActive;
            supplier.UpdatedDate = DateTime.Now;

            // Audit log
            AuditLogHelper.AddLog(
                _context,
                "StatusChange",
                "Supplier",
                supplier.Id,
                $"{supplier.CompanyName} tedarikçisi {(supplier.IsActive ? "aktif" : "pasif")} yapıldı."
            );

            await _context.SaveChangesAsync();

            var totalCount = await _context.Suppliers.CountAsync();
            var activeCount = await _context.Suppliers.CountAsync(x => x.IsActive);
            var passiveCount = await _context.Suppliers.CountAsync(x => !x.IsActive);

            return Json(new
            {
                success = true,
                isActive = supplier.IsActive,
                totalCount,
                activeCount,
                passiveCount
            });
        }
    }
}