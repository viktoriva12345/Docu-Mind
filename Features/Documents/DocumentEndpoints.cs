namespace DocuMind.Features.Documents
{
    public static class DocumentEndpoints
    {
        public static void MapDocumentEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/documents")
                .WithTags("Documents");

            group.MapPost("/upload", UploadDocument)
                .DisableAntiforgery()
                .WithDescription("Upload a document (PDF, XLSX, TXT) for AI analysis");

            group.MapGet("/{id:guid}", GetDocument)
                .WithDescription("Get document details by ID");
        }

        private static async Task<IResult> UploadDocument(IFormFile file, DocumentService service)
        {
            try
            {
                var document = await service.UploadAsync(file);

                var preview = document.ExtractedText.Length > 200
                    ? document.ExtractedText[..200] + "..."
                    : document.ExtractedText;

                return Results.Ok(new
                {
                    document.Id,
                    document.FileName,
                    document.FileExtension,
                    document.Status,
                    document.FileSizeBytes,
                    document.CreatedAt,
                    ExtractedTextPreview = preview
                });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
            catch (NotSupportedException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Results.UnprocessableEntity(new { Error = ex.Message });
            }
        }

        private static async Task<IResult> GetDocument(Guid id, DocumentService service)
        {
            try
            {
                var document = await service.GetDocumentAsync(id);

                return Results.Ok(new
                {
                    document.Id,
                    document.FileName,
                    document.FileExtension,
                    document.Status,
                    document.FileSizeBytes,
                    document.CreatedAt,
                    TextLength = document.ExtractedText.Length
                });
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { Error = ex.Message });
            }
        }
    }
}
