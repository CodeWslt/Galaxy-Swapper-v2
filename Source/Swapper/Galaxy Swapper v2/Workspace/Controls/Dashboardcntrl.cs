using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Galaxy_Swapper_v2.Workspace.Controls
{
    public partial class Dashboardcntrl : UserControl
    {
        public Dashboardcntrl()
        {
            InitializeComponent();
        }

        private void Dashboardcntrl_Load(object sender, EventArgs e)
        {
            if (!Workspace.Other.Colors.SecTextHex().ToString().Contains("3A3F4B"))
            {
                richTextBox1.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SecTextHex());
                richTextBox2.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SecTextHex());
            }
            foreach (Control x in this.Controls)
            {
                if (x is Label)
                    x.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            }
            if (File.ReadAllText(Workspace.global.GalaxyRoamingFolder + "\\Swaplogs.log").Length == 0)
                richTextBox2.Text = Workspace.ApiReturns.LanReturn("NoSwappedLogsMsg");
            else
                richTextBox2.Text = File.ReadAllText(Workspace.global.GalaxyRoamingFolder + "\\Swaplogs.log");
            Task.Run(() => {
                CheckForIllegalCrossThreadCalls = false;
                try
                {
                    richTextBox1.Text = Workspace.ApiReturns.LanReturn("PatchNotes");
                    label5.Text = Workspace.ApiReturns.LanReturn("DashboardCredits");
                    using (WebClient w = new WebClient())
                    {
                        JObject parse = JObject.Parse(w.DownloadString("https://fortnite-api.com/v2/news/br"));
                        pictureBox1.LoadAsync(parse["data"]["image"].ToString());
                    }
                }
                catch { pictureBox1.LoadAsync("https://www.galaxyswapperv2.com/Icons/InvalidIcon.png"); }
            });
        }
    }
}
