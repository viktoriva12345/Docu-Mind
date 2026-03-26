# ADR-0004: Minimal API over Controllers

## Status
Accepted

## Context
We needed to choose an API endpoint definition style in ASP.NET Core 9.

Options considered:
1. Controller-based API — classic MVC approach, attributes, conventions
2. Minimal API — functional style, less boilerplate

## Decision
Minimal API with MapGroup extension methods per feature was chosen.

## Rationale
- Minimal API naturally fits the feature folder organization — 
  each feature has its own extension method (MapSummarizeEndpoints, etc.)
- Less ceremonial code — no base controllers, attributes, 
  ActionResult wrappers
- MapGroup enables endpoint grouping with shared prefixes and tags
- Better suited for APIs with fewer endpoints per feature (1-2 per group)
- .NET 9 Minimal API is feature-complete for API development

## Consequences
- Positive: Cleaner Program.cs with a clear overview of all routes
- Positive: Each feature class is self-contained and easily testable
- Negative: For large APIs with many endpoints, controllers may 
  provide better organization
- Negative: Less familiar to developers coming from the MVC world