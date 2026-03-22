using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ActionType { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string EntityName { get; set; } = string.Empty;

        public int? EntityId { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}