using AncestralMod;
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
        
        // var queueGO = new UnityEngine.GameObject("DiscordRPCQueue");
        // DontDestroyOnLoad(queueGO);
        // queueGO.AddComponent<DiscordRPCQueue>();
        
        Client = new DiscordRpcClient(ConfigHandler.DiscordAppID.Value);

        Client.OnReady += (sender, e) =>
        {
            Log.LogInfo($"Connected to discord with user {e.User.Username}");
        };

        Client.Initialize();

        LocalizedText.OnLangugageChanged += LocalizationManager.OnLanguageChanged;

        _harmony ??= new Harmony(Info.Metadata.GUID);
        _harmony.PatchAll(typeof(DiscordRPCPatch));

        // try
        // {
        //     InvokeRepeating(nameof(PeriodicUpdateDiscordRPC), 15f, 15f);
        // }
        // catch (Exception ex)
        // {
        //     Log.LogWarning($"Failed to start periodic RPC updater: {ex.Message}");
        // }
    }

    // private void PeriodicUpdateDiscordRPC()
    // {
    //     try
    //     {
    //         var richPresence = GameHandler.GetService<RichPresenceService>();
    //         if (richPresence != null)
    //         {
    //             DiscordRPCPatch.UpdateDiscordRPC(richPresence.m_currentState);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Log.LogWarning($"PeriodicUpdateDiscordRPC failed: {ex.Message}");
    //     }
    // }

    private void OnDestroy()
    {
        // try
        // {
        //     CancelInvoke(nameof(PeriodicUpdateDiscordRPC));
        // }
        // catch { }

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
