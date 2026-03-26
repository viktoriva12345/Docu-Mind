namespace DocuMind.Features.Summarization
{
    public class SummarizeRequest
    {
        public Guid DocumentId { get; set; }
        public string Length { get; set; } = "short";
    }

    public class SummarizeResponse
    {
        public Guid DocumentId { get; set; }
        public string Summary { get; set; } = string.Empty;
        public string Length { get; set; } = string.Empty;
    }
}
