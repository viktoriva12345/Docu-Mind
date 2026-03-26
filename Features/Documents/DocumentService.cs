using DocuMind.Infrastructure.Storage;

namespace DocuMind.Features.Documents
{
    public class DocumentService
    {
        private readonly IEnumerable<ITextExtractor> _extractors;
        private readonly DocumentRepository _repository;
        private readonly long _maxFileSizeBytes;
        private readonly string[] _allowedExtensions;

        public DocumentService(
            IEnumerable<ITextExtractor> extractors,
            DocumentRepository repository,
            IConfiguration configuration)
        {
            _extractors = extractors;
            _repository = repository;
            _maxFileSizeBytes = configuration.GetValue<int>("Upload:MaxFileSizeMB", 10) * 1024L * 1024L;
            _allowedExtensions = configuration.GetSection("Upload:AllowedExtensions")
                .Get<string[]>() ?? [".pdf", ".xlsx", ".txt"];
        }

        public async Task<Document> UploadAsync(IFormFile file)
        {
            if (file.Length == 0)
                throw new ArgumentException("Uploaded file is empty.");

            if (file.Length > _maxFileSizeBytes)
                throw new ArgumentException($"File size exceeds the maximum allowed size of {_maxFileSizeBytes / (1024 * 1024)} MB.");

            var extension = Path.GetExtension(file.FileName);

            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentException("File has no extension. Supported types: .pdf, .xlsx, .txt");

            if (!_allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                throw new NotSupportedException($"File type '{extension}' is not supported. Supported types: {string.Join(", ", _allowedExtensions)}");

            var extractor = _extractors.FirstOrDefault(e => e.CanHandle(extension));

            if (extractor is null)
                throw new NotSupportedException($"No text extractor found for '{extension}'.");

            using var stream = file.OpenReadStream();
            var extractedText = await extractor.ExtractTextAsync(stream, file.FileName);

            if (string.IsNullOrWhiteSpace(extractedText))
                throw new InvalidOperationException("No text could be extracted from the uploaded file.");

            var document = new Document
            {
                FileName = file.FileName,
                FileExtension = extension,
                ExtractedText = extractedText,
                FileSizeBytes = file.Length,
                Status = "Ready"
            };

            await _repository.SaveAsync(document);

            return document;
        }

        public async Task<Document> GetDocumentAsync(Guid id)
        {
            var document = await _repository.GetByIdAsync(id);

            if (document is null)
                throw new KeyNotFoundException($"Document with ID '{id}' was not found.");

            return document;
        }
    }
}
