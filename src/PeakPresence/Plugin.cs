using BepInEx;
using BepInEx.Logging;
using System;
using DiscordRPC;
using HarmonyLib;

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

        var queueGO = new UnityEngine.GameObject("DiscordRPCQueue");
        DontDestroyOnLoad(queueGO);
        queueGO.AddComponent<DiscordRPCQueue>();

        try
        {
            Client = new DiscordRpcClient(ConfigHandler.DiscordAppID.Value);

            Client.OnReady += (sender, e) =>
            {
                Log.LogInfo($"Connected to discord with user {e.User.Username}");
            };

            Client.Initialize();
        }
        catch (Exception ex)
        {
            Log.LogWarning($"Failed to initialize Discord Rich Presence client: {ex.Message}");
        }

        LocalizedText.OnLangugageChanged += LocalizationManager.OnLanguageChanged;

        _harmony ??= new Harmony(Info.Metadata.GUID);
        _harmony.PatchAll(typeof(DiscordRPCPatch));

    }

    private void OnDestroy()
    {
        try
        {
            Client?.Dispose();
        }
        catch (Exception ex)
        {
            Log.LogWarning($"Error disposing Discord client: {ex.Message}");
        }
    }
}
