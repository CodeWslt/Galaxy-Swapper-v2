using Newtonsoft.Json.Linq;
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
    public partial class Options : Form
    {
        public string CosmeticName;
        public Options(string CosmeticTmp)
        {
            InitializeComponent();
            Workspace.UI.Siticon.SetForm(this);
            Workspace.UI.Siticon.SetDrag(Dragbar);
            this.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.MHex());
            Dragbar.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SHex());
            CosmeticName = CosmeticTmp;
            label1.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            this.Text = CosmeticName;
        }
        private void ButtonClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            new Workspace.Forms.SwapForm(((PictureBox)sender).Name).ShowDialog();
            this.Close();
        }
        public Control IconPicturebox(string Icon, string Name)
        {
            PictureBox PictureApply = new PictureBox();
            PictureApply.Size = new Size(Workspace.Other.Colors.IconSize(), Workspace.Other.Colors.IconSize());
            PictureApply.SizeMode = PictureBoxSizeMode.StretchImage;
            PictureApply.ImageLocation = Icon;
            PictureApply.Name = Name;
            PictureApply.Click += ButtonClick;
            return PictureApply;
        }

        private void Options_Load(object sender, EventArgs e)
        {
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
                    foreach (var Option in Cosmetic["Options"])
                        flowLayoutPanel1.Controls.Add(IconPicturebox(Option["Icon"].ToString(), Option["Name"].ToString()));
                }
            }
            try { Workspace.Other.RPC.SetRPC(CosmeticName); }
            catch { }
        }

        private void CloseBox_Click(object sender, EventArgs e) => this.Close();
    }
}
