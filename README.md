# PeakPresence

PeakPresence is a mod for PEAK that adds Discord Rich Presence integration, allowing your Discord friends to see your current in-game status, location, and activity.

## Features

- Discord Rich Presence support
- Shows current biome/location (e.g., Shore, Tropics, Alpine, etc.)
- Displays solo or multiplayer status
- Customizable and localizable presence text
- Language support via `l10n` files

### Disclaimer
All translations (other than English and French) were AI-generated. You are welcome to make a Github Pull Request if you want to submit more accurate translations.

## Installation

1. **Requirements:**
   - [BepInEx 5](https://thunderstore.io/c/peak/p/BepInEx/BepInExPack_PEAK/)

2. **Install the Mod:**
   - Download the latest release from [Thunderstore](https://thunderstore.io/c/peak/p/TheJordanDev/PeakPresence/).
   - Extract the contents to your `BepInEx/plugins/PeakPresence/` directory.

3. **Localization:**
   - Edit or add language files in `BepInEx/plugins/PeakPresence/l10n/` (e.g., `en.json`).
   - The mod will automatically use the language set in-game.

## Configuration

- The mod generates a config file on first launch.
- You can customize Rich Presence text and behavior via the config file or by editing localization files for full language support.
- You can configure ForcedLanguage to force the RPC to use a specific Language Code instead of Syncing with Game language ("en", "fr", "it", "de", "es", "es-419", "pt-BR", "ru", "uk", "zh-Hans", "zh-Hant", "ja", "ko", "pl", "tr")

## Building

To build the project yourself:

```sh
dotnet build -c Release
```

### Thunderstore Packaging

To package for Thunderstore:

```sh
dotnet build -c Release -target:PackTS -v d
```

The built package will be found at `artifacts/thunderstore/`.

## Contributing

Pull requests and issues are welcome! Please open an issue for bugs or feature requests.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.