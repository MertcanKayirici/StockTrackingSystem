using StockTrackingSystem.Models;

namespace StockTrackingSystem.Services.Export
{
    // Defines PDF export operations for product and stock movement reports.
    public interface IPdfExportService
    {
        // Generates a PDF report for the product list.
        byte[] ExportProductsToPdf(List<Product> products);

        // Generates a PDF report for stock movement records.
        byte[] ExportStockMovementsToPdf(List<StockMovement> movements);
    }
}