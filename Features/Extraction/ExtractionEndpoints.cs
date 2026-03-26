namespace DocuMind.Features.Extraction
{
    public static class ExtractionEndpoints
    {
        public static void MapExtractionEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/extract")
                .WithTags("Extraction");

            group.MapPost("/", Extract)
                .WithDescription("Extract named entities (people, dates, amounts, locations, organizations) from a document");
        }

        private static async Task<IResult> Extract(ExtractRequest request, ExtractionService service)
        {
            try
            {
                var response = await service.ExtractAsync(request);
                return Results.Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { Error = ex.Message });
            }
        }
    }
}
