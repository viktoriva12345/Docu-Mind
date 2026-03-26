# ADR-0001: Feature Folders over Clean Architecture

## Status
Accepted

## Context
We needed to choose an architectural approach for DocuMind API. 
The project is an AI document analyzer that serves as a wrapper 
around the Gemini API with five clearly defined features: 
upload, summarization, classification, Q&A, and entity extraction.

Two options were considered:
1. Clean Architecture (Domain/Application/Infrastructure/API layers)
2. Feature folder organization (single project, vertical slices)

## Decision
Feature folder organization was chosen.

## Rationale
- The project domain is simple — there are no complex business rules, 
  aggregates, or domain events that would justify a Domain layer
- Each feature (Summarization, Classification, QnA, Extraction) 
  is independent and self-contained — endpoint, service, and models in one place
- Clean Architecture would add 3 additional projects and numerous abstractions 
  without real benefit for a project of this size
- Feature folders provide better cohesion — everything related to one 
  feature lives in one folder
- Adding a new feature is easier without impacting the rest of the system

## Consequences
- Positive: Faster development, easier navigation, less boilerplate code
- Positive: A new developer can understand a single feature without knowing the entire system
- Negative: If the project grows significantly in complexity, reorganization 
  into a layered structure may be needed
- Trade-off: Shared code (AI client, storage) is extracted into an Infrastructure 
  folder as a shared dependency