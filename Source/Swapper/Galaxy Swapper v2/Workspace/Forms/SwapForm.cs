using CUE4Parse.Encryption.Aes;
using CUE4Parse.FileProvider;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Galaxy_Swapper_v2.Workspace.Forms
{
    public partial class SwapForm : Form
    {
        public string CosmeticArray;
        public string CosmeticName;
        public string Aeskey = string.Empty;
        public static ZlibBlock zlibblock;
        public SwapForm(string CosmeticNm)
        {
            InitializeComponent();
            Workspace.UI.Siticon.SetForm(this);
            Workspace.UI.Siticon.SetDrag(Dragbar);
            this.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.MHex());
            Dragbar.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SHex());
            ConvertButton.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.ButtonHex());
            RevertButton.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.ButtonHex());
            foreach (Control x in this.Controls)
            {
                if (x is Label)
                {
                    if (x.Height == 39)
                    {
                        x.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
                        Workspace.UI.Siticon.SetEclipse(x);
                        x.MouseEnter += Workspace.UI.Siticon.MouseEnter;
                        x.MouseLeave += Workspace.UI.Siticon.MouseLeave;
                    }
                    else
                        x.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
                }
            }
            CosmeticName = CosmeticNm;
            this.TopMost = true;
            this.Text = CosmeticName;
        }
        public void Log(string Content) => Logs.Text = $"LOG : {Content}\n";
        private void SwapForm_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            string Api = string.Empty;
            if (Environment.UserName == "white")
            {
                if (File.Exists(@"C:\Users\white\OneDrive\Desktop\Source\Galaxy Swapper\Galaxy Swapper v2\API\Cosmetics.json"))
                    Api = File.ReadAllText(@"C:\Users\white\OneDrive\Desktop\Source\Galaxy Swapper\Galaxy Swapper v2\API\Cosmetics.json");
                else
                    Api = Workspace.Encryption.Compression.Decompress(Workspace.global.ApiReturn("Cosmetics"));
            }
            else
                Api = Workspace.Encryption.Compression.Decompress(Workspace.global.ApiReturn("Cosmetics"));
            JObject parse = JObject.Parse(Api);
            foreach (var Cosmetic in parse["Cosmetics"])
            {
                if (Cosmetic["Name"].ToString() == CosmeticName)
                {
                    CosmeticArray = Cosmetic.ToString();
                    CosmeticNameLabel.Text = CosmeticName;
                    SearchI.ImageLocation = Cosmetic["Swapicon"].ToString();
                    ReplaceI.ImageLocation = Cosmetic["Icon"].ToString();
                    this.Name = CosmeticName;
                    if (IfSwapped(CosmeticNameLabel.Text) == true)
                    {
                        SwappedActive.ForeColor = Color.Yellow;
                        SwappedActive.Text = "ON";
                    }
                    else
                    {
                        SwappedActive.ForeColor = Color.Red;
                        SwappedActive.Text = "OFF";
                    }
                    try { Workspace.Other.RPC.SetRPC($"{CosmeticName.Substring(CosmeticName.LastIndexOf("To ")).Replace("To", "")}"); }
                    catch { }
                    Task.Run(() => {
                        if (Cosmetic["Message"].ToString() != "false")
                            MessageBox.Show(this, Cosmetic["Message"].ToString(), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    });
                    return;
                }
            }
        }
        private void ConvertButton_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker Swap = new BackgroundWorker())
            {
                Swap.DoWork += Convert_DoWork;
                if (Swap.IsBusy)
                    MessageBox.Show(this, "Swap Worker Is Currently Busy!", string.Empty);
                else
                    Swap.RunWorkerAsync();
            }
        }
        private void RevertButton_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker Revert = new BackgroundWorker())
            {
                Revert.DoWork += Revert_DoWork;
                if (Revert.IsBusy)
                    MessageBox.Show(this, "Revert Worker Is Currently Busy!", string.Empty);
                else
                    Revert.RunWorkerAsync();
            }
        }

        public void LoadAes()
        {
            if (Aeskey == string.Empty)
            {
                using (WebClient w = new WebClient())
                {
                    try
                    {
                        JObject aesparse = JObject.Parse(w.DownloadString("https://fortnite-api.com/v2/aes"));
                        Aeskey = $"0x{aesparse["data"]["mainKey"].ToString()}";
                    }
                    catch
                    {
                        JObject aesparse = JObject.Parse(w.DownloadString("https://benbot.app/api/v1/aes"));
                        Aeskey = aesparse["mainKey"].ToString();
                    }
                }
            }
        }
        private void Convert_DoWork(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Stopwatch TotalTime = new Stopwatch();
            TotalTime.Start();
            Log("Starting...");
            LoadAes();
            LogsController("remove", CosmeticNameLabel.Text);
            Workspace.global.CloseFN();
            var provider = new DefaultFileProvider(Properties.Settings.Default.FortniteInstall, SearchOption.TopDirectoryOnly);
            int Swapcount = 0;
            JObject Parse = JObject.Parse(CosmeticArray);
            foreach (var Assets in Parse["Assets"])
            {
                if (Assets["CompressionType"].ToString() == "oodle")
                {
                    Log($"Backing Up {Assets["AssetUcas"]}.ucas");
                    DupeFiles(Assets["AssetUcas"].ToString());
                    provider.Initialize(Assets["AssetUcas"].ToString());
                    provider.UnloadedVfs.All(x => { provider.SubmitKey(x.EncryptionKeyGuid, new FAesKey(Aeskey)); return true; });
                    string Assethex = SwapUtils.ByteToHex(provider.SaveAsset(Assets["AssetPath"].ToString()));
                    foreach (var Swaps in Assets["Swaps"])
                    {
                        switch (Swaps["type"].ToString())
                        {
                            case "string":
                                Assethex = Assethex.Replace(SwapUtils.StringToHex(Swaps["search"].ToString()), SwapUtils.Matchlength(Swaps["search"].ToString(), Swaps["replace"].ToString()));
                                break;
                            case "hex":
                                Assethex = Assethex.Replace(Swaps["search"].ToString(), Swaps["replace"].ToString());
                                break;
                        }
                    }
                    Assethex = SwapUtils.InvalidScript(Assethex);
                    byte[] CompressedBytes = Workspace.Other.Oodle.Compress(SwapUtils.HexToByte(Assethex));
                    CompressedBytes = SwapUtils.MatchBytelength(Workspace.global.CompressedBytes, CompressedBytes);
                    using (FileStream UcasEdit = File.Open($"{Properties.Settings.Default.FortniteInstall}\\Galaxy Swapper v2\\{Assets["AssetUcas"]}.ucas", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        UcasEdit.Position = global.Offset;
                        UcasEdit.Write(CompressedBytes, 0, CompressedBytes.Length);
                        UcasEdit.Close();
                    }
                }
                else
                {
                    if (!File.Exists(Properties.Settings.Default.FortniteInstall + $"\\{Assets["AssetUcas"]}.pak"))
                    {
                        MessageBox.Show(this, Workspace.ApiReturns.LanReturn("MissingPak").Replace("%FILE%", Assets["AssetUcas"].ToString()), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    provider.Initialize(Assets["AssetUcas"].ToString());
                    provider.UnloadedVfs.All(x => { provider.SubmitKey(x.EncryptionKeyGuid, new FAesKey(Aeskey)); return true; });
                    Workspace.global.CID = Assets["CosmeticIDLoc"].ToString();
                    provider.SaveAsset(Assets["AssetPath"].ToString());
                    string Assethex = SwapUtils.ByteToHex(zlibblock.decompressed);
                    foreach (var Swaps in Assets["Swaps"])
                    {
                        switch (Swaps["type"].ToString())
                        {
                            case "string":
                                Assethex = Assethex.Replace(SwapUtils.StringToHex(Swaps["search"].ToString()), SwapUtils.Matchlength(Swaps["search"].ToString(), Swaps["replace"].ToString()));
                                break;
                            case "hex":
                                Assethex = Assethex.Replace(Swaps["search"].ToString(), Swaps["replace"].ToString());
                                break;
                        }
                    }
                    byte[] compressed = Workspace.Other.Zlib.ZlibCompress(SwapUtils.HexToByte(Assethex));
                    if (!Directory.Exists(Properties.Settings.Default.FortniteInstall + @"\~GalaxyLobby"))
                        Directory.CreateDirectory(Properties.Settings.Default.FortniteInstall + @"\~GalaxyLobby");
                    if (!File.Exists($"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{Assets["AssetUcas"]}.pak"))
                    {
                        Log($"Backing Up {Assets["AssetUcas"]}.pak");
                        File.Copy($"{Properties.Settings.Default.FortniteInstall}\\{Assets["AssetUcas"]}.pak", $"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{Assets["AssetUcas"]}.pak");
                    }
                    if (!File.Exists($"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{Assets["AssetUcas"]}.sig"))
                    {
                        Log($"Backing Up {Assets["AssetUcas"]}.sig");
                        File.Copy($"{Properties.Settings.Default.FortniteInstall}\\{Assets["AssetUcas"]}.sig", $"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{Assets["AssetUcas"]}.sig");
                    }
                    using (FileStream UcasEdit = File.Open($"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{Assets["AssetUcas"]}.pak", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        UcasEdit.Position = global.Offset;
                        UcasEdit.Write(compressed, 0, compressed.Length);
                        UcasEdit.Close();
                    }
                }
                Swapcount++;
                Log($"Converted Cosmetic Asset {Swapcount}");
            }
            TimeSpan st = TotalTime.Elapsed;
            Log($"Completed In {st.Seconds} Seconds");
            LogsController("add", CosmeticNameLabel.Text);
            if (IfSwapped(CosmeticNameLabel.Text) == true)
            {
                SwappedActive.ForeColor = Color.Yellow;
                SwappedActive.Text = "ON";
            }
            else
            {
                SwappedActive.ForeColor = Color.Red;
                SwappedActive.Text = "OFF";
            }
            MessageBox.Show(this, $"Successfully Converted In {st.Seconds} Seconds", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Revert_DoWork(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Stopwatch TotalTime = new Stopwatch();
            TotalTime.Start();
            LoadAes();
            Log("Starting...");
            Workspace.global.CloseFN();
            var provider = new DefaultFileProvider(Properties.Settings.Default.FortniteInstall, SearchOption.TopDirectoryOnly);
            int Swapcount = 0;
            JObject Parse = JObject.Parse(CosmeticArray);
            foreach (var Assets in Parse["Assets"])
            {
                if (Assets["CompressionType"].ToString() == "oodle")
                {
                    Log($"Backing Up {Assets["AssetUcas"]}.ucas");
                    DupeFiles(Assets["AssetUcas"].ToString());
                    provider.Initialize(Assets["AssetUcas"].ToString());
                    provider.UnloadedVfs.All(x => { provider.SubmitKey(x.EncryptionKeyGuid, new FAesKey(Aeskey)); return true; });
                    provider.SaveAsset(Assets["AssetPath"].ToString());
                    if (File.Exists($"{Properties.Settings.Default.FortniteInstall}\\Galaxy Swapper v2\\{Assets["AssetUcas"]}.ucas"))
                    {
                        using (FileStream UcasEdit = File.Open($"{Properties.Settings.Default.FortniteInstall}\\Galaxy Swapper v2\\{Assets["AssetUcas"]}.ucas", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            UcasEdit.Position = global.Offset;
                            UcasEdit.Write(global.CompressedBytes, 0, global.CompressedBytes.Length);
                            UcasEdit.Close();
                        }
                    }
                }
                else
                {
                    if (!File.Exists(Properties.Settings.Default.FortniteInstall + $"\\{Assets["AssetUcas"]}.pak"))
                    {
                        MessageBox.Show(this, Workspace.ApiReturns.LanReturn("MissingPak").Replace("%FILE%", Assets["AssetUcas"].ToString()), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    provider.Initialize(Assets["AssetUcas"].ToString());
                    provider.UnloadedVfs.All(x => { provider.SubmitKey(x.EncryptionKeyGuid, new FAesKey(Aeskey)); return true; });
                    Workspace.global.CID = Assets["CosmeticIDLoc"].ToString();
                    provider.SaveAsset(Assets["AssetPath"].ToString());
                    if (!Directory.Exists(Properties.Settings.Default.FortniteInstall + @"\~GalaxyLobby"))
                        Directory.CreateDirectory(Properties.Settings.Default.FortniteInstall + @"\~GalaxyLobby");
                    if (File.Exists($"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{Assets["AssetUcas"]}.pak"))
                    {
                        using (FileStream UcasEdit = File.Open($"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{Assets["AssetUcas"]}.pak", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            UcasEdit.Position = global.Offset;
                            UcasEdit.Write(zlibblock.compressed, 0, zlibblock.compressed.Length);
                            UcasEdit.Close();
                        }
                    }
                }
                Swapcount++;
                Log($"Reverted Cosmetic Asset: {Swapcount}");
            }
            TimeSpan st = TotalTime.Elapsed;
            Log($"Completed In {st.Seconds} Seconds");
            LogsController("remove", CosmeticNameLabel.Text);
            if (IfSwapped(CosmeticNameLabel.Text) == true)
            {
                SwappedActive.ForeColor = Color.Yellow;
                SwappedActive.Text = "ON";
            }
            else
            {
                SwappedActive.ForeColor = Color.Red;
                SwappedActive.Text = "OFF";
            }
            MessageBox.Show(this, $"Successfully Reverted In {st.Seconds} Seconds", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void DupeFiles(string Ucas)
        {
            string[] Exenstions = new string[]
            {
            ".ucas",
            ".utoc",
            ".pak",
            ".sig"
            };
            foreach (string Ex in Exenstions)
            {
                if (!Directory.Exists(Properties.Settings.Default.FortniteInstall + @"\Galaxy Swapper v2"))
                    Directory.CreateDirectory(Properties.Settings.Default.FortniteInstall + @"\Galaxy Swapper v2");
                if (File.Exists(Workspace.global.InstallLoc + $"\\{Ucas}{Ex}"))
                {
                    if (!File.Exists(Workspace.global.InstallLoc + $"\\Galaxy Swapper v2\\{Ucas}{Ex}"))
                    {
                        File.Copy(Workspace.global.InstallLoc + $"\\{Ucas}{Ex}", Workspace.global.InstallLoc + $"\\Galaxy Swapper v2\\{Ucas}{Ex}");
                    }
                }
                else
                {
                    MessageBox.Show(this, Workspace.ApiReturns.LanReturn("MissingPak").Replace("%FILE%", Ucas), string.Empty,MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                    return;
                }
            }
        }
        public bool IfSwapped(string Itemname)
        {
            try
            {
                string LogsFile = File.ReadAllText(Workspace.global.GalaxyRoamingFolder + "\\Swaplogs.log");
                if (LogsFile.Contains(Itemname))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public void LogsController(string Function, string Item)
        {
            try
            {
                if (Function == "add")
                {
                    File.AppendAllText(Workspace.global.GalaxyRoamingFolder + "\\Swaplogs.log", Item + Environment.NewLine);
                }
                if (Function == "remove")
                {
                    string LogsFile = File.ReadAllText(Workspace.global.GalaxyRoamingFolder + "\\Swaplogs.log");
                    LogsFile = LogsFile.Replace(Item + Environment.NewLine, string.Empty);
                    File.WriteAllText(Workspace.global.GalaxyRoamingFolder + "\\Swaplogs.log", LogsFile);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Caught A Error While Trying To Edit Logs!\n{error.ToString()}\nPlease Restart The Swapper And Try Again.", "Caught A Error Please Read!", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                return;
            }
        }
        private void CloseBox_Click(object sender, EventArgs e) => this.Close();
    }
    public static class SwapUtils
    {
        public static byte[] StringToBytes(this string str)
        {
            return Encoding.Default.GetBytes(str);
        }
        public static string InvalidScript(string Assethex)
        {
            string[] Scripts = new string[]
            {
            "SoftObjectProperty",
            "SkeletalMesh",
            "Package",
            "ObjectProperty",
            "MasterSkeletalMeshes",
            "GenderPermitted",
            "Default__CustomCharacterPart",
            "CustomCharacterPart",
            "CustomCharacterBodyPartData",
            "CharacterPartType",
            "ByteProperty",
            "bSupportsColorSwatches",
            "BoolProperty",
            "AnimClass",
            "ArrayProperty",
            "BodyTypesPermitted",
            "DefaultCustomCharacterBodyPartData",
            "DefaultCustomCharacterPart",
            "IntProperty",
            "StructProperty"
            };
            foreach (string Scwipt in Scripts)
            {
                Assethex = Assethex.Replace(SwapUtils.StringToHex(Scwipt), SwapUtils.Matchlength(Scwipt, "_"));
            }
            return Assethex;
        }
        public static byte[] MatchBytelength(byte[] search, byte[] replace)
        {
            if (search.Length < replace.Length)
            {
                MessageBox.Show("Search Byte Is Longer Then Replace Byte! Please Verify Fortnite In Galaxy Swapper Settings And Try Again.", "Caught A Error Please Read!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            List<byte> result = new List<byte>(replace);
            int difference = search.Length - replace.Length;
            for (int i = 0; i < difference; i++)
                result.Add(0);
            return result.ToArray();
        }
        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] array = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, array, 0, first.Length);
            Buffer.BlockCopy(second, 0, array, first.Length, second.Length);
            return array;
        }
        public static string Matchlength(string Match, string Input)
        {
            Match = SwapUtils.StringToHex(Match);
            Input = SwapUtils.StringToHex(Input);
            while (Input.Length != Match.Length)
                Input += "00";
            return Input;
        }
        public static string ByteToHex(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            byte b;
            for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
            {
                b = ((byte)(bytes[bx] >> 4));
                c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                b = ((byte)(bytes[bx] & 0x0F));
                c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
            }
            return new string(c);
        }
        public static string StringToHex(string hexstring)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char t in hexstring)
            {
                sb.Append(Convert.ToInt32(t).ToString("x"));
            }
            return sb.ToString();
        }
        public static byte[] HexToByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return data;
        }
    }
}
