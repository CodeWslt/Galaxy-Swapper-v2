using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxy_Swapper_v2.Workspace
{
    class global
    {
        public static string Appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string GalaxyRoamingFolder = $"{Appdata}\\Galaxyv2Data";
        public static string InstallLoc = Properties.Settings.Default.FortniteInstall;
        public static int Offset;
        public static byte[] CompressedBytes;
        public static string CID;
        public static string Username = "Unkown";
        public static string UserPfp = "https://www.galaxyswapperv2.com/Icons/InvalidIcon.png";
        //You Will Need To Add Your Own API Here For The Source To Work
        public static string ApiReturn(string Type)
        {
            using (WebClient wc = new WebClient())
            {
                return wc.DownloadString("https://www.galaxyswapperv2.com/ApiReturn.php");
            }
        }
        public static void ResetCosmeticTabs()
        {
            Workspace.Controls.ControlsSave.Skins.Controls.Clear();
            Workspace.Controls.ControlsSave.Backblings.Controls.Clear();
            Workspace.Controls.ControlsSave.Pickaxes.Controls.Clear();
            Workspace.Controls.ControlsSave.Emotes.Controls.Clear();
            Workspace.Controls.ControlsSave.Wrap.Controls.Clear();
            Workspace.Controls.ControlsSave.Misc.Controls.Clear();
        }
        public static void UrlStart(string url)
        {
            ProcessStartInfo Procc = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C start {url}",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };
            Process.Start(Procc);
        }
        public static void CloseFN()
        {
            JObject parse = JObject.Parse(Workspace.global.ApiReturn("ProcessToKill"));
            foreach (var ProcessNm in parse["CloseFortniteProcesses"])
            {
                Process[] pname = Process.GetProcessesByName(ProcessNm["Process"].ToString());
                if (pname.Length == 0)
                    continue;
                else
                {
                    try { pname[0].Kill(); } catch { }
                    Thread.Sleep(1000);
                }
            }
        }
    }
    public static class ApiReturns
    {
        public static string LanReturn(string Lan)
        {
            JObject parse = JObject.Parse(Workspace.global.ApiReturn("Languages"));
            return parse[Lan].ToString();
        }
        public static string StatusReturn(string Lan)
        {
            JObject parse = JObject.Parse(Workspace.global.ApiReturn("Status"));
            return parse[Lan].ToString();
        }
    }
}
