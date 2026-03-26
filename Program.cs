using DocuMind.Features.Documents;
using DocuMind.Features.Summarization;
using DocuMind.Infrastructure.AI;
using DocuMind.Infrastructure.Storage;
using Scalar.AspNetCore;
using DocuMind.Features.Classification;
using DocuMind.Features.QnA;
using DocuMind.Features.Extraction;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

// Text extractors
builder.Services.AddSingleton<ITextExtractor, PlainTextExtractor>();
builder.Services.AddSingleton<ITextExtractor, PdfTextExtractor>();
builder.Services.AddSingleton<ITextExtractor, ExcelTextExtractor>();

// Document feature
builder.Services.AddSingleton(new DocumentRepository(connectionString));
builder.Services.AddScoped<DocumentService>();

builder.Services.AddScoped<ClassifyService>();
builder.Services.AddScoped<QnAService>();
builder.Services.AddScoped<ExtractionService>();

// Gemini AI
var geminiApiKey = builder.Configuration["Gemini:ApiKey"]
    ?? throw new InvalidOperationException("Gemini API key is not configured.");

builder.Services.AddSingleton<IAiClient>(new GeminiClient(geminiApiKey));

// Summarization feature
builder.Services.AddScoped<SummarizeService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapDocumentEndpoints();
app.MapSummarizeEndpoints();
app.MapClassifyEndpoints();
app.MapQnAEndpoints();
app.MapExtractionEndpoints();

app.UseHttpsRedirection();

app.Run();