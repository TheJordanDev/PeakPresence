using HarmonyLib;
using DiscordRPC;
using Photon.Pun;
using System;
using WebSocketSharp;

namespace PeakPresence;

class DiscordRPCPatch
{

	[HarmonyPatch(typeof(RichPresenceService), "SetState")]
	[HarmonyPostfix]
	static void DiscordRPCPatchPostfix(RichPresenceService __instance, RichPresenceState state)
	{
		UpdateDiscordRPC(state);
	}

	[HarmonyPatch(typeof(RichPresenceService), "Dirty")]
	[HarmonyPostfix]
	static void DiscordRPCPatchPostfix(RichPresenceService __instance)
	{
		UpdateDiscordRPC(__instance._presence.State);
	}

	
	public static void UpdateDiscordRPC(RichPresenceState currentState) {
		if (Plugin.Client != null)
		{
			(string SmallImageKey, string SmallImageText, string Details) = Helper.GetCurrentStateContext(currentState);

			int RoomPlayerAmount = PhotonNetwork.InRoom ? PhotonNetwork.PlayerList.Length : 1;
			int MaxRoomPlayers = PhotonNetwork.CurrentRoom?.MaxPlayers ?? 1;

			string State = "";
			Party? Party = null;

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