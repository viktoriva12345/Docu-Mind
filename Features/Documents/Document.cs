namespace DocuMind.Features.Documents
{
    public class Document
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string ExtractedText { get; set; } = string.Empty;
        public string Status { get; set; } = "Ready";
        public long FileSizeBytes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
