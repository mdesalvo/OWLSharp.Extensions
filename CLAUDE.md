# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

OWLSharp.Extensions is a set of extension libraries for [OWLSharp](https://github.com/mdesalvo/OWLSharp) that make it more convenient to model, validate and reason over ontologies in three LinkedData domains, each isolated in its own namespace/folder under `OWLSharp.Extensions/`:

- **SKOS** (`OWLSharp.Extensions.SKOS`) — controlled vocabularies / concept schemes (W3C SKOS + SKOS-XL).
- **GEO** (`OWLSharp.Extensions.GEO`) — spatial features (GeoSPARQL), backed by NetTopologySuite/ProjNet.
- **TIME** (`OWLSharp.Extensions.TIME`) — temporal features and Allen relations (OWL-TIME).

All three follow the same shape: a static `<Domain>Helper` class exposing extension methods on `OWLOntology` (declare A-BOX axioms, query/analyze), plus (for SKOS and TIME) a `<Domain>Validator` and (for TIME) a `<Domain>Reasoner`, each built from a pluggable rule set.

## Build & test

```bash
dotnet build -c Release
dotnet test -c Release
```

Run a single test class or method (MSTest):

```bash
dotnet test --filter "FullyQualifiedName~SKOSHelperTest"
dotnet test --filter "FullyQualifiedName~GEOHelperTest.ShouldGetDistanceBetweenFeatures"
```

The main library multi-targets `netstandard2.0;net8.0`; the test project targets `net10.0`. CI (`.github/workflows/linux.yml`, `windows.yml`) builds Release and runs `dotnet test` on both Linux (dotnet 10) and Windows (dotnet 8) against every push to `master`; Linux also collects coverage via Codecov.

Code targeting `netstandard2.0` cannot assume `Parallel.ForEachAsync` or other net8-only APIs — validator/reasoner code guards this with `#if !NET8_0_OR_GREATER` blocks (see below).

## Architecture

### Helper classes (`<Domain>Helper.cs`)

Each helper is a `static class` of extension methods on `OWLOntology`, organized under `#region`s:

- **Helper (Initializer, Declarer[, Getter])** — `InitializeXAsync` imports the domain's T-BOX ontologies (dereferenced from their canonical URIs, e.g. `RDFVocabulary.SKOS.DEREFERENCE_URI`) with configurable timeout/cache; `DeclareX` methods inject A-BOX axioms (class assertions, object/data property assertions) for domain entities. Declare methods validate arguments and throw `OWLException` on invalid input.
- **Analyzer** — read-only query methods (`CheckHasX`, `GetX...`) that traverse the ontology's assertions/graph to answer domain questions (e.g. SKOS broader/narrower closure, GEO distance/direction/containment between features, TIME instant/interval coordinates and extents).
- **Utilities** — internal helpers backing the above (WKT/GML conversion via NTS in GEO, SWRL-relation traversal in SKOS/TIME).

GEO and TIME model entities as dedicated value types rather than raw RDF: `GEOEntity`/`GEOPoint`/`GEOLine`/`GEOArea` (`GEO/GEOEntity.cs`) wrap an NTS `Geometry` and know how to serialize themselves to WKT/GML; `TIMEInstant`/`TIMEInterval`/`TIMECoordinate`/`TIMEExtent`/`TIMEUnit` (`TIME/Abstractions/`) model temporal positions/durations across reference systems, with `TIMEConverter` doing position↔coordinate and calendar-TRS conversions.

### Validators (SKOS, TIME) and the Reasoner (TIME)

`SKOSValidator` / `TIMEValidator` / `TIMEReasoner` all share one pattern:

- A `Rules` list of an enum (`SKOSEnums.SKOSValidatorRules`, `TIMEEnums.TIMEValidatorRules`, `TIMEEnums.TIMEReasonerRules`); a `Default` static instance runs every rule in the enum.
- `ApplyToOntologyAsync(ontology)` fans the selected rules out in parallel (`Parallel.ForEachAsync` on net8+, `Dasync.Collections.ParallelForEachAsync` otherwise — this is the reason for the `#if !NET8_0_OR_GREATER` guard), collects results into a per-rule dictionary, then flattens to `List<OWLIssue>` (validators) or `List<OWLInference>` (reasoner).
- Each individual rule lives in its own `internal static class` under a `RuleSet/` subfolder (`SKOS/Validator/RuleSet/`, `TIME/Validator/RuleSet/`, `TIME/Reasoner/RuleSet/`), named `<Rule>AnalysisRule` (validators) or `<Rule>EntailmentRule` (reasoner), sharing the parent namespace (no extra `using` needed). Validator rules expose `internal static Task<List<OWLIssue>> ExecuteRuleAsync(OWLOntology)`; reasoner rules build a `SWRLRule` (antecedent/consequent atoms + built-ins) and delegate to `swrlRule.ApplyToOntologyAsync(ontology)`.

**To add a new rule**: create the rule class in the appropriate `RuleSet/` folder following an existing sibling as a template, add the corresponding enum value to `SKOSEnums`/`TIMEEnums`, and wire a `case` for it into the `switch` in `SKOSValidator`/`TIMEValidator`/`TIMEReasoner`.

GEO currently has no validator/reasoner counterpart — only the `GEOHelper` declare/analyze surface.

### Namespace boundary

Everything for a domain stays inside its own top-level folder/namespace (`OWLSharp.Extensions.SKOS`, `.GEO`, `.TIME`); there is no cross-domain coupling between SKOS/GEO/TIME — each maps directly to one W3C/OGC spec and depends only on `OWLSharp` (+ NetTopologySuite/ProjNet for GEO).
