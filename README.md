# cs-helpers

> Biblioteca de classes utilitárias para .NET — helpers para strings, enums, coleções, arquivos, datas, redes, validações e processamento assíncrono.

| Info | Valor |
|------|-------|
| **Versão** | 1.1.9 |
| **Framework** | .NET 8 / C# 12 |
| **Autor** | Sergio Machado |
| **Package ID** | `cs-helpers` |

---

## Estrutura do Projeto

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

## Classes e Helpers

### Utilitários Gerais (`namespace Helpers`)

| Classe | Descrição |
|--------|-----------|
| `Base64Helper` | Codificação/decodificação Base64 |
| `BinaryHelper` | Operações binárias |
| `BooleanHelper` | Operações booleanas |
| `BrasilDocsValidationHelper` | Validação de documentos brasileiros: **CPF**, **CNPJ**, **PIS**, **CEP** |
| `DateTimeHelper` | Manipulação de datas e horas |
| `EnumHelper` | Operações com enums: `Parse`, `TryParse`, `GetDescription`, `ToList`, `GetNames`, `GetValues`, `ConvertToDescriptionDictionary`, `IsDefined` |
| `EnumerableExtensions` | Extensões de coleções: `HasData`, `IsEmpty`, `ForEach`, `ChunkBy`, `DistinctBy`, `Shuffle`, `Partition`, `FindItemWithNeighbors`, `ToCommaSeparatedString` |
| `FileHelper` | Operações com arquivos |
| `FloatingNumberHelper` | Operações com ponto flutuante |
| `GuidHelper` | Validação, short GUID Base64 URL-safe, parse seguro e geração sequencial |
| `JsonHelper` | `TryDeserialize`, `Prettify`, `Minify`, `Merge`, `SerializeCamelCase`, `TryGetProperty`, `Flatten` |
| `MathHelper` | Operações matemáticas |
| `NameOfHelper` | Helper para `nameof` |
| `NetworkHelper` | Operações de rede |
| `NumericHelper` | Operações numéricas |
| `PasswordHelper` | Geração segura de senhas, validação de força, score e hash SHA256 |
| `RegExValidation` | Validações via Regex |
| `RetryHelper` | Política de retentativa: `Execute`, `ExecuteAsync`, `ExecuteWithExponentialBackoffAsync` |
| `SerializationHelper` | Serialização/desserialização de objetos |
| `SSLHelper` | Auxiliares SSL |
| `StreamHelper` | Operações com streams |
| `StringHelper` | Validações e manipulações: `IsAlpha`, `IsNumeric`, `IsValidEmail`, `IsValidUrl`, `ToCamelCase`, `ToSnakeCase`, `ToTitleCase`, `Truncate`, `TextAfter`, `GetLineNumber` |
| `TimeseriesHelper` | Operações com séries temporais |
| `TreeHelper` | Operações com estruturas de árvore |
| `UnitHelper` | Conversão de unidades |
| `UriHelper` | Manipulação de URIs |

### Extensões (`namespace Helpers.Extensions`)

| Classe | Descrição |
|--------|-----------|
| `CurrencyExtensions` | Extensões de formatação de moeda |
| `NullSafeExtensions` | Tratamento elegante de nulos: `ThrowIfNull`, `ThrowIfEmpty`, `NullIf`, `IsIn`, `SafeFirstOrDefault`, `IfNotNull`, `With` |

### Threading (`namespace Helpers.Threading.Channels`)

| Classe | Descrição |
|--------|-----------|
| `BaseChannelWorker<T>` | Worker base (`BackgroundService`) que consome a fila com isolamento de escopo via DI |
| `ChannelQueue<T>` | Fila assíncrona com `System.Threading.Channels` — capacidade configurável, modo `Wait` quando cheia |

### Patterns Arquiteturais (`namespace Helpers`)

| Classe | Descrição |
|--------|-----------|
| `BackgroundTaskQueue` | Fila thread-safe de delegates `async` para background jobs. `EnqueueAsync`, `DequeueAsync`, `ReadAllAsync` |
| `CircuitBreakerHelper` | Padrão circuit breaker com estados `Closed`/`Open`/`HalfOpen`, threshold e duração configuráveis |
| `EventBusHelper` | Pub/sub in-process com handlers sync e async. `Subscribe`, `Publish`, `PublishAsync`, `Unsubscribe` |
| `InMemoryCacheHelper` | Cache in-memory com TTL por entrada, `GetOrSet` sync/async, `Invalidate` por chave e prefixo |
| `PipelineBuilder<T>` | Encadeia middlewares async sobre um contexto genérico. `Use`, `UseTerminal`, `Build`, `ExecuteAsync` |
| `RateLimiterHelper` | Token bucket thread-safe. `TryAcquire`, `Execute` sync/async, reabastecimento automático |

---

## Testes

- **Framework:** xUnit 2.5.3 + FluentAssertions 6.12.1 + Coverlet
- **32 arquivos de teste** — um por helper, com cobertura 1:1

| Classe de Teste | Helper Testado |
|----------------|----------------|
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
| **Patterns Arquiteturais** | |
| `BackgroundTaskQueueTests` | `BackgroundTaskQueue` |
| `CircuitBreakerHelperTests` | `CircuitBreakerHelper` |
| `EventBusHelperTests` | `EventBusHelper` |
| `InMemoryCacheHelperTests` | `InMemoryCacheHelper` |
| `PipelineBuilderTests` | `PipelineBuilder<T>` |
| `RateLimiterHelperTests` | `RateLimiterHelper` |

---

## Dependências

**Biblioteca principal (`src`):**
- `System.Threading.Channels` 8.0.0
- `Microsoft.Extensions.Hosting` 8.0.1

**Testes (`tests`):**
- `xunit` 2.5.3
- `FluentAssertions` 6.12.1
- `coverlet.collector` 6.0.0

---

## Destaques

- ✅ Todos os métodos documentados com XML doc comments
- ✅ Nullable habilitado (`#nullable enable`)
- ✅ `[GeneratedRegex]` utilizado em `BrasilDocsValidationHelper` para melhor performance
- ✅ API fluente via extension methods em toda a biblioteca
- ✅ Infraestrutura assíncrona pronta para uso: `ChannelQueue` + `BaseChannelWorker` + `BackgroundTaskQueue`
- ✅ Resiliência embutida: `RetryHelper` (fixo + exponential backoff) + `CircuitBreakerHelper`
- ✅ Cache in-memory sem dependência de DI: `InMemoryCacheHelper` com TTL e eviction automático
- ✅ Pub/sub in-process leve e sem broker: `EventBusHelper`
- ✅ Composição de pipelines: `PipelineBuilder<T>` com suporte a short-circuit
- ✅ Função RateLimiter (Token Bucket) (`RateLimiterHelper`) thread-safe.
- ✅ Pacote NuGet gerado automaticamente no build (`GeneratePackageOnBuild`)

---

## Como consumir o pacote privado (GitHub Packages)

Este pacote é publicado de forma privada no GitHub Packages associado à conta/organização `slmachado`. Para consumir o pacote `cs-helpers` nos seus projetos locais ou em pipelines de CI, siga os passos abaixo:

### 1. Gerar um Personal Access Token (PAT)
Você precisa de um token do GitHub com a permissão `read:packages`.
- Vá para: GitHub -> Settings -> Developer settings -> Personal access tokens -> Tokens (classic) -> Generate new token (classic).
- Marque o escopo `read:packages`.
- Copie o token gerado.

### 2. Configurar o nuget.config no seu repositório local
Na raiz do seu projeto ou solução que irá consumir o pacote `cs-helpers`, crie ou edite o arquivo `nuget.config` e defina as credenciais:

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
      <add key="Username" value="SEU_USUARIO_DO_GITHUB" />
      <add key="ClearTextPassword" value="SEU_PAT_COM_READ_PACKAGES" />
    </slmachado>
  </packageSourceCredentials>
</configuration>
```

> ⚠️ **Atenção:** Certifique-se de adicionar `nuget.config` ao seu `.gitignore` caso ele contenha senhas em texto claro (como o seu PAT local de máquina), ou utilize a CLI do NuGet / variáveis de ambiente em esteiras de CI. Para pipelines de CI locais, substitua o PAT pela variável de ambiente com o secret apropriado.

### 3. Instalar o pacote
Depois de configurar o feed, você pode adicionar o pacote normalmente usando o CLI do .NET:
```bash
dotnet add package cs-helpers
```
