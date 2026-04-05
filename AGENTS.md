# AGENTS.md - Cutline Development Guide

This document provides guidance for AI coding agents working in this repository.

## Project Overview

Cutline is a fantasy golf league API built with .NET 10, Entity Framework Core, and PostgreSQL. The backend uses a clean domain model with feature-based organization.

## Build Commands

### Standard Development
```bash
# Restore tools and packages
dotnet tool restore
dotnet restore

# Build the solution
dotnet build

# Build with verbose CSharpier output
dotnet build -v:normal

# Run the API
dotnet run --project Cutline.Api

# Watch mode (hot reload)
dotnet watch --project Cutline.Api

# Production build
dotnet build --configuration release
```

### Testing
Currently, there is no test project in the solution. If tests are added:
```bash
# Run all tests
dotnet test

# Run a specific test
dotnet test --filter "FullyQualifiedName~TestName"

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Commit Message Standards

### Format
Use lowercase with conventional format: `<type>: <description>`

Types:
- `feat` - New feature
- `refactor` - Code organization changes
- `fix` - Bug fixes
- `chore` - Maintenance, tooling
- `format` - Code formatting only
- `docs` - Documentation changes

### Scoping
For projects with multiple components, include scope:
- `feat(api): add GetLeagues endpoint`
- `feat(db): add LeagueTeamsNavigationProp migration`
- `refactor(web): simplify LeagueList component`

### Large Changes
- Break into logical smaller commits when possible
- If a change touches multiple scopes, use multiple commits
- For large commits, add body lines for context

### Writing Messages
1. Run `git diff --staged` to see exactly what changed
2. Focus on **what** changed and **why**
3. Keep under 72 characters for the first line
4. Use past tense: "add" not "adding"

### Code Quality

```bash
# Format all C# files with CSharpier
dotnet csharpier .

# Check if files need formatting (CI mode - exits with 1 if unformatted)
dotnet csharpier --check .

# Format specific file
dotnet csharpier Path/To/File.cs
```

### Database
```bash
# Create migrations (run from Cutline.Api directory)
dotnet ef migrations add <MigrationName>

# Apply migrations
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

## Code Style Guidelines

### General Principles
- **Enable nullable reference types** - Always use `?` for nullable types
- **Use primary constructors** - Prefer `public class Foo(DbContext db)` over traditional constructors
- **Use file-scoped namespaces** - `namespace Cutline.Api.Entities;`
- **Use expression-bodied members** - Prefer `public int Value => _value;` for simple properties
- **No trailing newlines** - Files should not end with blank lines
- **Use `readonly` structs** - Prefer `readonly struct` and `readonly record`

### Formatting Rules (from .editorconfig)
- **Indentation**: 4 spaces (no tabs)
- **Line endings**: CRLF (Windows-style)
- **Braces**: Always required (`csharp_prefer_braces = true`)
- **New lines**: Open brace on new line (`csharp_new_line_before_open_brace = all`)
- **Print width**: 100 characters soft limit
- **Spaces**: Around binary operators, after commas, after keywords in control flow

### Naming Conventions
- **Types/Methods/Properties**: `PascalCase` (e.g., `GetPlayers`, `FullName`)
- **Interfaces**: Prefix with `I` (e.g., `IPlayerRepository`)
- **Private fields**: `_camelCase` (e.g., `_dbContext`, `_httpClient`)
- **Constants**: `PascalCase` (e.g., `DEFAULT_YEAR`)
- **Files**: Match the type name (e.g., `Player.cs` contains `public class Player`)

### Import Organization
- **No explicit sorting required** - CSharpier handles this
- **System directives first** is disabled - keep logical grouping
- **Using directives placement**: `outside_namespace` (before namespace declaration)
- **Implicit usings enabled** - Don't add common namespaces like `System`, `System.Collections.Generic`, etc.

### Type Preferences
- **Use `var`** when type is apparent: `var player = new Player();`
- **Prefer predefined types**: `int`, `string`, `bool` over `Int32`, `String`, `Boolean`
- **Use records** for immutable DTOs: `public sealed record GetPlayersResponse(IReadOnlyList<PlayerDto> Players);`
- **Tuple names**: Use explicit tuple names `var (first, last) = tuple;`

### Pattern Preferences
- **Null coalescing**: Use `??` and `??=` where appropriate
- **Null propagation**: Use `?.` and `?[]` operators
- **Collection initializers**: Prefer `new List<int> { 1, 2, 3 }` over multiple `.Add()` calls
- **Pattern matching**: Use `is` and `is not` patterns
- **Switch expressions**: Prefer over switch statements for simple mappings
- **Discard variables**: Use `_` for unused values

### Error Handling
- **Use `is not null`** instead of `!= null` for pattern matching consistency
- **Throw expressions**: Use `throw new XException(...)` in expression contexts
- **Early returns**: Return early for guard clauses to reduce nesting
- **No empty catch blocks**: Always handle or log exceptions

### Async/Await
- **Use `async Task`** for methods that perform async work
- **Use `CancellationToken`** as last parameter in async methods
- **Don't use `.Result` or `.Wait()`** - Always await async methods
- **Static anonymous functions**: Prefer `csharp_prefer_static_anonymous_function = true`

### Entity Framework
- **Use primary constructors** with `DbContext` injection
- **Use `IReadOnlyList<T>`** for collections that shouldn't be modified
- **Index attributes**: Add `[Index]` for frequently queried columns
- **Use `IsUnique()`** for unique constraints in `OnModelCreating`
- **Async methods**: Prefer async EF methods (`ToListAsync`, `FirstOrDefaultAsync`, etc.)

### API Endpoints Pattern
Use domain-level aggregator files with action classes in the same namespace:

```csharp
// Features/Players/PlayersEndpoints.cs - Aggregator (one per domain)
public static class PlayersEndpoints
{
    public static void MapPlayersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/players", GetPlayers.Handle)
            .WithName("GetPlayers")
            .WithSummary("Gets players")
            .WithTags("Players")
            .Produces<GetPlayers.GetPlayersResponse>();

        app.MapPost("/api/players", CreatePlayer.Handle)
            .WithTags("Players");
    }
}

// Features/Players/GetPlayers.cs - Action class (same namespace)
public static class GetPlayers
{
    public static async Task<IResult> Handle(AppDbContext db)
    {
        // ...
    }

    public sealed record GetPlayersResponse(IReadOnlyList<PlayerDto> Players);
}
```

**Key points:**
- Each domain has an aggregator file (`PlayersEndpoints.cs`)
- Aggregator calls `MapGet`, `MapPost`, etc. directly with hardcoded routes
- Action classes (`GetPlayers`, `CreatePlayer`) have `public static Handle` methods
- All types in same namespace for easy access
- Program.cs calls `app.MapPlayersEndpoints()` (no parameters)

### Attribute Usage
- Use `[JsonPropertyName("camelCase")]` for JSON serialization (explicit control)
- Use `[MaxLength(n)]` for string property validation
- Use `[Required]` sparingly - prefer nullable types with validation

## Architecture

This project follows **Clean Architecture** principles with **Vertical Slice** organization:

- **Entities** - Domain models (no behavior, just data)
- **Features** - Each feature is self-contained with its own DTOs and endpoint
- **Integrations** - External API clients (GolfApi)
- **Jobs** - Background workers (TickerQ)
- **Database** - EF Core DbContext and configuration

**No message handlers or CQRS patterns** - keep it simple.

### Project Structure
```
Cutline.Api/
├── Database/         # AppDbContext, EF configuration
├── Entities/         # Domain models (Player, Tournament)
├── Features/         # Vertical slices by domain
│   └── [Domain]/     # e.g., Players, Teams
│       ├── [Domain]Endpoints.cs   # Aggregator - maps all routes
│       └── [Action].cs            # Action class + DTOs (e.g., GetPlayers.cs)
├── Integrations/      # External clients (GolfApiClient)
├── Jobs/             # Background jobs (TickerQ)
└── Migrations/       # EF migrations
```

## Common Tasks

### Adding a New Endpoint
1. Create action file in domain folder: `Features/Players/GetPlayers.cs`
2. Add action class with `public static Handle` method and DTOs
3. Add route to aggregator: `Features/Players/PlayersEndpoints.cs`
4. Call in `Program.cs`: `app.MapPlayersEndpoints();`

### Adding a New Entity
1. Create file in `Entities/`: `Player.cs`
2. Define properties with appropriate attributes
3. Add to `AppDbContext` as `DbSet<T>`
4. Configure in `OnModelCreating` if needed
5. Create migration: `dotnet ef migrations add AddPlayer`

### Adding a New Background Job
1. Create class in `Jobs/`
2. Inject dependencies via constructor
3. Apply `[TickerFunction]` attribute to the execute method
4. Register in `Program.cs`: `builder.Services.AddScoped<MyJob>();`

### Package Management
All NuGet packages are managed centrally in `Directory.Packages.props`:
1. Add `<PackageVersion Include="PackageName" Version="x.y.z" />` to `Directory.Packages.props`
2. Reference it in `.csproj` as `<PackageReference Include="PackageName" />` (no version)
3. Run `dotnet restore`

## Configuration

Environment variables and `appsettings.json`:
- `ConnectionStrings:DefaultConnection` - PostgreSQL connection string
- `GolfApi:BaseUrl` - Golf API base URL (required)
- `GolfApi:RapidApiKey` - RapidAPI key for golf API
- `GolfApi:RapidApiHost` - RapidAPI host header

## Development Tips

- **CSharpier runs on build** - Your code will be automatically formatted
- **Enable format on save** in your IDE using the CSharpier extension
- **Scalar API docs** available at `/scalar/v1` in development mode
- **TickerQ Dashboard** available at `/jobs` for background job monitoring
- **Use `dotnet watch`** for hot reload during development
