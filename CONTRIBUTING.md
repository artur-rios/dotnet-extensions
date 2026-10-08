# Contributing

- Issues and PRs are welcome. If you plan a larger change, open an issue first with a short proposal.
- Coding style: follow existing conventions; keep APIs small and focused.

## Prerequisites

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) or later
- Git

Use the official [.NET CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/) to build, test and publish the project.
If you want, optional helper toolsets I built to facilitate these tasks are available:

- [Dotnet Tools](https://github.com/artur-rios/dotnet-tools)
- [Python Dotnet Tools](https://github.com/artur-rios/python-dotnet-tools)

## Build

```bash
dotnet build src/ArturRios.Extensions.sln
```

## Testing

The test suite is xUnit, and every test is named with the Given / When / Then pattern. Every test class
carries a `Category` trait, so the two kinds can be run — and reported — separately:

```bash
dotnet test src/ArturRios.Extensions.sln --filter "Category=Unit"
dotnet test src/ArturRios.Extensions.sln --filter "Category=Functional"
```

Unit tests exercise the code in isolation against test doubles.
Functional tests run the extensions together over real JSON files on disk and real console output.
CI runs the two as separate jobs, and both must pass before a pull request can be merged.

The unit job also fails when a test has no `Category` trait, since no job would run it, and when
`dotnet format --verify-no-changes` finds a file to reformat; run `dotnet format src/ArturRios.Extensions.sln` before pushing.

## Branching and pull requests

`develop` is the integration branch and the base for all new work; `main` only holds released code.

Branch off `develop` — `feature/<name>` for features, `fix/<name>` for fixes (`feat/`, `bugfix/`, `chore/`,
`refactor/`, `docs/`, `ci/`, `test/`, `perf/` and `build/` are accepted too) — and open a pull request back into
`develop`.

Dependabot's `dependabot/*` dependency-update branches are accepted into `develop` too.

Pull requests into `develop` and `main` must pass the tests and the branch policy check.

Commit messages follow [Conventional Commits](https://www.conventionalcommits.org/) with a lowercase subject, e.g.
`refactor: use prime logic from util package` or `chore: organize csproj properties`.

Record every change a package consumer would notice under `## [Unreleased]` in [CHANGELOG.md](./CHANGELOG.md), in the
same pull request that makes it.

## Versioning

The project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html). As a library, its contract is the
public API and the documented behavior of the `ArturRios.Extensions` package:

- **Major**: a change that can break a consumer — a public type or member removed, renamed or given a different
  signature, or documented behavior changed in a way callers may rely on. Its CHANGELOG entry carries an
  `### Upgrading from <X>.x to <Y>.0` subsection telling consumers what to change.
- **Minor**: new public API, or a behavior change existing callers do not have to adapt to, dependency updates
  included.
- **Patch**: a fix or tweak that leaves the public API and the documented behavior as they are.

The version number lives in one place, `<Version>` in `src/ArturRios.Extensions.csproj`. It is set on the release branch
(see [Releasing](#releasing)), the package is packed with it, and the branch policy check and the publish workflow
reject a release branch or tag whose version differs from it.

## Releasing

1. Cut `release/<version>` from `develop`, set `<Version>` in `src/ArturRios.Extensions.csproj` to that version,
   rename `## [Unreleased]` in [CHANGELOG.md](./CHANGELOG.md) to `## [<version>] - <yyyy-mm-dd>` above a fresh, empty
   `## [Unreleased]`, update the compare links at the bottom, and open a pull request into `main`. Only `release/*`
   branches can be merged into `main`.
2. Once it is merged, tag the merge commit on `main` with the version. Pushing the tag publishes the package to
   nuget.org and GitHub Packages:

   ```bash
   git switch main && git pull
   git tag <version> && git push origin <version>
   ```

   Tag with the bare version, e.g. `1.3.0`. Some older releases were tagged with a `v` prefix
   (`v1.1.0`); those tags stay as they are, but new tags drop the prefix.

3. Open a pull request from `main` into `develop` to bring the release back into the integration branch.

Only the repository owner can push version tags, and the publish workflow rejects tags that do not point at a commit on
`main` or whose version differs from the one in the csproj.
