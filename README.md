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
│   ├── MathHelper.cs
│   ├── NameOfHelper.cs
│   ├── NetworkHelper.cs
│   ├── NumericHelper.cs
│   ├── RegExValidation.cs
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
│   └── Threading/
│       └── Channels/
│           ├── BaseChannelWorker.cs
│           └── ChannelQueue.cs
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
    ├── MathHelperTests.cs
    ├── NameOfHelperTests.cs
    ├── NetworkHelperTests.cs
    ├── NumericHelperTests.cs
    ├── RegExValidationTests.cs
    ├── SerializationHelperTests.cs
    ├── SslHelperTests.cs
    ├── StreamHelperTests.cs
    ├── StringHelperTests.cs
    ├── TimeSeriesHelperTests.cs
    ├── TreeHelperTests.cs
    ├── UnitHelperTests.cs
    └── UriHelperTests.cs
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
| `MathHelper` | Operações matemáticas |
| `NameOfHelper` | Helper para `nameof` |
| `NetworkHelper` | Operações de rede |
| `NumericHelper` | Operações numéricas |
| `RegExValidation` | Validações via Regex |
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

---

## Testes

- **Framework:** xUnit 2.5.3 + FluentAssertions 6.12.1 + Coverlet
- **22 arquivos de teste** — um por helper, com cobertura 1:1

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
| `MathHelperTests` | `MathHelper` |
| `NameOfHelperTests` | `NameOfHelper` |
| `NetworkHelperTests` | `NetworkHelper` |
| `NumericHelperTests` | `NumericHelper` |
| `RegExValidationTests` | `RegExValidation` |
| `SerializationHelperTests` | `SerializationHelper` |
| `SslHelperTests` | `SSLHelper` |
| `StreamHelperTests` | `StreamHelper` |
| `StringHelperTests` | `StringHelper` |
| `TimeSeriesHelperTests` | `TimeseriesHelper` |
| `TreeHelperTests` | `TreeHelper` |
| `UnitHelperTests` | `UnitHelper` |
| `UriHelperTests` | `UriHelper` |

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
- ✅ Infraestrutura assíncrona pronta para uso: `ChannelQueue` + `BaseChannelWorker`
- ✅ Pacote NuGet gerado automaticamente no build (`GeneratePackageOnBuild`)
