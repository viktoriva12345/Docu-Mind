namespace DocuMind.Features.QnA
{
    public class AskRequest
    {
        public Guid DocumentId { get; set; }
        public string Question { get; set; } = string.Empty;
    }

    public class AskResponse
    {
        public Guid DocumentId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}
