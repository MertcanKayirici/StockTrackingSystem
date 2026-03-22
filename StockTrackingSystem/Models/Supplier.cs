using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ContactName { get; set; }

        [StringLength(30)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}