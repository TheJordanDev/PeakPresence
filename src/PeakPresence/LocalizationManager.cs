using System;
using System.Collections.Generic;
using System.IO;
using AncestralMod;
using Newtonsoft.Json;
using PeakPresence;
using WebSocketSharp;

public static class LocalizationManager
{

	public static string assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
	public static string l10nDir = Path.Combine(assemblyDir, "l10n");

	private static Dictionary<string, string> _localizedTexts = new Dictionary<string, string>();

	public static void OnLanguageChanged()
	{
		if (ConfigHandler.ForcedLanguage.Value.IsNullOrEmpty())
		{
			Plugin.Log.LogInfo($"Language changed to {LocalizedText.CURRENT_LANGUAGE}");
			LoadLanguage(LocalizedText.CURRENT_LANGUAGE);
			if (GameHandler.Instance != null)
				GameHandler.GetService<RichPresenceService>().Dirty();
		}
		else if (_localizedTexts.Count == 0) {
			LoadLanguage(GetLangFromCode(ConfigHandler.ForcedLanguage.Value));
		}
	}

	public static void LoadLanguage(LocalizedText.Language lang)
	{
		string langCode = string.IsNullOrEmpty(ConfigHandler.ForcedLanguage.Value) ? GetLangCode(lang) : ConfigHandler.ForcedLanguage.Value;
		string filePath = Path.Combine(l10nDir, $"{langCode}.json");
		if (!File.Exists(filePath)) {
			Plugin.Log.LogWarning($"Localization file not found: {filePath}. Using fallback.");
			filePath = Path.Combine(l10nDir, "en.json");
		}
		if (File.Exists(filePath))
		{
			var json = File.ReadAllText(filePath);
			if (json == null)
				_localizedTexts = new Dictionary<string, string>();
			else
			{
				var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
				_localizedTexts = dict ?? new Dictionary<string, string>();
			}
		}
		else
		{
			Plugin.Log.LogWarning($"Localization file not found: {filePath}");
			_localizedTexts = new Dictionary<string, string>();
		}
	}

	public static string Get(string key)
	{
		if (_localizedTexts.TryGetValue(key, out var value))
			return value;
		return key;
	}

	public static string GetVanilla(string key)
	{
		return LocalizedText.GetText(key);

	}

	private static string GetLangCode(LocalizedText.Language lang)
	{
		return lang switch
		{
			LocalizedText.Language.English => "en",
			LocalizedText.Language.French => "fr",
			LocalizedText.Language.Italian => "it",
			LocalizedText.Language.German => "de",
			LocalizedText.Language.SpanishSpain => "es",
			LocalizedText.Language.SpanishLatam => "es-419",
			LocalizedText.Language.BRPortuguese => "pt-BR",
			LocalizedText.Language.Russian => "ru",
			LocalizedText.Language.Ukrainian => "uk",
			LocalizedText.Language.SimplifiedChinese => "zh-Hans",
			LocalizedText.Language.TraditionalChinese => "zh-Hant",
			LocalizedText.Language.Japanese => "ja",
			LocalizedText.Language.Korean => "ko",
			LocalizedText.Language.Polish => "pl",
			LocalizedText.Language.Turkish => "tr",
			_ => "en",
		};
	}
	
	private static LocalizedText.Language GetLangFromCode(string code)
	{
		return code switch
		{
			"en" => LocalizedText.Language.English,
			"fr" => LocalizedText.Language.French,
			"it" => LocalizedText.Language.Italian,
			"de" => LocalizedText.Language.German,
			"es" => LocalizedText.Language.SpanishSpain,
			"es-419" => LocalizedText.Language.SpanishLatam,
			"pt-BR" => LocalizedText.Language.BRPortuguese,
			"ru" => LocalizedText.Language.Russian,
			"uk" => LocalizedText.Language.Ukrainian,
			"zh-Hans" => LocalizedText.Language.SimplifiedChinese,
			"zh-Hant" => LocalizedText.Language.TraditionalChinese,
			"ja" => LocalizedText.Language.Japanese,
			"ko" => LocalizedText.Language.Korean,
			"pl" => LocalizedText.Language.Polish,
			"tr" => LocalizedText.Language.Turkish,
			_ => LocalizedText.Language.English,
		};
	}
}