using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Swapper_v2.Workspace.Other
{
    class Colors
    {
        public static string Settingsfile = string.Empty;
        public static string MHex()
        {
            if (Settingsfile == string.Empty)
            {
                Settingsfile = File.ReadAllText($"{global.GalaxyRoamingFolder}\\Settings.config");
            }
            JObject parse = JObject.Parse(Settingsfile);
            return parse["MHex"].ToString();
        }
        public static string SHex()
        {
            if (Settingsfile == string.Empty)
            {
                Settingsfile = File.ReadAllText($"{global.GalaxyRoamingFolder}\\Settings.config");
            }
            JObject parse = JObject.Parse(Settingsfile);
            return parse["SHex"].ToString();
        }
        public static string HHex()
        {
            if (Settingsfile == string.Empty)
            {
                Settingsfile = File.ReadAllText($"{global.GalaxyRoamingFolder}\\Settings.config");
            }
            JObject parse = JObject.Parse(Settingsfile);
            return parse["HHex"].ToString();
        }
        public static string TextHex()
        {
            if (Settingsfile == string.Empty)
                Settingsfile = File.ReadAllText($"{global.GalaxyRoamingFolder}\\Settings.config");
            JObject parse = JObject.Parse(Settingsfile);
            return parse["TextHex"].ToString();
        }
        public static string SecTextHex()
        {
            if (Settingsfile == string.Empty)
                Settingsfile = File.ReadAllText($"{global.GalaxyRoamingFolder}\\Settings.config");
            JObject parse = JObject.Parse(Settingsfile);
            return parse["SecTextHex"].ToString();
        }
        public static string ButtonHex()
        {
            if (Settingsfile == string.Empty)
            {
                Settingsfile = File.ReadAllText($"{global.GalaxyRoamingFolder}\\Settings.config");
            }
            JObject parse = JObject.Parse(Settingsfile);
            return parse["ButtonHex"].ToString();
        }
        public static int IconSize()
        {
            if (Settingsfile == string.Empty)
            {
                Settingsfile = File.ReadAllText($"{global.GalaxyRoamingFolder}\\Settings.config");
            }
            JObject parse = JObject.Parse(Settingsfile);
            return int.Parse(parse["CosmeticIconSize"].ToString());
        }
        public static string DiscordRPC()
        {
            if (Settingsfile == string.Empty)
            {
                Settingsfile = File.ReadAllText($"{global.GalaxyRoamingFolder}\\Settings.config");
            }
            JObject parse = JObject.Parse(Settingsfile);
            return parse["DiscordRPC"].ToString();
        }
        public static string EmoteWarning()
        {
            if (Settingsfile == string.Empty)
            {
                Settingsfile = File.ReadAllText($"{global.GalaxyRoamingFolder}\\Settings.config");
            }
            JObject parse = JObject.Parse(Settingsfile);
            return parse["EmoteWarning"].ToString();
        }
        public static string DefaultHide()
        {
            if (Settingsfile == string.Empty)
            {
                Settingsfile = File.ReadAllText($"{global.GalaxyRoamingFolder}\\Settings.config");
            }
            JObject parse = JObject.Parse(Settingsfile);
            return parse["HideNonePerfectDefaultSwaps"].ToString();
        }
        public static string SearchID()
        {
            if (Settingsfile == string.Empty)
            {
                Settingsfile = File.ReadAllText($"{global.GalaxyRoamingFolder}\\Settings.config");
            }
            JObject parse = JObject.Parse(Settingsfile);
            return parse["SearchID"].ToString();
        }
        public static string ReplaceID()
        {
            if (Settingsfile == string.Empty)
            {
                Settingsfile = File.ReadAllText($"{global.GalaxyRoamingFolder}\\Settings.config");
            }
            JObject parse = JObject.Parse(Settingsfile);
            return parse["ReplaceID"].ToString();
        }
    }
}
