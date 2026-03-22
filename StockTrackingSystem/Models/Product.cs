using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı boş bırakılamaz.")]
        [StringLength(150, ErrorMessage = "Ürün adı en fazla 150 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Kategori seçilmelidir.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Range(0, 999999999)]
        public decimal UnitPrice { get; set; }

        [Range(0, 999999)]
        public int StockQuantity { get; set; }

        [Range(0, 999999)]
        public int CriticalStockLevel { get; set; } = 5;

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<StockMovement>? StockMovements { get; set; }
    }
}