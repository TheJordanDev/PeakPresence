using System;
using System.Collections;
using UnityEngine;
using DiscordRPC;

namespace PeakPresence;

public class DiscordRPCQueue : MonoBehaviour
{
	public static DiscordRPCQueue? Instance { get; private set; }

	public float MinIntervalSeconds = 15f;

	private RefreshRequest? _pendingRequest = null;
	private bool _isProcessing = false;
	private float _lastSentTime = -9999f;
	private RefreshRequest? _lastSentRequest = null;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(this);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public bool canSend()
	{
		return !_isProcessing && Time.realtimeSinceStartup - _lastSentTime >= MinIntervalSeconds;
	}

	public static void SendRefresh(RefreshRequest request)
	{
		if (Instance == null)
		{
			Debug.LogWarning("DiscordRPCQueue: no instance available to send refresh");
			return;
		}
		Instance.InternalEnqueue(request);
	}

	private void InternalEnqueue(RefreshRequest request)
	{
		_pendingRequest = request; // coalesce to latest

		// If we're not already processing and enough time has passed, send immediately.
		float now = Time.realtimeSinceStartup;
		float since = now - _lastSentTime;
		if (!_isProcessing && since >= MinIntervalSeconds)
		{
			var req = _pendingRequest.Value;
			_pendingRequest = null; // consumed
			try
			{
				if (Plugin.Client != null)
				{
					// skip sending if identical to last sent
					if (_lastSentRequest.HasValue && AreRequestsEqual(req, _lastSentRequest.Value))
					{
						return; // nothing to do
					}

					var presence = new RichPresence()
					{
						Details = req.Details,
						State = req.State,
						Party = req.Party,
						Assets = req.Assets,
						Timestamps = req.Timestamps,
						Type = req.Type
					};
					Plugin.Client.SetPresence(presence);
					_lastSentTime = Time.realtimeSinceStartup;
					_lastSentRequest = req;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning($"DiscordRPCQueue: immediate send failed: {ex.Message}");
			}

			return;
		}

		if (!_isProcessing)
		{
			StartCoroutine(ProcessQueue());
		}
	}

	private IEnumerator ProcessQueue()
	{
		_isProcessing = true;
		while (_pendingRequest.HasValue)
		{
			var req = _pendingRequest.Value;
			_pendingRequest = null; // clear so new updates can be coalesced

			// wait remaining time if needed
			float now = Time.realtimeSinceStartup;
			float since = now - _lastSentTime;
			if (since < MinIntervalSeconds)
			{
				yield return new WaitForSeconds(MinIntervalSeconds - since);
			}

			try
			{
				if (Plugin.Client != null)
				{
					// skip sending if identical to last sent
					if (_lastSentRequest.HasValue && AreRequestsEqual(req, _lastSentRequest.Value))
					{
						// nothing to do
					}
					else
					{
						var presence = new RichPresence()
						{
							Details = req.Details,
							State = req.State,
							Party = req.Party,
							Assets = req.Assets,
							Timestamps = req.Timestamps,
							Type = req.Type
						};
						Plugin.Client.SetPresence(presence);
						_lastSentTime = Time.realtimeSinceStartup;
						_lastSentRequest = req;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning($"DiscordRPCQueue: failed to send presence: {ex.Message}");
			}

			// small pause to allow new requests to set _pendingRequest
			yield return null;
		}

		_isProcessing = false;
	}

	private bool AreRequestsEqual(RefreshRequest a, RefreshRequest b)
	{
		if (!string.Equals(a.Details, b.Details)) return false;
		if (!string.Equals(a.State, b.State)) return false;
		if (a.Type != b.Type) return false;

		// Party comparison
		if (a.Party == null && b.Party != null) return false;
		if (a.Party != null && b.Party == null) return false;
		if (a.Party != null && b.Party != null)
		{
			if (!string.Equals(a.Party.ID, b.Party.ID)) return false;
			if (a.Party.Size != b.Party.Size) return false;
			if (a.Party.Max != b.Party.Max) return false;
		}

		return true;
	}


    public struct RefreshRequest
    {
        public string Details;
        public string State;
        public Party? Party;
        public Assets Assets;
        public Timestamps? Timestamps;
        public ActivityType Type;

        public RefreshRequest(string details, string state, Party? party, Assets assets, Timestamps? timestamps, ActivityType type)
        {
            Details = details;
            State = state;
            Party = party;
            Assets = assets;
            Timestamps = timestamps;
            Type = type;
        }
    }

}