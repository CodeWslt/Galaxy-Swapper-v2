using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Galaxy_Swapper_v2.Workspace.Controls
{
    public partial class Settingcntrl : UserControl
    {
        Dashboard Dashboard;
        public Settingcntrl(Dashboard Dsh)
        {
            InitializeComponent();
            Dashboard = Dsh;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged;
        }

        private void Settingcntrl_Load(object sender, EventArgs e)
        {
            Task.Run(() => {
                CheckForIllegalCrossThreadCalls = false;
                FortniteInstallLabel.Text = Workspace.global.InstallLoc;
                label13.Text = $"Username: {Workspace.global.Username}";
                FolderIcon.LoadAsync("https://www.galaxyswapperv2.com/Icons/folder.png");
                pictureBox2.ImageLocation = Workspace.global.UserPfp;
                FortniteInstallLabel.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
                Workspace.UI.Siticon.SetEclipse2(pictureBox2, 100);

                if (Workspace.Other.Colors.SecTextHex() != "#3A3F4B")
                    FortniteInstallLabel.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SecTextHex());
                if (Workspace.Other.Colors.DiscordRPC() == "true")
                    checkBox1.Checked = true;
                if (Workspace.Other.Colors.EmoteWarning() == "true")
                    checkBox2.Checked = true;
                if (Workspace.Other.Colors.DefaultHide() == "true")
                    checkBox3.Checked = true;

                foreach (Control x in this.Controls)
                {
                    if (x is Label)
                    {
                        if (x.Font.Size == 9)
                        {
                            x.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
                            x.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.ButtonHex());
                            x.MouseEnter += Workspace.UI.Siticon.MouseEnter;
                            x.MouseLeave += Workspace.UI.Siticon.MouseLeave;
                        }
                        else
                            x.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
                    }
                    if (x is CheckBox)
                        x.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
                }
                
                using (var reader = new StringReader(Workspace.Encryption.Compression.Decompress(File.ReadAllText($"{global.GalaxyRoamingFolder}\\Key.config"))))
                {
                    int keydays = 0;
                    for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                        keydays++;
                    label14.Text = $"Key Days: {keydays}";
                }
            });
        }
        private void FortniteInstallLabel_MouseClick(object sender, MouseEventArgs e) => FolderIcon.Focus();
        private void FortniteInstallLabel_MouseDoubleClick(object sender, MouseEventArgs e) => FolderIcon.Focus();
        private void FolderIcon_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderselection = new FolderBrowserDialog();
            folderselection.Description = "Select Your Fortnite Install Location!";
            folderselection.UseDescriptionForTitle = true;
            if (folderselection.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(folderselection.SelectedPath + "\\global.ucas"))
                {
                    Properties.Settings.Default.FortniteInstall = folderselection.SelectedPath;
                    Properties.Settings.Default.Save();
                    Workspace.global.InstallLoc = Properties.Settings.Default.FortniteInstall;
                    FortniteInstallLabel.Text = folderselection.SelectedPath;
                }
                else
                {
                    MessageBox.Show(this, "The Selected Location Does Not Contain global.ucas!\nMake Sure You Selected Your Paks Folder.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void RemoveDupedUcasButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(Workspace.global.InstallLoc + "\\Galaxy Swapper v2"))
                    Directory.Delete(Workspace.global.InstallLoc + "\\Galaxy Swapper v2", true);
                if (Directory.Exists(Workspace.global.InstallLoc + "\\~GalaxyLobby"))
                    Directory.Delete(Workspace.global.InstallLoc + "\\~GalaxyLobby", true);
                if (File.Exists(Workspace.global.GalaxyRoamingFolder + "\\Swaplogs.log"))
                    File.WriteAllText(Workspace.global.GalaxyRoamingFolder + "\\Swaplogs.log", string.Empty);
            }
            catch (Exception error)
            {
                MessageBox.Show(this, $"Caught A Error While Trying To Delete Duped Ucas!\n{error.ToString()}\nPlease Restart The Swapper And Try Again.", "Caught A Error Please Read!", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                return;
            }
            MessageBox.Show(this, Workspace.ApiReturns.LanReturn("RemoveDupedUcasMessage"), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void VerifyFNButton_Click(object sender, EventArgs e)
        {
            string PakFolder = Workspace.global.InstallLoc;
            System.IO.DirectoryInfo di = new DirectoryInfo(PakFolder);
            foreach (FileInfo file in di.GetFiles())
            {
                if (file.Extension == ".ucas")
                {
                    var Matching = FindMatchingLength(file.Length, file.Name);
                    if (Matching != null)
                    {
                        //So Which Ever File Is Smaller Will Be Our Normal Ucas And Which Ever Is Bigger Name Length Is Duped
                        if (Matching.Length > file.Name.Length)
                        {
                            try
                            {
                                File.Delete(PakFolder + "\\" + Matching);
                                if (File.Exists(PakFolder + "\\" + Matching.Replace(".ucas", ".utoc")))
                                    File.Delete(PakFolder + "\\" + Matching.Replace(".ucas", ".utoc"));
                                if (File.Exists(PakFolder + "\\" + Matching.Replace(".ucas", ".pak")))
                                    File.Delete(PakFolder + "\\" + Matching.Replace(".ucas", ".pak"));
                                if (File.Exists(PakFolder + "\\" + Matching.Replace(".ucas", ".sig")))
                                    File.Delete(PakFolder + "\\" + Matching.Replace(".ucas", ".sig"));
                            }
                            catch (Exception error)
                            {
                                MessageBox.Show(this,$"Caught A Error While Trying To Delete Duped Ucas!\n{error.ToString()}\nPlease Restart The Swapper And Try Again.", "Caught A Error Please Read!", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                                return;
                            }
                        }
                    }
                }
            }
            //Finds Any Ucas,Utocs,Paks,Sigs Files In Any Other Directory Then Pak Folder And Deletes Them
            foreach (DirectoryInfo PakDirList in di.GetDirectories())
            {
                try
                {
                    foreach (string Paks in Directory.EnumerateFiles(PakDirList.FullName, "*.pak*", SearchOption.AllDirectories))
                        File.Delete(Paks);
                    foreach (string Sigs in Directory.EnumerateFiles(PakDirList.FullName, "*.sig*", SearchOption.AllDirectories))
                        File.Delete(Sigs);
                    foreach (string Ucas in Directory.EnumerateFiles(PakDirList.FullName, "*.ucas*", SearchOption.AllDirectories))
                        File.Delete(Ucas);
                    foreach (string Utocs in Directory.EnumerateFiles(PakDirList.FullName, "*.utoc*", SearchOption.AllDirectories))
                        File.Delete(Utocs);
                }
                catch (Exception error)
                {
                    MessageBox.Show(this, $"Caught A Error While Trying To Delete Duped Ucas!\n{error.ToString()}\nPlease Restart The Swapper And Try Again.", "Caught A Error Please Read!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (File.Exists(Workspace.global.GalaxyRoamingFolder + "\\Swaplogs.log"))
                File.WriteAllText(Workspace.global.GalaxyRoamingFolder + "\\Swaplogs.log", string.Empty);
            Workspace.global.UrlStart("com.epicgames.launcher://apps/Fortnite?action=verify&silent=false");
            MessageBox.Show(this, Workspace.ApiReturns.LanReturn("VerifyMessage"), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static string FindMatchingLength(long length, string CurrentPakName)
        {
            string PakFolder = Workspace.global.InstallLoc;
            System.IO.DirectoryInfo di = new DirectoryInfo(PakFolder);

            foreach (FileInfo file in di.GetFiles())
            {
                if (file.Extension == ".ucas")
                {
                    if (file.Length == length)
                        if (file.Name != CurrentPakName)
                            return file.Name;
                }
            }
            return null;
        }
        private void label2_Click(object sender, EventArgs e)
        {
            new Workspace.Forms.ThemeEditor(Dashboard).ShowDialog();
        }
        private void ResetLogsButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, Workspace.ApiReturns.LanReturn("ResetLogsMsg1"), string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (File.Exists(Workspace.global.GalaxyRoamingFolder + "\\Swaplogs.log"))
                    File.WriteAllText(Workspace.global.GalaxyRoamingFolder + "\\Swaplogs.log", string.Empty);
                MessageBox.Show(this, Workspace.ApiReturns.LanReturn("ResetLogsMsg2"), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void label3_Click(object sender, EventArgs e)
        {
            Workspace.global.ResetCosmeticTabs();
            MessageBox.Show(this, Workspace.ApiReturns.LanReturn("RefreshCosmetic"), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ConvertedItemListButton_Click(object sender, EventArgs e)
        {
            string LogsFile = File.ReadAllText(Workspace.global.GalaxyRoamingFolder + "\\Swaplogs.log");
            if (LogsFile.Length == 0)
                MessageBox.Show(this, Workspace.ApiReturns.LanReturn("NoSwappedLogsMsg"), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(this, LogsFile, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void CloseFNButton_Click(object sender, EventArgs e)
        {
            Workspace.global.CloseFN();
            MessageBox.Show(this, Workspace.ApiReturns.LanReturn("CloseFN"), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void StartFNButton_Click(object sender, EventArgs e)
        {
            Workspace.global.UrlStart("com.epicgames.launcher://apps/Fortnite?action=launch&silent=true");
            this.ParentForm.Visible = false;
            while (true)
            {
                Process[] ProcessID = Process.GetProcessesByName("FortniteClient-Win64-Shipping");
                if (ProcessID.Length == 0)
                    Thread.Sleep(1000);
                else
                {
                    ProcessID[0].WaitForExit();
                    Process.GetProcessesByName("EpicGamesLauncher")[0].Kill();
                    this.ParentForm.Visible = true;
                    return;
                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Workspace.Other.SettingsController.EditSettings("DiscordRPC", checkBox1.Checked.ToString().ToLower());
                if (Workspace.Other.RPC.Client != null)
                {
                    Workspace.Other.RPC.Client.ClearPresence();
                    Workspace.Other.RPC.Client.Dispose();
                }
            }
            else
            {
                Workspace.Other.SettingsController.EditSettings("DiscordRPC", checkBox1.Checked.ToString().ToLower());
                DialogResult dialogResult = MessageBox.Show(this, Workspace.ApiReturns.LanReturn("DiscordRPCRestart"), string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Yes)
                    Application.Restart();
            }
            Workspace.Other.Colors.Settingsfile = string.Empty;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Workspace.Other.SettingsController.EditSettings("EmoteWarning", checkBox2.Checked.ToString().ToLower());
            Workspace.Other.Colors.Settingsfile = string.Empty;
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Workspace.Other.SettingsController.EditSettings("HideNonePerfectDefaultSwaps", checkBox3.Checked.ToString().ToLower());
            Workspace.Other.Colors.Settingsfile = string.Empty;
            Workspace.Controls.ControlsSave.Skins.Controls.Clear();
        }
    }
}
