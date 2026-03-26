# ADR-0003: Dapper over Entity Framework Core

## Status
Accepted

## Context
Database access to SQL Server is needed for storing document 
metadata and extracted text.

Options considered:
1. Entity Framework Core — full ORM, migrations, LINQ, change tracking
2. Dapper — micro ORM, manual SQL, minimal overhead

## Decision
Dapper was chosen.

## Rationale
- The project has a single table (Documents) with simple CRUD operations
- EF Core would introduce unnecessary overhead: migrations, DbContext 
  configuration, change tracking — none of which is needed for this scope
- Dapper provides full control over SQL queries with transparent mapping
- Performance is better for simple operations due to no ORM overhead
- The SQL creation script is explicit and versioned in the repository

## Consequences
- Positive: Fewer dependencies, faster startup, more transparent queries
- Positive: SQL script serves as database schema documentation
- Negative: Manual SQL writing for each operation
- Negative: No automatic migrations — schema changes require manual SQL scripts
- Trade-off: For a project with a single table, Dapper's advantages 
  outweigh EF Core's benefits