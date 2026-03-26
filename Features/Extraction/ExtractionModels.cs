namespace DocuMind.Features.Extraction
{
    public class ExtractRequest
    {
        public Guid DocumentId { get; set; }
    }

    public class ExtractResponse
    {
        public Guid DocumentId { get; set; }
        public List<string> People { get; set; } = [];
        public List<string> Dates { get; set; } = [];
        public List<string> Amounts { get; set; } = [];
        public List<string> Locations { get; set; } = [];
        public List<string> Organizations { get; set; } = [];
    }
}
