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
		if (Plugin.Client != null)
		{
			(string SmallImageKey, string SmallImageText, string Details) = Helper.GetCurrentStateContext(__instance);

			int RoomPlayerAmount = PhotonNetwork.InRoom ? PhotonNetwork.PlayerList.Length : 1;
			int MaxRoomPlayers = PhotonNetwork.CurrentRoom?.MaxPlayers ?? 1;

			string State = "";
			Party Party = new Party();

			if (__instance.m_currentState != RichPresenceState.Status_MainMenu)
			{
				if (PhotonNetwork.OfflineMode) State = LocalizationManager.Get("playing.solo");
				else if (PhotonNetwork.InRoom)
				{
					State = LocalizationManager.Get("playing.multiplayer");
					Party.ID = PhotonNetwork.CurrentRoom?.Name ?? "";
					Party.Size = RoomPlayerAmount;
					Party.Max = MaxRoomPlayers;
				}
			}

			Assets Assets = new Assets()
			{
				LargeImageKey = ConfigHandler.LogoImageKey.Value,
				LargeImageText = Helper.GetCurrentAscent() ?? "PEAK",
			};
			if (!SmallImageKey.IsNullOrEmpty()) Assets.SmallImageKey = SmallImageKey;
			if (!SmallImageText.IsNullOrEmpty()) Assets.SmallImageText = SmallImageText;

			Timestamps Timestamps = new Timestamps();
			float? currentTime = Helper.GetCurrentGameTime();
			if (currentTime != null) Timestamps.Start = DateTime.UtcNow.AddSeconds(-currentTime.Value);
			Plugin.Client.ClearPresence();
			Plugin.Client.SetPresence(new RichPresence()
			{
				Details = Details,
				State = State,
				Party = Party,
				Assets = Assets,
				Timestamps = Timestamps,
				Type = ActivityType.Playing,
			});
			Plugin.Client.Invoke();
		}
	}



}