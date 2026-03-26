namespace DocuMind.Features.Summarization
{
    public static class SummarizeEndpoints
    {
        public static void MapSummarizeEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/summarize")
                .WithTags("Summarization");

            group.MapPost("/", Summarize)
                .WithDescription("Generate an AI summary of an uploaded document");
        }

        private static async Task<IResult> Summarize(SummarizeRequest request, SummarizeService service)
        {
            try
            {
                var response = await service.SummarizeAsync(request);
                return Results.Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { Error = ex.Message });
            }
        }
    }
}
