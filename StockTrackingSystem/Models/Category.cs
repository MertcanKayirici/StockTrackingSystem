using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models
{
    public class Category
    {
        // Primary key
        public int Id { get; set; }

        // Category name
        [Required(ErrorMessage = "Kategori adı boş bırakılamaz.")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; }

        // Optional description
        [StringLength(250, ErrorMessage = "Açıklama en fazla 250 karakter olabilir.")]
        public string? Description { get; set; }

        // Active/passive status
        public bool IsActive { get; set; } = true;

        // Creation date
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property for related products
        public ICollection<Product>? Products { get; set; }
    }
}