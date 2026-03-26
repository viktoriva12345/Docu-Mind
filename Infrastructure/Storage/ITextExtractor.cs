namespace DocuMind.Infrastructure.Storage
{
    public interface ITextExtractor
    {
        bool CanHandle(string fileExtension);
        Task<string> ExtractTextAsync(Stream fileStream, string fileName);
    }
}
