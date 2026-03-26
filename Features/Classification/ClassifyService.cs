using DocuMind.Features.Documents;
using DocuMind.Infrastructure.AI;
using System.Text.Json;

namespace DocuMind.Features.Classification
{
    public class ClassifyService
    {
        private readonly IAiClient _aiClient;
        private readonly DocumentService _documentService;

        public ClassifyService(IAiClient aiClient, DocumentService documentService)
        {
            _aiClient = aiClient;
            _documentService = documentService;
        }

        public async Task<ClassifyResponse> ClassifyAsync(ClassifyRequest request)
        {
            var document = await _documentService.GetDocumentAsync(request.DocumentId);

            var systemPrompt = """
            You are a document classification expert. Analyze the document and return ONLY a JSON object with this exact structure, no other text:
            {
                "category": "main category (e.g. Legal, Financial, Technical, Medical, Business, Education, Government)",
                "subCategory": "more specific subcategory",
                "tags": ["tag1", "tag2", "tag3", "tag4", "tag5"]
            }
            Respond with valid JSON only. No markdown, no explanation.
            """;

            var result = await _aiClient.SendPromptAsync(systemPrompt, document.ExtractedText);

            var cleaned = result
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            var parsed = JsonSerializer.Deserialize<ClassifyAiResult>(cleaned, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new ClassifyResponse
            {
                DocumentId = document.Id,
                Category = parsed?.Category ?? "Unknown",
                SubCategory = parsed?.SubCategory ?? "Unknown",
                Tags = parsed?.Tags ?? []
            };
        }

        private class ClassifyAiResult
        {
            public string Category { get; set; } = string.Empty;
            public string SubCategory { get; set; } = string.Empty;
            public List<string> Tags { get; set; } = [];
        }
    }
}
