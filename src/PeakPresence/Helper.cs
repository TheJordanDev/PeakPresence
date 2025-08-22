using AncestralMod;
using UnityEngine.SceneManagement;

namespace PeakPresence;

public static class Helper
{

	public static string UppercaseFirst(string s)
	{
		if (string.IsNullOrEmpty(s)) return string.Empty;
		return char.ToUpper(s[0]) + s[1..];
	}

	public static bool IsOnIsland()
	{
		return SceneManager.GetActiveScene().name.ToLower().StartsWith("level_") || SceneManager.GetActiveScene().name == "WilIsland";
	}

	public static string GetStateSmallImageKey(RichPresenceState state)
	{
		return state switch
		{
			RichPresenceState.Status_Shore => ConfigHandler.ShoreImageKey.Value,
			RichPresenceState.Status_Tropics => ConfigHandler.TropicsImageKey.Value,
			RichPresenceState.Status_Alpine => ConfigHandler.AlpineImageKey.Value,
			RichPresenceState.Status_Mesa => ConfigHandler.MesaImageKey.Value,
			RichPresenceState.Status_Caldera => ConfigHandler.CalderaImageKey.Value,
			RichPresenceState.Status_Kiln => ConfigHandler.KilnImageKey.Value,
			RichPresenceState.Status_Peak => ConfigHandler.PeakImageKey.Value,
			_ => ""
		};
	}

	public static string GetStateSmallImageText(RichPresenceState state)
	{
		return state switch
		{
			RichPresenceState.Status_MainMenu => LocalizationManager.Get("main_menu"),
			RichPresenceState.Status_Airport => LocalizationManager.Get("airport"),
			RichPresenceState.Status_Shore => LocalizationManager.GetVanilla("SHORE"),
			RichPresenceState.Status_Tropics => LocalizationManager.GetVanilla("TROPICS"),
			RichPresenceState.Status_Alpine => LocalizationManager.GetVanilla("ALPINE"),
			RichPresenceState.Status_Mesa => LocalizationManager.GetVanilla("MESA"),
			RichPresenceState.Status_Caldera => LocalizationManager.GetVanilla("CALDERA"),
			RichPresenceState.Status_Kiln => LocalizationManager.GetVanilla("THE KILN"),
			RichPresenceState.Status_Peak => LocalizationManager.GetVanilla("PEAK"),
			_ => "unknown"
		};
	}


	public static (string Key, string Text, string Details) GetCurrentStateContext(RichPresenceService __instance)
	{

		string Key = GetStateSmallImageKey(__instance.m_currentState);

		string Text = GetStateSmallImageText(__instance.m_currentState);
		Text = UppercaseFirst(Text.ToLower());

		string Details = LocalizationManager.Get("ingame");
		Details = Details.Replace("{1}", Text);

		return (Key, Text, Details);

	}

	public static float? GetCurrentGameTime()
	{
		return IsOnIsland() ? RunManager.Instance?.timeSinceRunStarted : null;
	}

	public static string? GetCurrentAscent()
	{
		if (!IsOnIsland()) return null;
		string ascent = Ascents._currentAscent switch
		{
			-1 => LocalizationManager.GetVanilla("TENDERFOOT"),
			0 => LocalizationManager.GetVanilla("PEAK"),
			_ => LocalizationManager.GetVanilla($"ASCENT {Ascents._currentAscent}")
		};
		return UppercaseFirst(ascent.ToLower());
	}
}