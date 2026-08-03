# Changelog

All notable changes to this package are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-08-03

### Added

- Initial release as a UPM package. The sources previously lived under
  `Assets/Scripts/WeaponSystem` in the Wings of Wrath project and were extracted unchanged; all `.meta`
  GUIDs were preserved, so scene, prefab and `.asmdef` references in projects migrating off the in-project
  copy keep resolving.
- `package.json` with `license`, `repository`, `documentationUrl`, `changelogUrl`, `licensesUrl` and a
  `testables` entry exposing the package's edit-mode tests in the Test Runner.
- README with installation instructions, a weapon-by-weapon guide and an API reference.
- MIT `LICENSE.md`. The code previously had no license, which left it all-rights-reserved.

[1.0.0]: https://github.com/NoumanWaheed93/WeaponSystem/releases/tag/v1.0.0
