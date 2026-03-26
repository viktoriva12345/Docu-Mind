using DocuMind.Features.Documents;
using DocuMind.Infrastructure.AI;
using System.Text.Json;

namespace DocuMind.Features.Extraction
{
    public class ExtractionService
    {
        private readonly IAiClient _aiClient;
        private readonly DocumentService _documentService;

        public ExtractionService(IAiClient aiClient, DocumentService documentService)
        {
            _aiClient = aiClient;
            _documentService = documentService;
        }

        public async Task<ExtractResponse> ExtractAsync(ExtractRequest request)
        {
            var document = await _documentService.GetDocumentAsync(request.DocumentId);

            var systemPrompt = """
            You are a named entity recognition expert. Extract all entities from the document and return ONLY a JSON object with this exact structure, no other text:
            {
                "people": ["list of person names found"],
                "dates": ["list of dates found"],
                "amounts": ["list of monetary amounts found"],
                "locations": ["list of locations, addresses, cities, countries found"],
                "organizations": ["list of company names, institutions found"]
            }
            Return empty arrays if no entities of that type are found.
            Respond with valid JSON only. No markdown, no explanation.
            """;

            var result = await _aiClient.SendPromptAsync(systemPrompt, document.ExtractedText);

            var cleaned = result
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            var parsed = JsonSerializer.Deserialize<ExtractAiResult>(cleaned, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new ExtractResponse
            {
                DocumentId = document.Id,
                People = parsed?.People ?? [],
                Dates = parsed?.Dates ?? [],
                Amounts = parsed?.Amounts ?? [],
                Locations = parsed?.Locations ?? [],
                Organizations = parsed?.Organizations ?? []
            };
        }

        private class ExtractAiResult
        {
            public List<string> People { get; set; } = [];
            public List<string> Dates { get; set; } = [];
            public List<string> Amounts { get; set; } = [];
            public List<string> Locations { get; set; } = [];
            public List<string> Organizations { get; set; } = [];
        }
    }
}
