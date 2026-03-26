namespace DocuMind.Infrastructure.AI
{
    public interface IAiClient
    {
        Task<string> SendPromptAsync(string systemPrompt, string userContent);
    }
}
