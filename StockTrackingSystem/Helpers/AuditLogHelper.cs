using StockTrackingSystem.Data;
using StockTrackingSystem.Models;

namespace StockTrackingSystem.Helpers
{
    public static class AuditLogHelper
    {
        // =========================
        // ADD LOG
        // =========================

        // Adds a new audit log entry to the database context
        public static void AddLog(
            AppDbContext context,
            string actionType,
            string entityName,
            int? entityId,
            string description)
        {
            // Create new audit log object
            var log = new AuditLog
            {
                ActionType = actionType,
                EntityName = entityName,
                EntityId = entityId,
                Description = description,
                CreatedDate = DateTime.Now
            };

            // Add log to context (will be saved with SaveChanges)
            context.AuditLogs.Add(log);
        }
    }
}