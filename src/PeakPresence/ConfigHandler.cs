using BepInEx.Configuration;

namespace PeakPresence;

public static class ConfigHandler
{
    public static ConfigFile config { get; private set; } = null!;

    public static ConfigEntry<string> DiscordAppID { get; private set; } = null!;

    public static ConfigEntry<string> ForcedLanguage { get; private set; } = null!;

    public static ConfigEntry<bool> UseDetailedDetails { get; private set; } = null!;

    public static ConfigEntry<string> LogoImageKey { get; private set; } = null!;
    public static ConfigEntry<string> ShoreImageKey { get; private set; } = null!;
    public static ConfigEntry<string> TropicsImageKey { get; private set; } = null!;
    public static ConfigEntry<string> RootsImageKey { get; private set; } = null!;
    public static ConfigEntry<string> AlpineImageKey { get; private set; } = null!;
    public static ConfigEntry<string> MesaImageKey { get; private set; } = null!;
    public static ConfigEntry<string> CalderaImageKey { get; private set; } = null!;
    public static ConfigEntry<string> KilnImageKey { get; private set; } = null!;
    public static ConfigEntry<string> PeakImageKey { get; private set; } = null!;
    public static ConfigEntry<string> GloomImageKey { get; private set; } = null!;
    public static ConfigEntry<string> CitadelImageKey { get; private set; } = null!;
    public static ConfigEntry<string> NadirImageKey { get; private set; } = null!;

    public static void Initialize(ConfigFile configFile)
    {
        config = configFile;


        DiscordAppID = config.Bind("General", "DiscordAppID", "1408478763682894045", "The Discord Application ID to use for Rich Presence.");

        ForcedLanguage = config.Bind("General", "ForcedLanguage", "", new ConfigDescription(
            "The language to use for forced localization.",
            new AcceptableValueList<string>(["", "en", "fr", "it", "de", "es", "es-419", "pt-BR", "ru", "uk", "zh-Hans", "zh-Hant", "ja", "ko", "pl", "tr"])
        ));

        UseDetailedDetails = config.Bind("General", "UseDetailedDetails", false, "Whether to use detailed presence information instead of \"In Game: {location}\"");

        LogoImageKey = config.Bind("Images", "LogoImageKey", "logo", "The key for the logo image.");
        ShoreImageKey = config.Bind("Images", "ShoreImageKey", "shore", "The key for the shore image.");
        TropicsImageKey = config.Bind("Images", "TropicsImageKey", "tropics", "The key for the tropics image.");
        RootsImageKey = config.Bind("Images", "RootsImageKey", "roots", "The key for the roots image.");
        AlpineImageKey = config.Bind("Images", "AlpineImageKey", "alpine", "The key for the alpine image.");
        MesaImageKey = config.Bind("Images", "MesaImageKey", "mesa", "The key for the mesa image.");
        CalderaImageKey = config.Bind("Images", "CalderaImageKey", "caldera", "The key for the caldera image.");
        KilnImageKey = config.Bind("Images", "KilnImageKey", "kiln", "The key for the kiln image.");
        PeakImageKey = config.Bind("Images", "PeakImageKey", "peak", "The key for the peak image.");
        GloomImageKey = config.Bind("Images", "GloomImageKey", "gloom", "The key for the gloom image.");
        CitadelImageKey = config.Bind("Images", "CitadelImageKey", "citadel", "The key for the citadel image.");
        NadirImageKey = config.Bind("Images", "NadirImageKey", "nadir", "The key for the nadir image.");
    }
}
