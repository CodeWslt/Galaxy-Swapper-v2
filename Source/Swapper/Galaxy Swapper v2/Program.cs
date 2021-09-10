using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;
using Galaxy_Swapper_v2.Workspace;
using Newtonsoft.Json.Linq;

namespace Galaxy_Swapper_v2
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string Appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (!Directory.Exists($"{Appdata}\\Galaxyv2Data"))
                Directory.CreateDirectory($"{Appdata}\\Galaxyv2Data");

            if (!File.Exists($"{Appdata}\\Galaxyv2Data\\Settings.config"))
                System.IO.File.WriteAllText($"{Appdata}\\Galaxyv2Data\\Settings.config", Workspace.Other.SettingsController.SettingsBaseRaw());

            if (File.ReadAllText($"{Appdata}\\Galaxyv2Data\\Settings.config").Length == 0)
                System.IO.File.WriteAllText($"{Appdata}\\Galaxyv2Data\\Settings.config", Workspace.Other.SettingsController.SettingsBaseRaw());

            if (!File.Exists($"{Appdata}\\Galaxyv2Data\\Swaplogs.log"))
                System.IO.File.WriteAllText($"{Appdata}\\Galaxyv2Data\\Swaplogs.log", string.Empty);

            if (Properties.Settings.Default.FortniteInstall == string.Empty)
            {
                Properties.Settings.Default.FortniteInstall = FortniteInstallLocation();
                Properties.Settings.Default.Save();
            }
            else
            {
                if (!File.Exists(Properties.Settings.Default.FortniteInstall + @"\global.ucas"))
                {
                    Properties.Settings.Default.FortniteInstall = FortniteInstallLocation();
                    Properties.Settings.Default.Save();
                }
            }
            CheckForUnkownUcas();
            Application.Run(new Dashboard());
        }
        public static void CheckForUnkownUcas()
        {
            string PakFolder = Workspace.global.InstallLoc;
            System.IO.DirectoryInfo di = new DirectoryInfo(PakFolder);
            foreach (DirectoryInfo PakDirList in di.GetDirectories())
            {
                try
                {
                    foreach (string Paks in Directory.EnumerateFiles(PakDirList.FullName, "*.pak*", SearchOption.AllDirectories))
                    {
                        if (!PakDirList.FullName.Contains("Galaxy Swapper v2") && !PakDirList.FullName.Contains("~GalaxyLobby"))
                            File.Delete(Paks);
                    }
                    foreach (string Sigs in Directory.EnumerateFiles(PakDirList.FullName, "*.sig*", SearchOption.AllDirectories))
                    {
                        if (!PakDirList.FullName.Contains("Galaxy Swapper v2") && !PakDirList.FullName.Contains("~GalaxyLobby"))
                            File.Delete(Sigs);
                    }
                    foreach (string Ucas in Directory.EnumerateFiles(PakDirList.FullName, "*.ucas*", SearchOption.AllDirectories))
                    {
                        if (!PakDirList.FullName.Contains("Galaxy Swapper v2") && !PakDirList.FullName.Contains("~GalaxyLobby"))
                            File.Delete(Ucas);
                    }
                    foreach (string Utocs in Directory.EnumerateFiles(PakDirList.FullName, "*.utoc*", SearchOption.AllDirectories))
                    {
                        if (!PakDirList.FullName.Contains("Galaxy Swapper v2") && !PakDirList.FullName.Contains("~GalaxyLobby"))
                            File.Delete(Utocs);
                    }
                }
                catch { return; }
            }
        }
        public static string FortniteInstallLocation()
        {
            if (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Epic\UnrealEngineLauncher"))
            {
                JObject Parse = JObject.Parse(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Epic\UnrealEngineLauncher\LauncherInstalled.dat"));
                foreach (var Array in Parse["InstallationList"])
                {
                    if (System.IO.Directory.Exists(Array["InstallLocation"].ToString() + @"\FortniteGame"))
                        return Array["InstallLocation"].ToString() + @"\FortniteGame\Content\Paks";
                }
            }
            else
                return string.Empty;
            return string.Empty;
        }
    }
}
