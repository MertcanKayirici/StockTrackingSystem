using ClosedXML.Excel;
using StockTrackingSystem.Models;

namespace StockTrackingSystem.Services.Export
{
    // Handles Excel file generation for product and stock movement reports.
    public class ExcelExportService : IExcelExportService
    {
        // Exports the product list to a formatted Excel file.
        public byte[] ExportProductsToExcel(List<Product> products)
        {
            // Create a new Excel workbook and worksheet
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Products");

            // Define header row
            worksheet.Cell(1, 1).Value = "Product Name";
            worksheet.Cell(1, 2).Value = "Category";
            worksheet.Cell(1, 3).Value = "Supplier";
            worksheet.Cell(1, 4).Value = "Unit Price";
            worksheet.Cell(1, 5).Value = "Stock Quantity";
            worksheet.Cell(1, 6).Value = "Critical Stock Level";
            worksheet.Cell(1, 7).Value = "Status";
            worksheet.Cell(1, 8).Value = "Created Date";

            // Apply styling to header row
            var headerRange = worksheet.Range(1, 1, 1, 8);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;

            // Populate worksheet with product data
            foreach (var product in products)
            {
                worksheet.Cell(row, 1).Value = product.Name;
                worksheet.Cell(row, 2).Value = product.Category?.Name ?? "-";
                worksheet.Cell(row, 3).Value = product.Supplier?.CompanyName ?? "-";
                worksheet.Cell(row, 4).Value = product.UnitPrice;
                worksheet.Cell(row, 5).Value = product.StockQuantity;
                worksheet.Cell(row, 6).Value = product.CriticalStockLevel;
                worksheet.Cell(row, 7).Value = product.IsActive ? "Active" : "Passive";
                worksheet.Cell(row, 8).Value = product.CreatedDate;

                row++;
            }

            // Apply formatting to columns
            worksheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
            worksheet.Column(8).Style.DateFormat.Format = "dd.MM.yyyy HH:mm";

            // Auto-fit column widths
            worksheet.Columns().AdjustToContents();

            // Save workbook to memory stream and return as byte array
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        // Exports stock movement records to a formatted Excel file.
        public byte[] ExportStockMovementsToExcel(List<StockMovement> movements)
        {
            // Create a new Excel workbook and worksheet
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Stock Movements");

            // Define header row
            worksheet.Cell(1, 1).Value = "Product";
            worksheet.Cell(1, 2).Value = "Movement Type";
            worksheet.Cell(1, 3).Value = "Quantity";
            worksheet.Cell(1, 4).Value = "Description";
            worksheet.Cell(1, 5).Value = "Created Date";

            // Apply styling to header row
            var headerRange = worksheet.Range(1, 1, 1, 5);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;

            // Populate worksheet with stock movement data
            foreach (var movement in movements)
            {
                worksheet.Cell(row, 1).Value = movement.Product?.Name ?? "-";
                worksheet.Cell(row, 2).Value = movement.MovementType.ToString();
                worksheet.Cell(row, 3).Value = movement.Quantity;
                worksheet.Cell(row, 4).Value = movement.Description ?? "-";
                worksheet.Cell(row, 5).Value = movement.CreatedDate;

                row++;
            }

            // Apply date format to column
            worksheet.Column(5).Style.DateFormat.Format = "dd.MM.yyyy HH:mm";

            // Auto-fit column widths
            worksheet.Columns().AdjustToContents();

            // Save workbook to memory stream and return as byte array
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }
    }
}