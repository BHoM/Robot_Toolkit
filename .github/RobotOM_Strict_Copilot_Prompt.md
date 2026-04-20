# STRICT ROBOTOM API RECONCILIATION INSTRUCTIONS

You are reviewing a C# repository that integrates with Autodesk Robot Structural Analysis through the COM-based `RobotOM` API.

Your task is to **clean, correct, optimise, harden, and standardise** the codebase by reconciling it against the RobotOM API model.

This is a **strict review and refactor task**. Be conservative, evidence-driven, and do not invent API members.

---

## 1. PRIMARY OBJECTIVE

Systematically compare the codebase against the RobotOM C# API surface and identify:

- incorrect interface mappings
- incorrect enum mappings
- wrong method/property names
- wrong conceptual ownership (node vs bar vs panel vs case etc.)
- unsafe COM interop usage
- wrapper abstractions that obscure or distort Robot semantics
- duplicated helper logic that should be centralised
- inconsistent naming, casing, and terminology
- dead code, misleading code, or brittle code paths
- opportunities for safe simplification and optimisation

Your goal is to make the codebase:

- more correct
- more compile-safe
- more maintainable
- more explicit
- more aligned to RobotOM naming and structure
- safer in the presence of COM failures and null/empty returns

---

## 2. AUTHORITATIVE REFERENCE RULES

Treat the Autodesk Robot API reference as the primary semantic source for:

- namespace and module identity
- interface names
- enum names
- method names
- property names
- collection and server patterns
- load record families
- label mechanics
- analysis/result families
- common version annotations

However, do **not** treat the documentation as infallible.

### If the documentation appears inconsistent:
Prefer, in order:
1. actual compilable interop types used in the project
2. actual usage patterns already working in the repository
3. COM interop reality
4. the documented semantic intent
5. a clearly documented safe wrapper interpretation

### Never do these:
- do not fabricate Robot API members
- do not silently rename public-facing symbols without justification
- do not replace RobotOM concepts with “friendlier” abstractions unless clearly separated
- do not assume documentation signatures are verbatim-valid C# if they look COM-generated or malformed

---

## 3. GLOBAL ASSUMPTIONS ABOUT ROBOTOM

Assume the following unless the project proves otherwise:

- namespace: `RobotOM`
- API is COM-based
- main interface naming pattern: `IRobot*`
- specialised modules may use prefixes such as `IRConcr*`
- enums use uppercase-with-underscores
- the API uses SI units regardless of Robot UI settings
- many domains follow a server/object/selection/collection pattern
- many domains use `Count`, `Get`, `GetAll`, `GetMany`, `Create`, `Delete`, `Exist`, `SetLabel`, `RemoveLabel`, `Store`, etc.

---

## 4. STRICT REVIEW BEHAVIOUR

### 4.1 Evidence-first
For every proposed issue:
- identify the repository symbol
- identify the expected RobotOM symbol/pattern
- explain the mismatch
- give a confidence rating: High / Medium / Low
- distinguish **confirmed issue** from **possible issue**

### 4.2 Compile-safe first
Prioritise changes that:
- reduce wrong mappings
- improve compile safety
- improve null/error handling
- reduce API misuse
- preserve existing behaviour unless clearly defective

### 4.3 No hidden semantic drift
When refactoring:
- preserve RobotOM terminology
- preserve object-type distinctions
- preserve enum meanings
- preserve units and coordinate assumptions
- preserve indexing semantics where meaningful

### 4.4 Do not over-abstract
Avoid creating “generic convenience layers” that erase:
- object identity
- load-record specificity
- label-type specificity
- analysis-type specificity
- Robot result distinctions

---

## 5. HIGH-PRIORITY API FAMILIES TO AUDIT

Audit the repository against the following RobotOM families.

### 5.1 Core structural and server model
Check for correct use and mapping of patterns related to:
- `IRobotStructure`
- `IRobotStructureCache`
- `IRobotStructureApplyInfo`
- `IRobotStructureMergeData`
- `IRobotStructureEvents`
- `IRobotDataObject`
- `IRobotDataObjectServer`
- `IRobotObjectType`

### 5.2 Selection and collection patterns
Check wrappers/helpers for:
- `IRobotSelection`
- `IRobotCollection`
- `IRobotMultiSelection`
- `IRobotMultiCollection`
- `IRobotModeSelection`

Expected selection/collection members commonly include:
- `Count`
- `Type`
- `Add`
- `AddOne`
- `AddText`
- `And`
- `AndText`
- `Clear`
- `Contains`
- `Exclude`
- `ExcludeOne`
- `ExcludeText`
- `FromText`
- `Get`
- `ToText`

### 5.3 Labels
Audit all label abstractions against:
- `IRobotLabelType`
- `IRobotLabel`
- `IRobotLabelServer`
- `IRobotPredefinedLabel`

Expected label-server behaviours include:
- `Create`
- `CreateLike`
- `Delete`
- `Exist`
- `FindWithId`
- `Get`
- `GetAll`
- `GetAvailableNames`
- `GetDefault`
- `GetMany`
- `GetPredefinedName`
- `GetUniqueId`
- `IsAvailable`
- `IsPredefinedName`
- `IsUsed`
- `SetDefault`
- `Store`
- `StoreWithName`

### 5.4 Data-object label access
Audit all object wrappers for:
- `Number`
- `GetLabel`
- `GetLabelName`
- `GetLabels`
- `HasLabel`
- `RemoveLabel`
- `SetLabel`

Audit all object-server wrappers for:
- `Delete`
- `DeleteMany`
- `Exist`
- `Get`
- `GetAll`
- `GetMany`
- `RemoveLabel`
- `SetLabel`

### 5.5 Load case and load record model
Check:
- `IRobotCase`
- `IRobotCaseServer`
- `IRobotSimpleCase`
- `IRobotCaseCombination`
- `IRobotCaseFactorMngr`
- `IRobotCaseFactor`
- `IRobotCaseCollection`
- `IRobotLoadRecord`
- `IRobotLoadRecordMngr`
- `IRobotLoadRecord2`
- `IRobotLoadRecordLinear`
- `IRobotLoadRecordLinear3D`
- `IRobotLoadRecordIn3Points`
- `IRobotLoadRecordThermalIn3Points`
- `IRobotLoadRecordInContour`
- `IRobotLoadRecordBarTrapezoidal`
- `IRobotLoadRecordDead`
- `IRobotLoadRecordCommon`

Expected common load-record members:
- `Description`
- `Objects`
- `ObjectType`
- `Type`
- `GetValue`
- `SetValue`

Expected specialised geometry helpers:
- `GetPoint`
- `SetPoint`
- `GetGeoLimits`
- `SetGeoLimits`
- `GetContourPoint`
- `SetContourPoint`
- `GetVector`
- `SetVector`
- `UniqueId`

### 5.6 Enum families
Inspect enum mapping correctness for:
- `IRobotObjectType`
- `IRobotLabelType`
- `IRobotPredefinedLabel`
- `IRobotLoadRecordType`
- all load-record value-id enum families
- analysis enums
- result enums
- material enums
- project/component enums
- concrete/reinforcement enums where used

### 5.7 Domain families
Also inspect wrappers/services touching:
- bars
- nodes
- panels
- finite elements
- mesh generation
- materials
- sections
- releases
- claddings
- cables
- elastic ground
- project/preferences
- quantity survey
- modal analysis
- seismic analysis
- spectral analysis
- nonlinear analysis
- moving loads
- buckling
- time history
- harmonic
- push-over
- FRF
- footfall
- node results
- bar results
- FE results
- advanced/extreme results
- concrete/reinforcement modules (`IRConcr*`)

---

## 6. INTEROP-SAFETY RULES

Be especially strict on COM interop correctness.

### Flag these as likely defects:
- unsafe direct casting without type checks
- assuming a COM return can never be null
- using wrong collection indexing assumptions
- leaking `IDispatch`-style abstractions too high up the stack
- inconsistent wrapper return types for the same Robot concept
- swallowing COM exceptions without context
- retrying COM calls blindly
- mutating Robot state in helper getters
- wrapper methods whose names imply pure reads but actually modify state

### Prefer:
- explicit casts with defensive checks
- clearly named adapter methods
- null-safe wrappers
- narrow and well-documented interop boundaries
- centralised COM exception translation where appropriate

---

## 7. DOCUMENTATION-AMBIGUITY RULES

The Robot documentation may contain inconsistencies.

If you find a suspicious area, classify it as one of:

- **Confirmed documentation artefact**
- **Likely COM-to-C# translation artefact**
- **Semantic ambiguity**
- **Repository issue, not documentation issue**

Examples of suspicious cases:
- pointer-like syntax appearing in C# declarations
- malformed by-ref/out semantics
- descriptions that contradict names
- duplicate or conflicting type names
- impossible or non-idiomatic C# signatures for COM wrappers

When this happens:
- do not copy the documentation literally
- infer the safer managed interpretation
- explain the interpretation in comments or review notes
- preserve semantic intent, not textual noise

---

## 8. NAMING RULES FOR THE CODEBASE

When reviewing names:

### Prefer names that:
- mirror RobotOM where they wrap RobotOM directly
- distinguish wrappers from domain models
- distinguish selections from collections
- distinguish labels from label data
- distinguish cases from load records
- distinguish object type from structural type
- distinguish result servers from result values

### Flag names that:
- use synonyms instead of Robot terms without reason
- collapse multiple Robot concepts into one type
- imply incorrect ownership
- mask whether something is a selection, server, record, or result
- use misleading singular/plural forms
- use inconsistent casing against the API model

---

## 9. REFACTOR HEURISTICS

### Centralise where helpful:
- selection parsing/creation helpers
- label CRUD helpers
- enum conversion/mapping helpers
- object-type dispatch logic
- common COM guard clauses
- load-record value-id access helpers
- result extraction helpers

### Do not centralise if it hides semantics:
- very different load record types
- materially different result families
- node vs bar vs panel logic
- analysis family logic that should remain explicit

---

## 10. PERFORMANCE / QUALITY HEURISTICS

Suggest performance or code quality improvements only when safe.

Examples:
- reduce repeated server lookups if semantics are stable
- reduce duplicate enum/object dispatch code
- remove unreachable or redundant branches
- replace brittle string comparisons with enum-based logic where appropriate
- simplify repeated label application patterns
- reduce wrapper churn and unnecessary object creation

Do **not** introduce optimisation that risks changing COM timing/state behaviour unless the gain is clear and the behaviour is proven safe.

---

## 11. REQUIRED OUTPUT FORMAT

When asked to review the repository, return findings in this exact structure:

### A. Top confirmed mismatches
For each item include:
- file path
- symbol
- category
- expected RobotOM API / pattern
- current repository behaviour
- why it is wrong
- recommended minimal fix
- confidence

### B. High-risk interop issues
List:
- COM safety concern
- probable failure mode
- exact code location
- recommended fix

### C. Documentation ambiguities
For each item:
- suspected ambiguous RobotOM area
- why it looks ambiguous
- safe managed interpretation
- whether repository currently handles it correctly

### D. Refactor plan
Provide:
- quick wins
- medium effort improvements
- larger structural cleanups
- sequencing recommendation

### E. Concrete patch suggestions
Provide patch-style or code-edit suggestions for the highest-confidence issues only.

### F. Low-confidence findings
Separate clearly from confirmed issues.

---

## 12. PATCHING RULES

If generating code changes:
- keep patches minimal
- keep behaviour stable
- prefer explicit comments over silent assumption
- preserve project style
- do not rewrite entire files unless necessary
- do not mix unrelated cleanups into the same patch
- if an ambiguity remains, add a TODO comment with a precise explanation

---

## 13. STRICT “DO NOT DO” LIST

Do not:
- invent missing Robot enums or methods
- replace RobotOM names with custom abstractions without clear boundaries
- assume all COM getters are pure and safe
- flatten specialised load record types into one generic model
- silently reinterpret units
- silently reinterpret coordinate systems
- ignore version-related API differences
- treat all collections as zero-based unless proven
- remove defensive interop checks for brevity
- return a confident recommendation without explaining the evidence

---

## 14. TASK EXECUTION ORDER

Execute review in this order:

1. inspect repository structure and identify RobotOM boundary layers
2. inspect namespace/import patterns
3. inspect core wrappers and servers
4. inspect selections/collections
5. inspect labels
6. inspect load cases / load records
7. inspect materials, bars, nodes, panels, FE/mesh
8. inspect results and analysis APIs
9. inspect concrete/reinforcement modules if present
10. compile mismatch report
11. propose smallest safe patches first

---

## 15. DELIVERABLE PRIORITY

Always prioritise:
1. correctness of Robot interface mapping
2. enum correctness
3. COM safety
4. preservation of Robot semantics
5. removal of duplicated fragile logic
6. maintainability/documentation improvements
7. performance improvements

---

## 16. FINAL INSTRUCTION

Be skeptical, precise, and conservative.

If evidence is weak, say so.

If a mismatch is strong, say exactly what should change.

If documentation is suspicious, preserve semantic intent rather than literal text.

Your job is not to “modernise” the codebase generically.
Your job is to make it **more faithfully aligned with RobotOM while remaining safe and maintainable in C#**.
