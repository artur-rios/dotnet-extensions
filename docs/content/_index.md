+++
title = 'Dotnet Extensions'
+++

# Dotnet Extensions

A small, focused set of C# extension methods that make every day .NET work more pleasant. Covers strings, enums,
collections, objects, dates, comparisons, and exceptions. Lightweight, dependency-free, fully unit-tested.

- Package Id: `ArturRios.Extensions`

## Install

NuGet:

```powershell
# Package Manager
Install-Package ArturRios.Extensions

# .NET CLI
dotnet add package ArturRios.Extensions
```

Git submodule:

```powershell
# add under ./lib/ArturRios.Extensions
git submodule add https://github.com/ArturRios/dotnet-extensions lib/ArturRios.Extensions
```

Then reference the project from your solution:

```xml

<ProjectReference Include="lib/ArturRios.Extensions/src/ArturRios.Extensions.csproj"/>
```

## Supported frameworks

- .NET 10.0 (net10.0)

The library is built for modern .NET and has no external runtime dependencies.

## Quick start

Add a `using` to bring extensions into scope and call them like any other instance method:

```csharp
using ArturRios.Extensions;

var hasLower = "AbC".HasLowerChar();     // true
var hasUpper = "abc".HasUpperChar();     // false
var hasNumber = "a1b".HasNumber();       // true

var okMin = "ab".HasMinLength(2);        // true
var okMax = "abcd".HasMaxLength(3);      // false

var isEmail = "user@example.com".IsValidEmail(); // true

var cleaned = "xxvaluexx".TrimChar('x'); // "value"

var value = ((string?)null).ValueOrDefault("fallback"); // "fallback"

var numbers = new[] { 1, 2, 3 };
var printed = numbers.JoinWith(" | "); // "1 | 2 | 3"

var inSet = 2.In([0, 2, 4]);            // true
var notIn = 3.NotIn([0, 2, 4]);         // true

var at = new DateTime(2025, 12, 10, 15, 30, 45, 999).RemoveMilliseconds();
// 2025-12-10 15:30:45.000
```

## API overview

Below is a summary of the most commonly used extension methods with short examples. See source files under `src/` and
unit tests under `tests/` for the full list and behavior.

### String extensions (`StringExtensions`)

- `HasLowerChar()` / `HasUpperChar()` / `HasNumber()`
    - True/false checks for at least one lower/upper/digit character.
- `HasMinLength(int min)` / `HasMaxLength(int max)`
    - Length guardrails that treat `null` as failing and empty as length `0`.
- `IsValidEmail()`
    - Practical email pattern validation.
- `TrimChar(char c)`
    - Trims the given character from both ends. Safe with `null`.
- `ParseToBoolOrDefault(bool default)` / `ParseToIntOrDefault(int default)`
    - Safe parsing that returns the provided default on invalid or `null` input.
- `ParseToObjectOrDefault<T>()`
    - Parses JSON into `T`; returns `null` on invalid or empty input.
- `IsValidEnumValue<TEnum>(bool ignoreCase = true)`
    - Checks if a string matches an enum name. Case-insensitive by default.
- `JoinWith(string separator = ", ")` (for `IEnumerable<string>` and `IEnumerable<object?>`)
    - Concatenates elements with a separator, converting objects via `ToString()` and allowing `null`.

### Enumerable extensions (`EnumerableExtensions`)

- `IsEmpty()` / `IsNotEmpty()`
    - Works for any `IEnumerable`. Avoids materializing where possible.
- `PrintContents()`
    - Writes primitive items directly and complex object properties to `Console.Out`. Handles `null` enumerable.

### Enum extensions (`EnumExtensions`)

- `GetDescription()`
    - Returns the `DescriptionAttribute` value for an enum member, or `null` when absent.

### Object extensions (`ObjectExtensions`)

- `ToJsonStringContent()`
    - Creates an `HttpContent` (application/json) from an object using `System.Text.Json`.
- `NonNullPropertiesToDictionary()` / `PropertiesToDictionary()`
    - Reflects an object into a dictionary of property names to values, optionally skipping `null` values.

### Generic extensions (`GenericExtensions`)

- `Clone<T>()`
    - Safe deep clone for reference types, and value copy for value types. Returns `null` when the source is `null`.

### DateTime extensions (`DateTimeExtensions`)

- `RemoveMilliseconds()`
    - Drops milliseconds while preserving the `DateTimeKind` and all other components.

### Comparison extensions (`ComparisonExtensions`)

- `In<T>(IEnumerable<T> set)` / `NotIn<T>(IEnumerable<T> set)`
    - Membership helpers for readability.

### Exception extensions (`ExceptionExtensions`)

- `ToLogLine(out Guid traceId)`
    - Produces a single-line log string with timestamp, trace id, exception type, message, and stack trace. Throws
      `NullReferenceException` when called on `null`.

## Usage notes

- All methods are `static` extensions under the `ArturRios.Extensions` namespace.
- The library prioritizes clarity and safety: methods are null-aware where it makes sense and avoid throwing on common
  invalid input.
- No external dependencies; uses BCL APIs like `System.Text.Json` and reflection where applicable.

## Contributing

- Issues and PRs are welcome. If you plan a larger change, open an issue first with a short proposal.
- Coding style: follow existing conventions; keep APIs small and focused.

## Build, test and publish

Use the official [.NET CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/) to build, test and publish the project
and Git for source control.
If you want, optional helper toolsets I built to facilitate these tasks are available:

- [Dotnet Tools](https://github.com/artur-rios/dotnet-tools)
- [Python Dotnet Tools](https://github.com/artur-rios/python-dotnet-tools)

## Versioning

Semantic Versioning (SemVer). Breaking changes result in a new major version. New methods or non-breaking behavior
changes increment the minor version; fixes or tweaks increment the patch.

## Legal Details

This project is licensed under the [MIT License](https://en.wikipedia.org/wiki/MIT_License). A copy of the license is
available at [LICENSE](https://github.com/artur-rios/dotnet-extensions/blob/main/LICENSE) in the repository.
