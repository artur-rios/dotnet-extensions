# Changelog

All notable changes to `ArturRios.Extensions` are recorded in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to
[Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed

- `ParseToIntOrDefault` parses with the invariant culture. Under the current culture, `"-5"` returned the default on a
  machine whose culture uses a different negative sign, such as fa-IR or ar-SA.
- `ParseToObjectOrDefault<T>` matches JSON property names regardless of case, like `Clone<T>`, the
  `ArturRios.Util` HTTP helpers and Microsoft.Extensions.Configuration binding. `{"name":"Ana"}` used to come back
  as an object with every member at its default.
- `In` and `NotIn` are declared as classic extension methods. The compiled method signatures are unchanged, so the
  change is binary compatible.

### Fixed

- Calling `In` or `NotIn` with an expanded argument list, such as `2.In(1, 2, 3)`, no longer raises a nullability
  warning (CS8620) for every argument in projects with nullable reference types enabled.
- `ToLogLine` returns a single line, as documented. Line breaks in the stack trace and the message are escaped as
  `\n`; written raw, they split one exception across several log lines and let a crafted message forge entries.
- `PropertiesToDictionary` and `NonNullPropertiesToDictionary` no longer throw `TargetParameterCountException` on an
  object with an indexer, or fail on a write-only property, and no longer include static properties.
- `PrintContents` prints a `Guid`, an enum or a date and time value as itself, instead of the type's static
  properties (`Guid.AllBitsSet`, `DateTime.Now`) or nothing at all, and skips indexers rather than throwing.

## [1.4.0] - 2026-08-24

### Changed

- `IsValidEmail` delegates to `ArturRios.Util.Text.EmailAddress.IsValid`, so mixed-case and internationalized domains
  are accepted.
- `In` and `NotIn` compare with `EqualityComparer<T>.Default` and reject a null range with `ArgumentNullException`
  instead of throwing `NullReferenceException`.
- `ArturRios.Util` updated from 2.0.0 to 2.1.0.

### Fixed

- `IsValidEnumValue<TEnum>` rejects a numeric string that names no declared member; `"999"` was accepted by a
  three-member enum.
- The XML documentation of `In`/`NotIn`, `Clone<T>`, `RemoveMilliseconds`, `HasMinLength`/`HasMaxLength` and
  `TrimChar` now describes what they actually do.

## [1.3.0] - 2026-08-19

### Changed

- `ArturRios.Util` updated from 1.2.0 to 2.0.0, so consumers no longer resolve an outdated transitive dependency.

## [1.2.0] - 2026-07-02

### Changed

- `IsPrime` uses the prime logic from `ArturRios.Util`, updated from 1.0.0 to 1.2.0.

## [1.1.0] - 2026-05-20

### Added

- `IntExtensions.IsPrime` for `int`, `long`, `short`, `byte`, `uint`, `ulong`, `ushort` and `sbyte`.

## [1.0.2] - 2025-12-12

### Removed

- `ObjectExtensions.ToJsonStringContent`.

## [1.0.1] - 2025-12-11

### Changed

- `ParseToBoolOrDefault` and `ParseToIntOrDefault` return `bool?` and `int?` and default to `null` when parsing fails.
- `ToLogLine` writes "No stack trace available" in place of a missing stack trace.

## [1.0.0] - 2025-12-10

### Added

- Extension methods for strings, enumerables, enums, objects, generics, dates, comparisons and exceptions.

[Unreleased]: https://github.com/artur-rios/dotnet-extensions/compare/1.4.0...HEAD
[1.4.0]: https://github.com/artur-rios/dotnet-extensions/compare/v1.3.0...1.4.0
[1.3.0]: https://github.com/artur-rios/dotnet-extensions/compare/v1.2.0...v1.3.0
[1.2.0]: https://github.com/artur-rios/dotnet-extensions/compare/v1.1.0...v1.2.0
[1.1.0]: https://github.com/artur-rios/dotnet-extensions/compare/v1.0.2...v1.1.0
[1.0.2]: https://github.com/artur-rios/dotnet-extensions/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/artur-rios/dotnet-extensions/compare/v1.0.0...v1.0.1
[1.0.0]: https://github.com/artur-rios/dotnet-extensions/releases/tag/v1.0.0
