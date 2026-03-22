using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models
{
    public class AuditLog
    {
        // Primary key
        public int Id { get; set; }

        // Type of action (Create, Update, Delete, etc.)
        [Required]
        [StringLength(50)]
        public string ActionType { get; set; } = string.Empty;

        // Related entity name (Category, Product, etc.)
        [Required]
        [StringLength(50)]
        public string EntityName { get; set; } = string.Empty;

        // Related entity ID (optional)
        public int? EntityId { get; set; }

        // Description of the action
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        // Timestamp of log creation
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}