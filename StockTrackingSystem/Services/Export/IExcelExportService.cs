using StockTrackingSystem.Models;

namespace StockTrackingSystem.Services.Export
{
    // Defines Excel export operations for product and stock movement reports.
    public interface IExcelExportService
    {
        // Exports the product list to an Excel file.
        byte[] ExportProductsToExcel(List<Product> products);

        // Exports stock movement records to an Excel file.
        byte[] ExportStockMovementsToExcel(List<StockMovement> movements);
    }
}