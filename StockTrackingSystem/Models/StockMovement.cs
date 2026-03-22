using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models
{
    public class StockMovement
    {
        // Primary key
        public int Id { get; set; }

        // Foreign key - Product
        [Required(ErrorMessage = "Ürün seçilmelidir.")]
        public int ProductId { get; set; }

        // Navigation property - Product
        public Product? Product { get; set; }

        // Movement type (In / Out)
        [Required(ErrorMessage = "Hareket tipi seçilmelidir.")]
        [StringLength(20)]
        public string MovementType { get; set; } = "In"; // In / Out

        // Movement quantity
        [Range(1, 999999, ErrorMessage = "Miktar 1 veya daha büyük olmalıdır.")]
        public int Quantity { get; set; }

        // Unit price (optional)
        [Range(0, 999999999)]
        public decimal? UnitPrice { get; set; }

        // Reference code (optional)
        [StringLength(50)]
        public string? ReferenceCode { get; set; }

        // Description (optional)
        [StringLength(500)]
        public string? Description { get; set; }

        // Movement date
        public DateTime MovementDate { get; set; } = DateTime.Now;

        // Record creation date
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}