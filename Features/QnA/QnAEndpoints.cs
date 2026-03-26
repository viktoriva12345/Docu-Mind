namespace DocuMind.Features.QnA
{
    public static class QnAEndpoints
    {
        public static void MapQnAEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/ask")
                .WithTags("Q&A");

            group.MapPost("/", Ask)
                .WithDescription("Ask a question about an uploaded document");
        }

        private static async Task<IResult> Ask(AskRequest request, QnAService service)
        {
            try
            {
                var response = await service.AskAsync(request);
                return Results.Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { Error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
        }
    }
}
