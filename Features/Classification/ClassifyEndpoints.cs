namespace DocuMind.Features.Classification
{
    public static class ClassifyEndpoints
    {
        public static void MapClassifyEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/classify")
                .WithTags("Classification");

            group.MapPost("/", Classify)
                .WithDescription("Classify a document into categories with tags");
        }

        private static async Task<IResult> Classify(ClassifyRequest request, ClassifyService service)
        {
            try
            {
                var response = await service.ClassifyAsync(request);
                return Results.Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { Error = ex.Message });
            }
        }
    }
}
