using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Firma adı boş bırakılamaz.")]
        [StringLength(150, ErrorMessage = "Firma adı en fazla 150 karakter olabilir.")]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Yetkili kişi en fazla 100 karakter olabilir.")]
        public string? ContactName { get; set; }

        [StringLength(30, ErrorMessage = "Telefon en fazla 30 karakter olabilir.")]
        public string? Phone { get; set; }

        [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string? Email { get; set; }

        [StringLength(250, ErrorMessage = "Adres en fazla 250 karakter olabilir.")]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}