# Changelog

## [0.5] - 05/11/2025

- Added support for the new biome : The ROOTS
- Added option to show height instead of "Playing Solo / Multiplayer" with the option to abbreviate meters to km when above 1000m
- Updated translations to include new height strings ("progress.height.meters" and "progress.height.kilometers")

## [0.4] - 29/08/2025

### Reverted

- Removed the Photon MAX_PLAYER offset from patch 0.3 as the devs patched out their ""dev slot"" 🎉🥳

## [0.3] - 26/08/2025

### Fixed

- Corrected Photon room MaxPlayers in Discord RPC by subtracting the hidden dev slot (shows real 4-player limit *or more if using [PeakUnlimited](https://thunderstore.io/c/peak/p/glarmer/PEAK_Unlimited/)* instead of 5).

## [0.2] - 23/08/2025

### Added

- Detailed messages for each states like "Surviving the crash at the Shore" instead of "InGame: Shore"

## [0.1] - 23/08/2025

### Added

- RPC with localization [ English, French, Italian, German, SpanishSpain, SpanishLatam, BRPortuguese, Russian, Ukrainian, SimplifiedChinese, TraditionalChinese, Japanese, Korean, Polish, Turkish ]
- Config for RPC to use another one
- Player can configure ForcedLanguage to force the RPC to use a specific Language Code instead of Syncing with Game language [ "en", "fr", "it", "de", "es", "es-419", "pt-BR", "ru", "uk", "zh-Hans", "zh-Hant", "ja", "ko", "pl", "tr" ]