using AncestralMod;
using BepInEx;
using BepInEx.Logging;
using DiscordRPC;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace PeakPresence;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{

    public static Plugin Instance { get; private set; } = null!;
    internal static ManualLogSource Log { get; private set; } = null!;
    private static Harmony? _harmony;

    public static DiscordRpcClient Client { get; private set; } = null!;

    private void Awake()
    {
        Instance = this;
        Log = Logger;
        Log.LogInfo($"Plugin {Name} is loaded!");

        ConfigHandler.Initialize(Config);
        
        Client = new DiscordRpcClient(ConfigHandler.DiscordAppID.Value);

        Client.OnReady += (sender, e) =>
        {
            Log.LogInfo($"Connected to discord with user {e.User.Username}");
        };

        Client.Initialize();

        LocalizedText.OnLangugageChanged += LocalizationManager.OnLanguageChanged;

        _harmony ??= new Harmony(Info.Metadata.GUID);
        _harmony.PatchAll(typeof(DiscordRPCPatch));
    }

    public void Destroy()
    {
        Client?.Dispose();
    }
}
