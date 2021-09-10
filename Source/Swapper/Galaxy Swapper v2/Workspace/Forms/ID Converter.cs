using CUE4Parse.Encryption.Aes;
using CUE4Parse.FileProvider;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Galaxy_Swapper_v2.Workspace.Forms
{
    public partial class ID_Converter : Form
    {
        public string Aeskey = string.Empty;
        public ID_Converter()
        {
            InitializeComponent();
            Workspace.UI.Siticon.SetForm(this);
            Workspace.UI.Siticon.SetDrag(Dragbar);
            this.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.MHex());
            Dragbar.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SHex());
            foreach (Control x in this.Controls)
            {
                if (x is Label)
                {
                    if (x.Height == 39)
                    {
                        Workspace.UI.Siticon.SetEclipse(x);
                        x.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
                        x.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.ButtonHex());
                        x.MouseEnter += Workspace.UI.Siticon.MouseEnter;
                        x.MouseLeave += Workspace.UI.Siticon.MouseLeave;
                    }
                    else
                        x.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
                }
            }
            textBox1.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.ButtonHex());
            textBox2.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.ButtonHex());
            textBox1.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            textBox2.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            if (Workspace.Other.Colors.SearchID().Length != 0)
                textBox1.Text = Workspace.Other.Colors.SearchID();
            if (Workspace.Other.Colors.ReplaceID().Length != 0)
                textBox2.Text = Workspace.Other.Colors.ReplaceID();
        }
        private void CloseBox_Click(object sender, EventArgs e) => this.Close();
        private void textBox1_TextChanged(object sender, EventArgs e) => label1.Text = textBox1.Text.Length.ToString();
        private void textBox2_TextChanged(object sender, EventArgs e) => label2.Text = textBox2.Text.Length.ToString();
        private void ConvertButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < textBox2.Text.Length)
            {
                MessageBox.Show(this, "Search String Is Longer Then Replace!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show(this, "Search String Is Blank!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox2.Text.Length == 0)
            {
                MessageBox.Show(this, "Replace String Is Blank!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (BackgroundWorker Swap = new BackgroundWorker())
            {
                Swap.DoWork += Swap_DoWork;
                if (Swap.IsBusy)
                    MessageBox.Show(this, "Swap Worker Is Currently Busy!", string.Empty);
                else
                    Swap.RunWorkerAsync();
            }
        }
        private void RevertButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < textBox2.Text.Length)
            {
                MessageBox.Show(this, "Search String Is Longer Then Replace!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show(this, "Search String Is Blank!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox2.Text.Length == 0)
            {
                MessageBox.Show(this, "Replace String Is Blank!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (BackgroundWorker Swap = new BackgroundWorker())
            {
                Swap.DoWork += Revert_DoWork;
                if (Swap.IsBusy)
                    MessageBox.Show(this, "Revert Worker Is Currently Busy!", string.Empty);
                else
                    Swap.RunWorkerAsync();
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
        private void Swap_DoWork(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Stopwatch TotalTime = new Stopwatch();
            TotalTime.Start();
            LoadAes();
            Workspace.global.CloseFN();
            JObject parse = JObject.Parse(Workspace.global.ApiReturn("IDConverter"));
            if (!Directory.Exists(Properties.Settings.Default.FortniteInstall + @"\~GalaxyLobby"))
                Directory.CreateDirectory(Properties.Settings.Default.FortniteInstall + @"\~GalaxyLobby");
            if (!File.Exists($"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{parse["AssetRegUcas"]}.pak"))
            {
                File.Copy($"{Properties.Settings.Default.FortniteInstall}\\{parse["AssetRegUcas"]}.pak", $"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{parse["AssetRegUcas"]}.pak");
            }
            if (!File.Exists($"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{parse["AssetRegUcas"]}.sig"))
            {
                File.Copy($"{Properties.Settings.Default.FortniteInstall}\\{parse["AssetRegUcas"]}.sig", $"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{parse["AssetRegUcas"]}.sig");
            }
            var provider = new DefaultFileProvider(Properties.Settings.Default.FortniteInstall + "\\~GalaxyLobby", SearchOption.TopDirectoryOnly);
            provider.Initialize(parse["AssetRegUcas"].ToString());
            provider.UnloadedVfs.All(x => { provider.SubmitKey(x.EncryptionKeyGuid, new FAesKey(Aeskey)); return true; });
            Workspace.global.CID = textBox1.Text + "." + textBox1.Text;
            provider.SaveAsset(parse["AssetReg"].ToString());
            string Assethex = SwapUtils.ByteToHex(Workspace.Forms.SwapForm.zlibblock.decompressed);
            Assethex = Assethex.Replace(SwapUtils.StringToHex(textBox1.Text + "." + textBox1.Text), SwapUtils.Matchlength(textBox1.Text + "." + textBox1.Text, textBox2.Text + "." + textBox2.Text));
            byte[] compressed = Workspace.Other.Zlib.ZlibCompress(SwapUtils.HexToByte(Assethex));
            using (FileStream UcasEdit = new FileStream($"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{parse["AssetRegUcas"]}.pak", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                UcasEdit.Position = global.Offset;
                UcasEdit.Write(compressed, 0, compressed.Length);
                UcasEdit.Close();
            }
            Workspace.Other.SettingsController.EditSettings("SearchID", textBox1.Text);
            Workspace.Other.SettingsController.EditSettings("ReplaceID", textBox2.Text);
            Workspace.Other.Colors.Settingsfile = string.Empty;
            TimeSpan st = TotalTime.Elapsed;
            MessageBox.Show(this, $"Successfully Converted In {st.Seconds} Seconds", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Revert_DoWork(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Stopwatch TotalTime = new Stopwatch();
            TotalTime.Start();
            LoadAes();
            Workspace.global.CloseFN();
            JObject parse = JObject.Parse(Workspace.global.ApiReturn("IDConverter"));
            if (!Directory.Exists(Properties.Settings.Default.FortniteInstall + @"\~GalaxyLobby"))
                Directory.CreateDirectory(Properties.Settings.Default.FortniteInstall + @"\~GalaxyLobby");
            if (!File.Exists($"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{parse["AssetRegUcas"]}.pak"))
            {
                File.Copy($"{Properties.Settings.Default.FortniteInstall}\\{parse["AssetRegUcas"]}.pak", $"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{parse["AssetRegUcas"]}.pak");
            }
            if (!File.Exists($"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{parse["AssetRegUcas"]}.sig"))
            {
                File.Copy($"{Properties.Settings.Default.FortniteInstall}\\{parse["AssetRegUcas"]}.sig", $"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{parse["AssetRegUcas"]}.sig");
            }
            var provider = new DefaultFileProvider(Properties.Settings.Default.FortniteInstall, SearchOption.TopDirectoryOnly);
            provider.Initialize(parse["AssetRegUcas"].ToString());
            provider.UnloadedVfs.All(x => { provider.SubmitKey(x.EncryptionKeyGuid, new FAesKey(Aeskey)); return true; });
            Workspace.global.CID = textBox1.Text + "." + textBox1.Text;
            provider.SaveAsset(parse["AssetReg"].ToString());
            using (FileStream UcasEdit = new FileStream($"{Properties.Settings.Default.FortniteInstall}\\~GalaxyLobby\\{parse["AssetRegUcas"]}.pak", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                UcasEdit.Position = global.Offset;
                UcasEdit.Write(Workspace.Forms.SwapForm.zlibblock.compressed, 0, Workspace.Forms.SwapForm.zlibblock.compressed.Length);
                UcasEdit.Close();
            }
            TimeSpan st = TotalTime.Elapsed;
            MessageBox.Show(this, $"Successfully Reverted In {st.Seconds} Seconds", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            JObject parse = JObject.Parse(Workspace.global.ApiReturn("IDConverter"));
            Workspace.global.UrlStart(parse["IDList"].ToString());
        }

        private void label4_Click(object sender, EventArgs e)
        {
            JObject parse = JObject.Parse(Workspace.global.ApiReturn("IDConverter"));
            Workspace.global.UrlStart(parse["Tutorial"].ToString());
        }

        private void ID_Converter_Load(object sender, EventArgs e)
        {
            Task.Run(() => {
                CheckForIllegalCrossThreadCalls = false;
                Workspace.Other.RPC.SetRPC("ID Swapper");
                MessageBox.Show(this, Workspace.ApiReturns.LanReturn("IDWarning"), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }
    }
}
