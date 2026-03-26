namespace DocuMind.Features.Classification
{
    public class ClassifyRequest
    {
        public Guid DocumentId { get; set; }
    }

    public class ClassifyResponse
    {
        public Guid DocumentId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string SubCategory { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = [];
    }
}
