using Newtonsoft.Json.Linq;
using System.IO;

namespace Galaxy_Swapper_v2.Workspace.Other
{
    class SettingsController
    {
        public static string SettingsBaseRaw()
        {
            JObject o = JObject.FromObject(new
            {
                DiscordRPC = false,
                EmoteWarning = false,
                HideNonePerfectDefaultSwaps = false,
                MHex = "#171a24",
                SHex = "#1e1e2f",
                TextHex = "#ffffff",
                ButtonHex = "#1e202f",
                HHex = "#151521",
                SecTextHex = "#3A3F4B",
                CosmeticIconSize = "80",
                SearchID = string.Empty,
                ReplaceID = string.Empty
            });
            return o.ToString();
        }
        public static void EditSettings(string SettingValue, string JsonValue)
        {
            JObject parse = JObject.Parse(File.ReadAllText($"{global.GalaxyRoamingFolder}\\Settings.config"));
            parse[SettingValue] = JsonValue;
            File.WriteAllText($"{global.GalaxyRoamingFolder}\\Settings.config", parse.ToString());
        }
    }
}
