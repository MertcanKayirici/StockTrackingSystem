using System;
using System.Collections.Generic;

namespace StockTrackingSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public int CriticalStockLevel { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<StockMovement>? StockMovements { get; set; }
    }
}