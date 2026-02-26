# cs-helpers

> A utility class library for .NET — helpers for strings, enums, collections, files, dates, networking, validations, and asynchronous processing.

| Info | Value |
|------|-------|
| **Version** | 1.1.9 |
| **Framework** | .NET 8 / C# 12 |
| **Author** | Sergio Machado |
| **Package ID** | `cs-helpers` |

---

## Project Structure

```
helpers/
├── cs-helpers.sln
├── CHANGELOG.md
├── src/
│   ├── Helpers.csproj
│   ├── Base64Helper.cs
│   ├── BinaryHelper.cs
│   ├── BooleanHelper.cs
│   ├── BrasilDocsValidationHelper.cs
│   ├── DateTimeHelper.cs
│   ├── EnumHelper.cs
│   ├── EnumerableExtensions.cs
│   ├── FileHelper.cs
│   ├── FloatingNumberHelper.cs
│   ├── GuidHelper.cs
│   ├── JsonHelper.cs
│   ├── MathHelper.cs
│   ├── NameOfHelper.cs
│   ├── NetworkHelper.cs
│   ├── NumericHelper.cs
│   ├── PasswordHelper.cs
│   ├── RegExValidation.cs
│   ├── RetryHelper.cs
│   ├── SSLHelper.cs
│   ├── SerializationHelper.cs
│   ├── StreamHelper.cs
│   ├── StringHelper.cs
│   ├── TimeseriesHelper.cs
│   ├── TreeHelper.cs
│   ├── UnitHelper.cs
│   ├── UriHelper.cs
│   ├── Extensions/
│   │   ├── CurrencyExtensions.cs
│   │   └── NullSafeExtensions.cs
│   ├── Threading/
│   │   └── Channels/
│   │       ├── BaseChannelWorker.cs
│   │       └── ChannelQueue.cs
│   ├── BackgroundTaskQueue.cs
│   ├── CircuitBreakerHelper.cs
│   ├── EventBusHelper.cs
│   ├── InMemoryCacheHelper.cs
│   ├── PipelineBuilder.cs
│   └── RateLimiterHelper.cs
└── tests/
    ├── Helpers.Tests.csproj
    ├── Base64HelperTests.cs
    ├── BinaryHelperTests.cs
    ├── BooleanHelperTests.cs
    ├── BrasilDocsValidationHelperTests.cs
    ├── DateTimeHelperTests.cs
    ├── EnumHelperTests.cs
    ├── EnumerableExtensionsTests.cs
    ├── FileHelperTests.cs
    ├── FloatingNumberHelperTests.cs
    ├── GuidHelperTests.cs
    ├── JsonHelperTests.cs
    ├── MathHelperTests.cs
    ├── NameOfHelperTests.cs
    ├── NetworkHelperTests.cs
    ├── NumericHelperTests.cs
    ├── PasswordHelperTests.cs
    ├── RegExValidationTests.cs
    ├── RetryHelperTests.cs
    ├── SerializationHelperTests.cs
    ├── SslHelperTests.cs
    ├── StreamHelperTests.cs
    ├── StringHelperTests.cs
    ├── TimeSeriesHelperTests.cs
    ├── TreeHelperTests.cs
    ├── UnitHelperTests.cs
    ├── UriHelperTests.cs
    │
    ├── BackgroundTaskQueueTests.cs
    ├── CircuitBreakerHelperTests.cs
    ├── EventBusHelperTests.cs
    ├── InMemoryCacheHelperTests.cs
    ├── PipelineBuilderTests.cs
    └── RateLimiterHelperTests.cs
```

---

## Classes and Helpers

### General Utilities (`namespace Helpers`)

| Class | Description |
|-------|-------------|
| `Base64Helper` | Base64 encoding/decoding |
| `BinaryHelper` | Binary operations |
| `BooleanHelper` | Boolean operations |
| `BrasilDocsValidationHelper` | Brazilian document validation: **CPF**, **CNPJ**, **PIS**, **CEP** |
| `DateTimeHelper` | Date and time manipulation |
| `EnumHelper` | Enum operations: `Parse`, `TryParse`, `GetDescription`, `ToList`, `GetNames`, `GetValues`, `ConvertToDescriptionDictionary`, `IsDefined` |
| `EnumerableExtensions` | Collection extensions: `HasData`, `IsEmpty`, `ForEach`, `ChunkBy`, `DistinctBy`, `Shuffle`, `Partition`, `FindItemWithNeighbors`, `ToCommaSeparatedString` |
| `FileHelper` | File operations |
| `FloatingNumberHelper` | Floating-point operations |
| `GuidHelper` | Validation, URL-safe Base64 short GUID, safe parse and sequential generation |
| `JsonHelper` | `TryDeserialize`, `Prettify`, `Minify`, `Merge`, `SerializeCamelCase`, `TryGetProperty`, `Flatten` |
| `MathHelper` | Mathematical operations |
| `NameOfHelper` | Helper for `nameof` expressions |
| `NetworkHelper` | Network operations |
| `NumericHelper` | Numeric operations |
| `PasswordHelper` | Secure password generation, strength validation, `GetStrengthScore` and SHA256 hashing |
| `RegExValidation` | Regex-based validations |
| `RetryHelper` | Retry policy: `Execute`, `ExecuteAsync`, `ExecuteWithExponentialBackoffAsync` |
| `SerializationHelper` | Object serialization/deserialization |
| `SSLHelper` | SSL utilities |
| `StreamHelper` | Stream operations |
| `StringHelper` | String validations and manipulation: `IsAlpha`, `IsNumeric`, `IsValidEmail`, `IsValidUrl`, `ToCamelCase`, `ToSnakeCase`, `ToTitleCase`, `Truncate`, `TextAfter`, `GetLineNumber` |
| `TimeseriesHelper` | Time series operations |
| `TreeHelper` | Tree structure operations |
| `UnitHelper` | Unit conversion |
| `UriHelper` | URI manipulation |

### Extensions (`namespace Helpers.Extensions`)

| Class | Description |
|-------|-------------|
| `CurrencyExtensions` | Currency formatting extensions |
| `NullSafeExtensions` | Elegant null handling: `ThrowIfNull`, `ThrowIfEmpty`, `NullIf`, `IsIn`, `SafeFirstOrDefault`, `IfNotNull`, `With` |

### Threading (`namespace Helpers.Threading.Channels`)

| Class | Description |
|-------|-------------|
| `BaseChannelWorker<T>` | Abstract `BackgroundService` worker that consumes the queue with DI scope isolation |
| `ChannelQueue<T>` | Async queue using `System.Threading.Channels` — configurable capacity, `Wait` mode when full |

### Architectural Patterns (`namespace Helpers`)

| Class | Description |
|-------|-------------|
| `BackgroundTaskQueue` | Thread-safe queue of `async` delegates for background jobs. `EnqueueAsync`, `DequeueAsync`, `ReadAllAsync` |
| `CircuitBreakerHelper` | Circuit breaker pattern with `Closed`/`Open`/`HalfOpen` states, configurable threshold and duration |
| `EventBusHelper` | Lightweight in-process pub/sub with sync and async handlers. `Subscribe`, `Publish`, `PublishAsync`, `Unsubscribe` |
| `InMemoryCacheHelper` | In-memory cache with per-entry TTL, `GetOrSet` sync/async, `Invalidate` by key and prefix |
| `PipelineBuilder<T>` | Chains async middlewares over a generic context. `Use`, `UseTerminal`, `Build`, `ExecuteAsync` |
| `RateLimiterHelper` | Thread-safe token bucket. `TryAcquire`, sync/async `Execute`, automatic token refill |

---

## Tests

- **Framework:** xUnit 2.5.3 + FluentAssertions 6.12.1 + Coverlet
- **32 test files** — one per helper, with 1:1 coverage

| Test Class | Helper Tested |
|------------|---------------|
| `Base64HelperTests` | `Base64Helper` |
| `BinaryHelperTests` | `BinaryHelper` |
| `BooleanHelperTests` | `BooleanHelper` |
| `BrasilDocsValidationHelperTests` | `BrasilDocsValidationHelper` |
| `DateTimeHelperTests` | `DateTimeHelper` |
| `EnumHelperTests` | `EnumHelper` |
| `EnumerableExtensionsTests` | `EnumerableExtensions` |
| `FileHelperTests` | `FileHelper` |
| `FloatingNumberHelperTests` | `FloatingNumberHelper` |
| `GuidHelperTests` | `GuidHelper` |
| `JsonHelperTests` | `JsonHelper` |
| `MathHelperTests` | `MathHelper` |
| `NameOfHelperTests` | `NameOfHelper` |
| `NetworkHelperTests` | `NetworkHelper` |
| `NumericHelperTests` | `NumericHelper` |
| `PasswordHelperTests` | `PasswordHelper` |
| `RegExValidationTests` | `RegExValidation` |
| `RetryHelperTests` | `RetryHelper` |
| `SerializationHelperTests` | `SerializationHelper` |
| `SslHelperTests` | `SSLHelper` |
| `StreamHelperTests` | `StreamHelper` |
| `StringHelperTests` | `StringHelper` |
| `TimeSeriesHelperTests` | `TimeseriesHelper` |
| `TreeHelperTests` | `TreeHelper` |
| `UnitHelperTests` | `UnitHelper` |
| `UriHelperTests` | `UriHelper` |
| **Architectural Patterns** | |
| `BackgroundTaskQueueTests` | `BackgroundTaskQueue` |
| `CircuitBreakerHelperTests` | `CircuitBreakerHelper` |
| `EventBusHelperTests` | `EventBusHelper` |
| `InMemoryCacheHelperTests` | `InMemoryCacheHelper` |
| `PipelineBuilderTests` | `PipelineBuilder<T>` |
| `RateLimiterHelperTests` | `RateLimiterHelper` |

---

## Dependencies

**Main library (`src`):**
- `System.Threading.Channels` 8.0.0
- `Microsoft.Extensions.Hosting` 8.0.1

**Tests (`tests`):**
- `xunit` 2.5.3
- `FluentAssertions` 6.12.1
- `coverlet.collector` 6.0.0

---

## Highlights

- ✅ All methods documented with XML doc comments
- ✅ Nullable enabled (`#nullable enable`)
- ✅ `[GeneratedRegex]` used in `BrasilDocsValidationHelper` for better performance
- ✅ Fluent API via extension methods throughout the library
- ✅ Async infrastructure ready to use: `ChannelQueue` + `BaseChannelWorker` + `BackgroundTaskQueue`
- ✅ Built-in resilience: `RetryHelper` (fixed + exponential backoff) + `CircuitBreakerHelper`
- ✅ In-memory cache without DI dependency: `InMemoryCacheHelper` with TTL and automatic eviction
- ✅ Lightweight in-process pub/sub without a broker: `EventBusHelper`
- ✅ Pipeline composition: `PipelineBuilder<T>` with short-circuit support
- ✅ Rate control: `RateLimiterHelper` (token bucket, thread-safe)
- ✅ NuGet package automatically generated on build (`GeneratePackageOnBuild`)

---

## How to consume the private package (GitHub Packages)

This package is published privately to GitHub Packages associated with the `slmachado` account/organization. To consume the `cs-helpers` package in your local projects or CI pipelines, follow the steps below:

### 1. Generate a Personal Access Token (PAT)
You need a GitHub token with the `read:packages` permission.
- Go to: GitHub -> Settings -> Developer settings -> Personal access tokens -> Tokens (classic) -> Generate new token (classic).
- Check the `read:packages` scope.
- Copy the generated token.

### 2. Configure nuget.config in your local repository
In the root of your project or solution that will consume the `cs-helpers` package, create or edit the `nuget.config` file and define the credentials:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="slmachado" value="https://nuget.pkg.github.com/slmachado/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <slmachado>
      <add key="Username" value="YOUR_GITHUB_USERNAME" />
      <add key="ClearTextPassword" value="YOUR_PAT_WITH_READ_PACKAGES" />
    </slmachado>
  </packageSourceCredentials>
</configuration>
```

> ⚠️ **Warning:** Make sure to add `nuget.config` to your `.gitignore` if it contains plain text passwords (like your local machine PAT), or use the NuGet CLI / environment variables in CI pipelines. For CI pipelines, replace the PAT with the appropriate environment variable or secret.

### 3. Install the package
After configuring the feed, you can install the package normally using the .NET CLI:
```bash
dotnet add package cs-helpers
```
