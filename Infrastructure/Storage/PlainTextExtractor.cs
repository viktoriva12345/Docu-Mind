namespace DocuMind.Infrastructure.Storage
{
    public class PlainTextExtractor : ITextExtractor
    {
        public bool CanHandle(string fileExtension)
        {
            return fileExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<string> ExtractTextAsync(Stream fileStream, string fileName)
        {
            using var reader = new StreamReader(fileStream);
            return await reader.ReadToEndAsync();
        }
    }
}
