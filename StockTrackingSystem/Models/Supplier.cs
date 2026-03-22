using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models
{
    public class Supplier
    {
        // Primary key
        public int Id { get; set; }

        // Company name
        [Required(ErrorMessage = "Firma adı boş bırakılamaz.")]
        [StringLength(150, ErrorMessage = "Firma adı en fazla 150 karakter olabilir.")]
        public string CompanyName { get; set; } = string.Empty;

        // Contact person name (optional)
        [StringLength(100, ErrorMessage = "Yetkili kişi en fazla 100 karakter olabilir.")]
        public string? ContactName { get; set; }

        // Phone number (optional)
        [StringLength(30, ErrorMessage = "Telefon en fazla 30 karakter olabilir.")]
        public string? Phone { get; set; }

        // Email address (optional)
        [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string? Email { get; set; }

        // Address (optional)
        [StringLength(250, ErrorMessage = "Adres en fazla 250 karakter olabilir.")]
        public string? Address { get; set; }

        // Active/passive status
        public bool IsActive { get; set; } = true;

        // Creation date
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Last update date
        public DateTime? UpdatedDate { get; set; }

        // Navigation property - Related products
        public ICollection<Product>? Products { get; set; }
    }
}