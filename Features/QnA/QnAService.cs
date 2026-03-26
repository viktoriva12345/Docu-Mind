using DocuMind.Features.Documents;
using DocuMind.Infrastructure.AI;

namespace DocuMind.Features.QnA
{
    public class QnAService
    {
        private readonly IAiClient _aiClient;
        private readonly DocumentService _documentService;

        public QnAService(IAiClient aiClient, DocumentService documentService)
        {
            _aiClient = aiClient;
            _documentService = documentService;
        }

        public async Task<AskResponse> AskAsync(AskRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
                throw new ArgumentException("Question cannot be empty.");

            var document = await _documentService.GetDocumentAsync(request.DocumentId);

            var systemPrompt = """
            You are a document analysis expert. Answer the user's question based ONLY on the provided document content.
            If the answer cannot be found in the document, say so clearly.
            Respond in the same language as the question.
            Be precise and cite specific details from the document when possible.
            """;

            var userContent = $"Document content:\n{document.ExtractedText}\n\n---\n\nQuestion: {request.Question}";

            var answer = await _aiClient.SendPromptAsync(systemPrompt, userContent);

            return new AskResponse
            {
                DocumentId = document.Id,
                Question = request.Question,
                Answer = answer
            };
        }
    }
}
