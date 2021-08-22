using IWshRuntimeLibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Media;
using System.Net;
using System.Threading;

namespace Galaxy_Swapper_v2_Installer
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Galaxy Swapper v2 Installer";
            Console.WriteLine("Before Installing Please Make Sure To Disable Your Anti Virus So Nothing Interferes With The Install Process!\nClick Enter To Continue.");
            Console.ReadLine();
            Console.Clear();
            Stopwatch s = new Stopwatch();
            s.Start();

            string InstallDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            using (WebClient w = new WebClient())
            {
                JObject parse = JObject.Parse(w.DownloadString("https://raw.githubusercontent.com/CodeWslt/Galaxy-Swapper-v2/main/Backend/Endpoints/Installer/InstallFiles.json"));
                if (IfDir(InstallDir + "\\GalaxySwapperv2") == true)
                    Directory.Delete(InstallDir + "\\GalaxySwapperv2", true);
                Directory.CreateDirectory(InstallDir + "\\GalaxySwapperv2");
                Log($"Created Directory {InstallDir + "\\GalaxySwapperv2"}");
                w.DownloadFile(parse["ZipDowload"].ToString(), InstallDir + "\\GalaxySwapperv2\\Install.zip");
                Log($"Installed.zip To {InstallDir + "\\GalaxySwapperv2\\Install.zip"}");
                w.DownloadFile(parse["IconDownload"].ToString(), InstallDir + "\\GalaxySwapperv2\\Icon.ico");
                Log($"Icon.ico To {InstallDir + "\\GalaxySwapperv2\\Icon.ico"}");
                Thread.Sleep(1000);
            }

            ZipFile.ExtractToDirectory(InstallDir + "\\GalaxySwapperv2\\Install.zip", InstallDir + "\\GalaxySwapperv2");
            Log($"Extracted Install Files To {InstallDir + "\\GalaxySwapperv2"}");
            System.IO.File.Delete(InstallDir + "\\GalaxySwapperv2\\Install.zip");
            Log($"Deleted Temp File {InstallDir + "\\GalaxySwapperv2\\Install.zip"}");
            CreateShortcut("Galaxy Swapper v2", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), FindExe(InstallDir + "\\GalaxySwapperv2"), InstallDir + "\\GalaxySwapperv2\\Icon.ico");
            Log($"Created Shortcut At {Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}");
            Thread.Sleep(2000);
            Console.Clear();
            SystemSounds.Exclamation.Play();
            TimeSpan ts = s.Elapsed;
            Console.ForegroundColor = ConsoleColor.Green;

            if (ts.Seconds.ToString().Length == 1)
                Console.WriteLine($"Completed Installing Galaxy Swapper v2 In {ts.Minutes}:{ts.Seconds}0!\nWould You Like To Open Galaxy Swapper Now?\n1. Yes\n2. No");
            else
                Console.WriteLine($"Completed Installing Galaxy Swapper v2 In {ts.Minutes}:{ts.Seconds}!\nWould You Like To Open Galaxy Swapper Now?\n1. Yes\n2. No");
            string Answer = Console.ReadLine();
            if (Answer == "1")
                Process.Start(FindExe(InstallDir + "\\GalaxySwapperv2"));
            else
                Environment.Exit(0);
        }
        static void Log(string Data) => Console.WriteLine($"LOG : {Data}");
        static bool IfDir(string Path)
        {
            if (Directory.Exists(Path))
                return true;
            else
                return false;
        }
        static string FindExe(string Dir)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(Dir);
            foreach (FileInfo file in di.GetFiles())
            {
                if (Path.GetExtension(file.FullName) == ".exe")
                    return file.FullName;
            }
            return null;
        }
        //https://www.fluxbytes.com/csharp/create-shortcut-programmatically-in-c/
        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation, string IcoPath)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "My shortcut description";   // The description of the shortcut
            shortcut.IconLocation = IcoPath;           // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }
    }
}
