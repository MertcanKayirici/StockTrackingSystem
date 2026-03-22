using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models
{
    public class StockMovement
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün seçilmelidir.")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required(ErrorMessage = "Hareket tipi seçilmelidir.")]
        [StringLength(20)]
        public string MovementType { get; set; } = "In"; // In / Out

        [Range(1, 999999, ErrorMessage = "Miktar 1 veya daha büyük olmalıdır.")]
        public int Quantity { get; set; }

        [Range(0, 999999999)]
        public decimal? UnitPrice { get; set; }

        [StringLength(50)]
        public string? ReferenceCode { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime MovementDate { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}