using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı boş bırakılamaz.")]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ürün kodu boş bırakılamaz.")]
        [StringLength(50)]
        public string ProductCode { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Kategori seçilmelidir.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required(ErrorMessage = "Tedarikçi seçilmelidir.")]
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        [Range(0, 999999999)]
        public decimal UnitPrice { get; set; }

        [Range(0, 999999)]
        public int StockQuantity { get; set; }

        [Range(0, 999999)]
        public int CriticalStockLevel { get; set; } = 5;

        [Required(ErrorMessage = "Birim türü boş bırakılamaz.")]
        [StringLength(30)]
        public string UnitType { get; set; } = "Adet";

        [StringLength(250)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        public ICollection<StockMovement>? StockMovements { get; set; }
    }
}