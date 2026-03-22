using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Data;
using StockTrackingSystem.Models;

namespace StockTrackingSystem.Controllers
{
    public class SupplierController : Controller
    {
        private readonly AppDbContext _context;

        public SupplierController(AppDbContext context)
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
            var query = _context.Suppliers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchText = search.Trim();

                query = query.Where(x =>
                    x.CompanyName.Contains(searchText) ||
                    (x.ContactName != null && x.ContactName.Contains(searchText)) ||
                    (x.Email != null && x.Email.Contains(searchText)) ||
                    (x.Phone != null && x.Phone.Contains(searchText)));
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
                "name_asc" => query.OrderBy(x => x.CompanyName),
                "name_desc" => query.OrderByDescending(x => x.CompanyName),
                "date_asc" => query.OrderBy(x => x.CreatedDate),
                _ => query.OrderByDescending(x => x.CreatedDate)
            };

            var suppliers = await query.ToListAsync();

            ViewBag.Search = search;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SortOrder = sortOrder;

            ViewBag.TotalCount = await _context.Suppliers.CountAsync();
            ViewBag.ActiveCount = await _context.Suppliers.CountAsync(x => x.IsActive);
            ViewBag.PassiveCount = await _context.Suppliers.CountAsync(x => !x.IsActive);
            ViewBag.FilteredCount = suppliers.Count;

            return View(suppliers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Supplier
            {
                IsActive = true
            });
        }

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
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Tedarikçi başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

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

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Tedarikçi başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

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

            if (supplier.Products != null && supplier.Products.Any())
            {
                return Json(new
                {
                    success = false,
                    message = "Bu tedarikçiye bağlı ürünler olduğu için silinemez."
                });
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Tedarikçi başarıyla silindi." });
        }
    }
}