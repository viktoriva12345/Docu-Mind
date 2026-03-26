using DocuMind.Features.Documents;
using DocuMind.Infrastructure.AI;

namespace DocuMind.Features.Summarization
{
    public class SummarizeService
    {
        private readonly IAiClient _aiClient;
        private readonly DocumentService _documentService;

        public SummarizeService(IAiClient aiClient, DocumentService documentService)
        {
            _aiClient = aiClient;
            _documentService = documentService;
        }

        public async Task<SummarizeResponse> SummarizeAsync(SummarizeRequest request)
        {
            var document = await _documentService.GetDocumentAsync(request.DocumentId);

            var systemPrompt = request.Length switch
            {
                "short" => "You are a document summarization expert. Provide a concise summary in 2-3 sentences. Respond in the same language as the document.",
                "medium" => "You are a document summarization expert. Provide a summary covering all key points in one paragraph. Respond in the same language as the document.",
                "detailed" => "You are a document summarization expert. Provide a detailed summary with all important information, organized in bullet points. Respond in the same language as the document.",
                _ => "You are a document summarization expert. Provide a concise summary in 2-3 sentences. Respond in the same language as the document."
            };

            var summary = await _aiClient.SendPromptAsync(systemPrompt, document.ExtractedText);

            return new SummarizeResponse
            {
                DocumentId = document.Id,
                Summary = summary,
                Length = request.Length
            };
        }
    }
}
