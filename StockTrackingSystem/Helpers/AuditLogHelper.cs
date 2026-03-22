using StockTrackingSystem.Data;
using StockTrackingSystem.Models;

namespace StockTrackingSystem.Helpers
{
    public static class AuditLogHelper
    {
        public static void AddLog(
            AppDbContext context,
            string actionType,
            string entityName,
            int? entityId,
            string description)
        {
            var log = new AuditLog
            {
                ActionType = actionType,
                EntityName = entityName,
                EntityId = entityId,
                Description = description,
                CreatedDate = DateTime.Now
            };

            context.AuditLogs.Add(log);
        }
    }
}