using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models
{
    public class Product
    {
        // Primary key
        public int Id { get; set; }

        // Product name
        [Required(ErrorMessage = "Ürün adı boş bırakılamaz.")]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        // Unique product code
        [Required(ErrorMessage = "Ürün kodu boş bırakılamaz.")]
        [StringLength(50)]
        public string ProductCode { get; set; } = string.Empty;

        // Optional description
        [StringLength(500)]
        public string? Description { get; set; }

        // Foreign key - Category
        [Required(ErrorMessage = "Kategori seçilmelidir.")]
        public int CategoryId { get; set; }

        // Navigation property - Category
        public Category? Category { get; set; }

        // Foreign key - Supplier
        [Required(ErrorMessage = "Tedarikçi seçilmelidir.")]
        public int SupplierId { get; set; }

        // Navigation property - Supplier
        public Supplier? Supplier { get; set; }

        // Product unit price
        [Range(0, 999999999)]
        public decimal UnitPrice { get; set; }

        // Current stock quantity
        [Range(0, 999999)]
        public int StockQuantity { get; set; }

        // Critical stock threshold
        [Range(0, 999999)]
        public int CriticalStockLevel { get; set; } = 5;

        // Unit type (e.g. piece, kg, etc.)
        [Required(ErrorMessage = "Birim türü boş bırakılamaz.")]
        [StringLength(30)]
        public string UnitType { get; set; } = "Adet";

        // Product image path
        [StringLength(250)]
        public string? ImageUrl { get; set; }

        // Active/passive status
        public bool IsActive { get; set; } = true;

        // Creation date
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Last update date
        public DateTime? UpdatedDate { get; set; }

        // Navigation property - Stock movements
        public ICollection<StockMovement>? StockMovements { get; set; }
    }
}