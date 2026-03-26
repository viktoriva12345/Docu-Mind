using DocuMind.Infrastructure.Storage;

namespace DocuMind.Features.Documents
{
    public class DocumentService
    {
        private readonly IEnumerable<ITextExtractor> _extractors;
        private readonly DocumentRepository _repository;

        public DocumentService(IEnumerable<ITextExtractor> extractors, DocumentRepository repository)
        {
            _extractors = extractors;
            _repository = repository;
        }

        public async Task<Document> UploadAsync(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);

            var extractor = _extractors.FirstOrDefault(e => e.CanHandle(extension));

            if (extractor is null)
                throw new NotSupportedException($"File type '{extension}' is not supported. Supported types: .pdf, .xlsx, .txt");

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
