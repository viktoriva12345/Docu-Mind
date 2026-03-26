# ADR-0002: Gemini API as AI Provider

## Status
Accepted

## Context
The project requires an external AI provider for document analysis 
(summarization, classification, NER, Q&A). We needed to select an 
AI service that meets technical requirements and budget constraints.

Options considered:
1. OpenAI API (GPT-4o) — widest adoption, rich documentation
2. Anthropic Claude API — strong analytical capabilities, longer context
3. Google Gemini API — free tier available, good for prototyping

## Decision
Google Gemini API was chosen (model: gemini-2.5-flash-lite).

## Rationale
- Free tier enables development and testing at zero cost
- Gemini 2.5 Flash-Lite provides sufficient quality for all 
  planned use cases (summarization, classification, NER, Q&A)
- Communication with the API is implemented through an IAiClient interface, 
  allowing provider replacement without changing business logic
- REST API is straightforward — single endpoint for content generation

## Consequences
- Positive: Zero cost for development and testing
- Positive: IAiClient abstraction enables easy migration to 
  OpenAI or Claude if needed
- Negative: Free tier has rate limiting that can slow down testing
- Negative: Response quality may be lower than GPT-4o or Claude 
  for complex documents, but sufficient for proof of concept