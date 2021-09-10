using Galaxy_Swapper_v2.Workspace.UI;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Galaxy_Swapper_v2
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            Siticon.SetForm(this);
            Siticon.SetDrag(Sidebar);
            Loadcolors(this);
            DashboardTabPnl.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.HHex());
            UsercntrlHolder.Controls.Add(new Workspace.Controls.Dashboardcntrl());
            Workspace.Other.RPC.StartRPC();
        }

        public void Loadcolors(Form FrmFrom)
        {
            this.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.MHex());
            Sidebar.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SHex());
            DashboardTabLabel.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            SkinsTabLabel.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            BackblingsTabLabel.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            PickaxesTabLabel.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            EmotesTabLabel.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            WrapsTabLabel.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            MiscTabLabel.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            SettingsTabLabel.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            DiscordTabLabel.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            Seperater.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SecTextHex());
            ExtraL.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SecTextHex());
            if (FrmFrom.Name == "ThemeEditor")
            {
                UsercntrlHolder.Controls.Clear();
                UsercntrlHolder.Controls.Add(new Workspace.Controls.Settingcntrl(this));
            }
        }

        public void ResetColors()
        {
            foreach (Control x in MainTabs.Controls)
            {
                if (x is Panel)
                    x.BackColor = Color.Transparent;
            }
            foreach (Control x in panel1.Controls)
            {
                if (x is Panel)
                    x.BackColor = Color.Transparent;
            }
        }
        private void Dashboard_Click(object sender, EventArgs e)
        {
            ResetColors();
            DashboardTabPnl.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.HHex());
            UsercntrlHolder.Controls.Clear();
            UsercntrlHolder.Controls.Add(new Workspace.Controls.Dashboardcntrl());
            Workspace.Other.RPC.SetRPC("Dashboard");
        }

        private void Skin_Click(object sender, EventArgs e)
        {
            ResetColors();
            SkinsTabPnl.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.HHex());
            UsercntrlHolder.Controls.Clear();
            UsercntrlHolder.Controls.Add(new Workspace.Controls.Cosmeticcntrl("Skin"));
            Workspace.Other.RPC.SetRPC("Dashboard | Skins");
        }

        private void Backbling_Click(object sender, EventArgs e)
        {
            ResetColors();
            BackblingsTabPnl.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.HHex());
            UsercntrlHolder.Controls.Clear();
            UsercntrlHolder.Controls.Add(new Workspace.Controls.Cosmeticcntrl("Backbling"));
            Workspace.Other.RPC.SetRPC("Dashboard | Backblings");
        }

        private void Pickaxe_Click(object sender, EventArgs e)
        {
            ResetColors();
            PickaxesTabPnl.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.HHex());
            UsercntrlHolder.Controls.Clear();
            UsercntrlHolder.Controls.Add(new Workspace.Controls.Cosmeticcntrl("Pickaxe"));
            Workspace.Other.RPC.SetRPC("Dashboard | Pickaxes");
        }

        private void Emote_Click(object sender, EventArgs e)
        {
            ResetColors();
            EmotesTabPnl.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.HHex());
            UsercntrlHolder.Controls.Clear();
            UsercntrlHolder.Controls.Add(new Workspace.Controls.Cosmeticcntrl("Emote"));
            Workspace.Other.RPC.SetRPC("Dashboard | Emotes");
            Task.Run(() => {
                CheckForIllegalCrossThreadCalls = false;
                if (Workspace.Other.Colors.EmoteWarning() != "true")
                    MessageBox.Show(this, Workspace.ApiReturns.LanReturn("EmoteWarning"), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }

        private void Wrap_Click(object sender, EventArgs e)
        {
            ResetColors();
            WrapsTabPnl.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.HHex());
            UsercntrlHolder.Controls.Clear();
            UsercntrlHolder.Controls.Add(new Workspace.Controls.Cosmeticcntrl("Wrap"));
            Workspace.Other.RPC.SetRPC("Dashboard | Wraps");
        }

        private void Misc_Click(object sender, EventArgs e)
        {
            ResetColors();
            MiscTabPnl.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.HHex());
            UsercntrlHolder.Controls.Clear();
            UsercntrlHolder.Controls.Add(new Workspace.Controls.Cosmeticcntrl("Misc"));
            Workspace.Other.RPC.SetRPC("Dashboard | Misc");
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            ResetColors();
            SettingsTabPnl.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.HHex());
            UsercntrlHolder.Controls.Clear();
            UsercntrlHolder.Controls.Add(new Workspace.Controls.Settingcntrl(this));
            Workspace.Other.RPC.SetRPC("Dashboard | Settings");
        }
        private void Discord_Click(object sender, EventArgs e) => Workspace.global.UrlStart(Workspace.ApiReturns.StatusReturn("Discordinvite"));
        private void CloseBox_Click(object sender, EventArgs e) => this.Close();
        private void MinimizedBox_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;
    }
}
