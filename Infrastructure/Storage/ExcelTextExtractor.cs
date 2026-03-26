using ClosedXML.Excel;
using System.Text;

namespace DocuMind.Infrastructure.Storage
{
    public class ExcelTextExtractor : ITextExtractor
    {
        public bool CanHandle(string fileExtension)
        {
            return fileExtension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase);
        }

        public Task<string> ExtractTextAsync(Stream fileStream, string fileName)
        {
            using var workbook = new XLWorkbook(fileStream);

            var textBuilder = new StringBuilder();

            foreach (var worksheet in workbook.Worksheets)
            {
                textBuilder.AppendLine($"=== Sheet: {worksheet.Name} ===");
                textBuilder.AppendLine();

                var usedRange = worksheet.RangeUsed();
                if (usedRange is null) continue;

                foreach (var row in usedRange.RowsUsed())
                {
                    var cells = row.CellsUsed()
                        .Select(cell => cell.GetFormattedString())
                        .ToArray();

                    textBuilder.AppendLine(string.Join(" | ", cells));
                }

                textBuilder.AppendLine();
            }

            return Task.FromResult(textBuilder.ToString().Trim());
        }
    }
}
