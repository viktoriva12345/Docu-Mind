using System.Text;
using UglyToad.PdfPig;

namespace DocuMind.Infrastructure.Storage
{
    public class PdfTextExtractor : ITextExtractor
    {
        public bool CanHandle(string fileExtension)
        {
            return fileExtension.Equals(".pdf", StringComparison.OrdinalIgnoreCase);
        }

        public Task<string> ExtractTextAsync(Stream fileStream, string fileName)
        {
            using var document = PdfDocument.Open(fileStream);

            var textBuilder = new StringBuilder();

            foreach (var page in document.GetPages())
            {
                textBuilder.AppendLine(page.Text);
                textBuilder.AppendLine();
            }

            return Task.FromResult(textBuilder.ToString().Trim());
        }
    }
}
