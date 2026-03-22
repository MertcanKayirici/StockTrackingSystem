using System;

namespace StockTrackingSystem.Models
{
    public class StockMovement
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string MovementType { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public DateTime MovementDate { get; set; }
    }
}