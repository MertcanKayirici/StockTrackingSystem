using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StockTrackingSystem.Models;

namespace StockTrackingSystem.Services.Export
{
    // Handles PDF report generation for product and stock movement data.
    public class PdfExportService : IPdfExportService
    {
        // Generates a PDF report for the product list.
        public byte[] ExportProductsToPdf(List<Product> products)
        {
            // Set QuestPDF license type
            QuestPDF.Settings.License = LicenseType.Community;

            // Create PDF document structure
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Set page margin
                    page.Margin(30);

                    // Add report header
                    page.Header()
                        .Text("Product Report")
                        .SemiBold()
                        .FontSize(20);

                    // Add main content table
                    page.Content().PaddingVertical(15).Table(table =>
                    {
                        // Define table column widths
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        // Define table header row
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Product");
                            header.Cell().Element(CellStyle).Text("Category");
                            header.Cell().Element(CellStyle).Text("Supplier");
                            header.Cell().Element(CellStyle).Text("Price");
                            header.Cell().Element(CellStyle).Text("Stock");
                            header.Cell().Element(CellStyle).Text("Status");
                        });

                        // Populate table rows with product data
                        foreach (var product in products)
                        {
                            table.Cell().Element(DataStyle).Text(product.Name ?? "-");
                            table.Cell().Element(DataStyle).Text(product.Category?.Name ?? "-");
                            table.Cell().Element(DataStyle).Text(product.Supplier?.CompanyName ?? "-");
                            table.Cell().Element(DataStyle).Text($"{product.UnitPrice:N2} ₺");
                            table.Cell().Element(DataStyle).Text(product.StockQuantity.ToString());
                            table.Cell().Element(DataStyle).Text(product.IsActive ? "Active" : "Passive");
                        }
                    });

                    // Add footer with generation timestamp
                    page.Footer()
                        .AlignCenter()
                        .Text($"Generated on: {DateTime.Now:dd.MM.yyyy HH:mm}");
                });
            });

            // Generate and return PDF file as byte array
            return document.GeneratePdf();
        }

        // Generates a PDF report for stock movement records.
        public byte[] ExportStockMovementsToPdf(List<StockMovement> movements)
        {
            // Set QuestPDF license type
            QuestPDF.Settings.License = LicenseType.Community;

            // Create PDF document structure
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Set page margin
                    page.Margin(30);

                    // Add report header
                    page.Header()
                        .Text("Stock Movements Report")
                        .SemiBold()
                        .FontSize(20);

                    // Add main content table
                    page.Content().PaddingVertical(15).Table(table =>
                    {
                        // Define table column widths
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                        });

                        // Define table header row
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Product");
                            header.Cell().Element(CellStyle).Text("Type");
                            header.Cell().Element(CellStyle).Text("Qty");
                            header.Cell().Element(CellStyle).Text("Description");
                            header.Cell().Element(CellStyle).Text("Date");
                        });

                        // Populate table rows with stock movement data
                        foreach (var movement in movements)
                        {
                            table.Cell().Element(DataStyle).Text(movement.Product?.Name ?? "-");
                            table.Cell().Element(DataStyle).Text(movement.MovementType.ToString());
                            table.Cell().Element(DataStyle).Text(movement.Quantity.ToString());
                            table.Cell().Element(DataStyle).Text(movement.Description ?? "-");
                            table.Cell().Element(DataStyle).Text(movement.CreatedDate.ToString("dd.MM.yyyy HH:mm"));
                        }
                    });

                    // Add footer with generation timestamp
                    page.Footer()
                        .AlignCenter()
                        .Text($"Generated on: {DateTime.Now:dd.MM.yyyy HH:mm}");
                });
            });

            // Generate and return PDF file as byte array
            return document.GeneratePdf();
        }

        // Applies header cell styling for PDF tables.
        private static IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .Padding(5)
                .Background(Colors.Grey.Lighten3);
        }

        // Applies standard data cell styling for PDF tables.
        private static IContainer DataStyle(IContainer container)
        {
            return container
                .Border(1)
                .Padding(5);
        }
    }
}