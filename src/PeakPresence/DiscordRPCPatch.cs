using HarmonyLib;
using DiscordRPC;
using Photon.Pun;
using System;
using WebSocketSharp;
using AncestralMod;

namespace PeakPresence;

class DiscordRPCPatch
{


	[HarmonyPatch(typeof(RichPresenceService), "SetState")]
	[HarmonyPostfix]
	static void DiscordRPCPatchPostfix(RichPresenceService __instance)
	{
		UpdateDiscordRPC(__instance.m_currentState);
	}

	// private static Character Player => Character.localCharacter;
	
	// [HarmonyPatch(typeof(Character), "HandleDeath")]
	// [HarmonyPrefix]
	// static void HandleDeathPrefix(Character __instance)
	// {
	// 	if (__instance.data.sinceDied == 0f)
	// 	{
	// 		RichPresenceService? richPresence = GameHandler.GetService<RichPresenceService>();
	// 		if (richPresence != null) UpdateDiscordRPC(richPresence.m_currentState);
	// 	}
	// }
	
	// [HarmonyPatch(typeof(Character), "HandlePassedOut")]
	// [HarmonyPrefix]
	// static void HandlePassedOutPrefix(Character __instance)
	// {
	// 	if (__instance.data.lastPassedOut == 0f) {
	// 		RichPresenceService? richPresence = GameHandler.GetService<RichPresenceService>();
	// 		if (richPresence != null) UpdateDiscordRPC(richPresence.m_currentState);
	// 	}
	// }
	
	public static void UpdateDiscordRPC(RichPresenceState currentState) {
		if (Plugin.Client != null)
		{
			(string SmallImageKey, string SmallImageText, string Details) = Helper.GetCurrentStateContext(currentState);

			int RoomPlayerAmount = PhotonNetwork.InRoom ? PhotonNetwork.PlayerList.Length : 1;
			int MaxRoomPlayers = PhotonNetwork.CurrentRoom?.MaxPlayers ?? 1;

			string State = "";
			Party? Party = null;

			// if (Helper.IsOnIsland())
			// {
			// 	if (string.IsNullOrEmpty(State) && ConfigHandler.ShowAliveStatus.Value)
			// 	{
			// 		if (Player.data.dead) State = LocalizationManager.Get("status.dead");
			// 		else if (Player.data.passedOut) State = LocalizationManager.Get("status.passed_out");
			// 	}
			// 	if (string.IsNullOrEmpty(State) && ConfigHandler.ShowHeight.Value)
			// 	{
			// 		float? height = Player.refs.stats.heightInMeters;
			// 		if (height != null)
			// 		{
			// 			float heightValue = height.Value;
			// 			string unit = LocalizationManager.Get("progress.height.meters");
			// 			if (heightValue >= 1000f && ConfigHandler.AbbreviateHeight.Value)
			// 			{
			// 				heightValue /= 1000f;
			// 				unit = LocalizationManager.Get("progress.height.kilometers");
			// 			}
			// 			State = string.Format(LocalizationManager.Get(unit), heightValue.ToString("F2"));
			// 		}
			// 	}
			// }

			if (string.IsNullOrEmpty(State)) State = PhotonNetwork.OfflineMode ? LocalizationManager.Get("playing.solo") : PhotonNetwork.InRoom ? LocalizationManager.Get("playing.multiplayer") : "";
			if (!PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)
			{
				Party = new Party();
				Party.ID = PhotonNetwork.CurrentRoom?.Name ?? "";
				Party.Size = RoomPlayerAmount;
				Party.Max = MaxRoomPlayers;
			}

			Assets Assets = new Assets()
			{
				LargeImageKey = ConfigHandler.LogoImageKey.Value,
				LargeImageText = Helper.GetCurrentAscent() ?? "PEAK",
			};
			if (!SmallImageKey.IsNullOrEmpty()) Assets.SmallImageKey = SmallImageKey;
			if (!SmallImageText.IsNullOrEmpty()) Assets.SmallImageText = SmallImageText;

			Timestamps? Timestamps = null;
			float? currentTime = Helper.GetCurrentGameTime();
			if (currentTime != null)
				Timestamps = new Timestamps { Start = DateTime.UtcNow.AddSeconds(-currentTime.Value) };
			DiscordRPCQueue.SendRefresh(new DiscordRPCQueue.RefreshRequest(
				Details,
				State,
				Party,
				Assets,
				Timestamps,
				ActivityType.Playing
			));
		}
	}

}