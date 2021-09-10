using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Galaxy_Swapper_v2.Workspace.Controls
{
    public partial class Cosmeticcntrl : UserControl
    {
        string CosmeticType;
        public Cosmeticcntrl(string Type)
        {
            InitializeComponent();
            CosmeticType = Type;
            SearchBarPanel.Location = new Point(868, 0);
            LoadCosm();
            richTextBox1.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SHex());
        }
        private void BarTrans(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            if (SearchBarPanel.Location.X == 868)
            {
                int count = 868;
                while (SearchBarPanel.Location.X != 605)
                    SearchBarPanel.Location = new Point(count -= 1, 0);
                pictureBox1.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.SHex());
            }
            else
            {
                int count = 605;
                while (SearchBarPanel.Location.X != 868)
                    SearchBarPanel.Location = new Point(count += 1, 0);
                pictureBox1.BackColor = Color.Transparent;
            }
            richTextBox1.Focus();
        }
        public void ButtonClick(object sender, EventArgs e)
        {
            string Name = ((PictureBox)sender).Name;
            if (((PictureBox)sender).Name.Contains("(Option)"))
                new Galaxy_Swapper_v2.Workspace.Forms.Options(Name).ShowDialog();
            else
                new Galaxy_Swapper_v2.Workspace.Forms.SwapForm(Name).ShowDialog();
            return;
        }
        public void IdConverterClick(object sender, EventArgs e)
        {
            new Workspace.Forms.ID_Converter().ShowDialog();
        }
        public Control TextBox(string Text)
        {
            Label MeshSkinMsg = new Label();
            MeshSkinMsg.Text = Text;
            MeshSkinMsg.ForeColor = ColorTranslator.FromHtml(Workspace.Other.Colors.TextHex());
            MeshSkinMsg.TextAlign = ContentAlignment.MiddleCenter;
            MeshSkinMsg.AutoSize = false;
            MeshSkinMsg.Size = new Size(820, 38);
            MeshSkinMsg.Font = new Font(MeshSkinMsg.Font.FontFamily, 13);
            return MeshSkinMsg;
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
        public void LoadCosm()
        {
            panel1.Controls.Clear();
            switch (CosmeticType)
            {
                case "Skin":
                    if (CheckIfControlsEmpty(ControlsSave.Skins) == false)
                    {
                        ControlsSave.Skins.AutoScroll = true;
                        ControlsSave.Skins.Dock = DockStyle.Fill;
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
                        ControlsSave.Skins.Controls.Add(TextBox("Mesh Skins | No Bypass!"));
                        foreach (var Cosmetic in parse["Cosmetics"])
                        {
                            if (Cosmetic["Type"].ToString() == "Skin:Mesh")
                            {
                                ControlsSave.Skins.Controls.Add(IconPicturebox(Cosmetic["Icon"].ToString(), Cosmetic["Name"].ToString()));
                            }
                        }
                        ControlsSave.Skins.Controls.Add(TextBox("Special Skins | Backblings Don't Show"));
                        foreach (var Cosmetic in parse["Cosmetics"])
                        {
                            if (Cosmetic["Type"].ToString() == "Skin:Default")
                            {
                                if (Workspace.Other.Colors.DefaultHide() == "true")
                                {
                                    if (Cosmetic["Message"].ToString() == "false")
                                        ControlsSave.Skins.Controls.Add(IconPicturebox(Cosmetic["Icon"].ToString(), Cosmetic["Name"].ToString()));
                                }    
                                else
                                    ControlsSave.Skins.Controls.Add(IconPicturebox(Cosmetic["Icon"].ToString(), Cosmetic["Name"].ToString()));
                            }
                        }
                        panel1.Controls.Add(ControlsSave.Skins);
                    }
                    else
                    {
                        panel1.Controls.Add(ControlsSave.Skins);
                    }
                    break;
                case "Backbling":
                    if (CheckIfControlsEmpty(ControlsSave.Backblings) == false)
                    {
                        ControlsSave.Backblings.AutoScroll = true;
                        ControlsSave.Backblings.Dock = DockStyle.Fill;
                        string Api = string.Empty;
                        if (Environment.UserName == "white")
                        {
                            if (File.Exists(@"C:\Users\white\OneDrive\Desktop\Source\Galaxy Swapper\Galaxy Swapper v2\API\Cosmetics.json"))
                                Api = File.ReadAllText(@"C:\Users\white\OneDrive\Desktop\Source\Galaxy Swapper\Galaxy Swapper v2\API\Cosmetics.json");
                        }
                        else
                            Api = Workspace.Encryption.Compression.Decompress(Workspace.global.ApiReturn("Cosmetics"));
                        JObject parse = JObject.Parse(Api);
                        foreach (var Cosmetic in parse["Cosmetics"])
                        {
                            if (Cosmetic["Type"].ToString() == "Backblings")
                            {
                                ControlsSave.Backblings.Controls.Add(IconPicturebox(Cosmetic["Icon"].ToString(), Cosmetic["Name"].ToString()));
                            }
                        }
                        panel1.Controls.Add(ControlsSave.Backblings);
                    }
                    else
                    {
                        panel1.Controls.Add(ControlsSave.Backblings);
                    }
                    break;
                case "Pickaxe":
                    if (CheckIfControlsEmpty(ControlsSave.Pickaxes) == false)
                    {
                        ControlsSave.Pickaxes.AutoScroll = true;
                        ControlsSave.Pickaxes.Dock = DockStyle.Fill;
                        string Api = string.Empty;
                        if (Environment.UserName == "white")
                        {
                            if (File.Exists(@"C:\Users\white\OneDrive\Desktop\Source\Galaxy Swapper\Galaxy Swapper v2\API\Cosmetics.json"))
                                Api = File.ReadAllText(@"C:\Users\white\OneDrive\Desktop\Source\Galaxy Swapper\Galaxy Swapper v2\API\Cosmetics.json");
                        }
                        else
                            Api = Workspace.Encryption.Compression.Decompress(Workspace.global.ApiReturn("Cosmetics"));
                        JObject parse = JObject.Parse(Api);
                        foreach (var Cosmetic in parse["Cosmetics"])
                        {
                            if (Cosmetic["Type"].ToString() == "Pickaxes")
                            {
                                ControlsSave.Pickaxes.Controls.Add(IconPicturebox(Cosmetic["Icon"].ToString(), Cosmetic["Name"].ToString()));
                            }
                        }
                        panel1.Controls.Add(ControlsSave.Pickaxes);
                    }
                    else
                    {
                        panel1.Controls.Add(ControlsSave.Pickaxes);
                    }
                    break;
                case "Emote":
                    if (CheckIfControlsEmpty(ControlsSave.Pickaxes) == false)
                    {
                        ControlsSave.Emotes.AutoScroll = true;
                        ControlsSave.Emotes.Dock = DockStyle.Fill;
                        string Api = string.Empty;
                        if (Environment.UserName == "white")
                        {
                            if (File.Exists(@"C:\Users\white\OneDrive\Desktop\Source\Galaxy Swapper\Galaxy Swapper v2\API\Cosmetics.json"))
                                Api = File.ReadAllText(@"C:\Users\white\OneDrive\Desktop\Source\Galaxy Swapper\Galaxy Swapper v2\API\Cosmetics.json");
                        }
                        else
                            Api = Workspace.Encryption.Compression.Decompress(Workspace.global.ApiReturn("Cosmetics"));
                        JObject parse = JObject.Parse(Api);
                        foreach (var Cosmetic in parse["Cosmetics"])
                        {
                            if (Cosmetic["Type"].ToString() == "Emote")
                            {
                                ControlsSave.Emotes.Controls.Add(IconPicturebox(Cosmetic["Icon"].ToString(), Cosmetic["Name"].ToString()));
                            }
                        }
                        panel1.Controls.Add(ControlsSave.Emotes);
                    }
                    else
                    {
                        panel1.Controls.Add(ControlsSave.Emotes);
                    }
                    break;
                case "Wrap":
                    if (CheckIfControlsEmpty(ControlsSave.Wrap) == false)
                    {
                        ControlsSave.Wrap.AutoScroll = true;
                        ControlsSave.Wrap.Dock = DockStyle.Fill;
                        string Api = string.Empty;
                        if (Environment.UserName == "white")
                        {
                            if (File.Exists(@"C:\Users\white\OneDrive\Desktop\Source\Galaxy Swapper\Galaxy Swapper v2\API\Cosmetics.json"))
                                Api = File.ReadAllText(@"C:\Users\white\OneDrive\Desktop\Source\Galaxy Swapper\Galaxy Swapper v2\API\Cosmetics.json");
                        }
                        else
                            Api = Workspace.Encryption.Compression.Decompress(Workspace.global.ApiReturn("Cosmetics"));
                        JObject parse = JObject.Parse(Api);
                        foreach (var Cosmetic in parse["Cosmetics"])
                        {
                            if (Cosmetic["Type"].ToString() == "Wraps")
                            {
                                ControlsSave.Wrap.Controls.Add(IconPicturebox(Cosmetic["Icon"].ToString(), Cosmetic["Name"].ToString()));
                            }
                        }
                        panel1.Controls.Add(ControlsSave.Wrap);
                    }
                    else
                    {
                        panel1.Controls.Add(ControlsSave.Wrap);
                    }
                    break;
                case "Misc":
                    if (CheckIfControlsEmpty(ControlsSave.Misc) == false)
                    {
                        ControlsSave.Misc.AutoScroll = true;
                        ControlsSave.Misc.Dock = DockStyle.Fill;
                        string Api = string.Empty;
                        if (Environment.UserName == "white")
                        {
                            if (File.Exists(@"C:\Users\white\OneDrive\Desktop\Source\Galaxy Swapper\Galaxy Swapper v2\API\Cosmetics.json"))
                                Api = File.ReadAllText(@"C:\Users\white\OneDrive\Desktop\Source\Galaxy Swapper\Galaxy Swapper v2\API\Cosmetics.json");
                        }
                        else
                            Api = Workspace.Encryption.Compression.Decompress(Workspace.global.ApiReturn("Cosmetics"));

                        PictureBox PictureApply = new PictureBox();
                        PictureApply.Size = new Size(Workspace.Other.Colors.IconSize(), Workspace.Other.Colors.IconSize());
                        PictureApply.SizeMode = PictureBoxSizeMode.StretchImage;
                        PictureApply.ImageLocation = "https://www.galaxyswapperv2.com/Icons/Idswapper.png";
                        PictureApply.Name = "ID Converter";
                        PictureApply.Click += IdConverterClick;
                        ControlsSave.Misc.Controls.Add(PictureApply);

                        JObject parse = JObject.Parse(Api);
                        foreach (var Cosmetic in parse["Cosmetics"])
                        {
                            if (Cosmetic["Type"].ToString() == "Misc")
                            {
                                ControlsSave.Misc.Controls.Add(IconPicturebox(Cosmetic["Icon"].ToString(), Cosmetic["Name"].ToString()));
                            }
                        }
                        panel1.Controls.Add(ControlsSave.Misc);
                    }
                    else
                    {
                        panel1.Controls.Add(ControlsSave.Misc);
                    }
                    break;
            }
        }
        public static bool CheckIfControlsEmpty(Control ToCheck)
        {
            foreach (Control x in ToCheck.Controls)
            {
                if (x is PictureBox)
                {
                    return true;
                }
            }
            return false;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker tran = new BackgroundWorker())
            {
                tran.DoWork += BarTrans;
                tran.RunWorkerAsync();
            }
        }
        public void LoadCosmeticSearchbar(string CosmeticName)
        {
            panel1.Controls.Clear();
            FlowLayoutPanel Flow = new FlowLayoutPanel();
            Flow.AutoScroll = true;
            Flow.Dock = DockStyle.Fill;
            JObject parse = JObject.Parse(Workspace.Encryption.Compression.Decompress(Workspace.global.ApiReturn("Cosmetics")));
            foreach (var Cosmetic in parse["Cosmetics"])
            {
                if (Cosmetic["Name"].ToString().ToLower().Contains(CosmeticName.ToLower()))
                {
                    if (!Cosmetic["Type"].ToString().ToLower().Contains("option"))
                        Flow.Controls.Add(IconPicturebox(Cosmetic["Icon"].ToString(), Cosmetic["Name"].ToString()));
                }
            }
            panel1.Controls.Add(Flow);
        }
        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (richTextBox1.Text.Length != 0)
            {
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                    LoadCosmeticSearchbar(richTextBox1.Text);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length == 0)
                LoadCosm();
        }
    }
    public class ControlsSave
    {
        public static FlowLayoutPanel Skins = new FlowLayoutPanel();
        public static FlowLayoutPanel Backblings = new FlowLayoutPanel();
        public static FlowLayoutPanel Pickaxes = new FlowLayoutPanel();
        public static FlowLayoutPanel Emotes = new FlowLayoutPanel();
        public static FlowLayoutPanel Wrap = new FlowLayoutPanel();
        public static FlowLayoutPanel Misc = new FlowLayoutPanel();
    }
}
