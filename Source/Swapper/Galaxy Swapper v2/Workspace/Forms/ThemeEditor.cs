using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Galaxy_Swapper_v2.Workspace.Forms
{
    public partial class ThemeEditor : Form
    {
        Dashboard Dashboard;
        public ThemeEditor(Dashboard Dsh)
        {
            InitializeComponent();
            LoadColors();
            Workspace.UI.Siticon.SetForm(this);
            Workspace.UI.Siticon.SetDrag(Dragbar);
            Dashboard = Dsh;
            foreach (Control x in this.Controls)
            {
                if (x is Label)
                {
                    if (x.Font.Size == 9)
                    {
                        x.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.ButtonHex());
                        x.MouseEnter += Workspace.UI.Siticon.MouseEnter;
                        x.MouseLeave += Workspace.UI.Siticon.MouseLeave;
                        Workspace.UI.Siticon.SetEclipse(x);
                    }
                    else
                        x.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
                }
                if (x is Panel)
                {
                    if (x.Height != 26)
                        x.Click += ClrPnl_Click;
                }
            }
            trackBar1.Value = Workspace.Other.Colors.IconSize();
            if (trackBar1.Value == 80)
                label11.Text = $"Cosmetic Icons Size:{trackBar1.Value} (Default)";
            else
                label11.Text = $"Cosmetic Icons Size:{trackBar1.Value}";
        }
        public void LoadColors()
        {
            this.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.MHex());
            Dragbar.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SHex());
            MHex.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.MHex());
            SHex.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SHex());
            ButtonHex.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.ButtonHex());
            TextHex.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            SecTextHex.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SecTextHex());
            HHex.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.HHex());
            foreach (Control x in this.Controls)
            {
                if (x is Label)
                {
                    if (x.Font.Size != 9)
                        x.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
                }
            }
        }
        public void ClrPnl_Click(object sender, EventArgs e)
        {
            using (ColorDialog a = new ColorDialog())
            {
                if (a.ShowDialog() == DialogResult.OK)
                {
                    ((Panel)sender).BackColor = a.Color;
                    Workspace.Other.SettingsController.EditSettings(((Panel)sender).Name, ColorTranslator.ToHtml(((Panel)sender).BackColor));
                    Workspace.Other.Colors.Settingsfile = string.Empty;
                    LoadColors();
                    Dashboard.Loadcolors(this);
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            File.Delete($"{global.GalaxyRoamingFolder}\\Settings.config");
            File.WriteAllText($"{global.GalaxyRoamingFolder}\\Settings.config", Workspace.Other.SettingsController.SettingsBaseRaw());
            Workspace.Other.Colors.Settingsfile = string.Empty;
            LoadColors();
            Dashboard.Loadcolors(this);
            trackBar1.Value = 80;
            Workspace.Other.SettingsController.EditSettings("CosmeticIconSize", trackBar1.Value.ToString());
            Workspace.Other.Colors.Settingsfile = string.Empty;
            Workspace.global.ResetCosmeticTabs();
            label11.Text = $"Cosmetic Icons Size:{trackBar1.Value} (Default)";
            MessageBox.Show(this, Workspace.ApiReturns.LanReturn("ResetSettings"), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void CloseBox_Click(object sender, EventArgs e) => this.Close();
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar1.Value == 80)
                label11.Text = $"Cosmetic Icons Size:{trackBar1.Value} (Default)";
            else
                label11.Text = $"Cosmetic Icons Size:{trackBar1.Value}";
            Workspace.Other.SettingsController.EditSettings("CosmeticIconSize", trackBar1.Value.ToString());
            Workspace.Other.Colors.Settingsfile = string.Empty;
            Workspace.global.ResetCosmeticTabs();
        }

        private void ThemeEditor_Load(object sender, EventArgs e)
        {
            Task.Run(() => {
                Workspace.Other.RPC.SetRPC("Theme Editor");
            });
        }
    }
}
