using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Galaxy_Swapper_v2.Workspace.UI
{
    public class Siticon
    {
        public static void SetHover(Control Lbl)
        {
            Lbl.MouseEnter += MouseEnter;
            Lbl.MouseLeave += MouseLeave;
        }
        public static void SetForm(Form UIComp)
        {
            SetShadowForm(UIComp);
            SetDrag(UIComp);
            SetEclipse(UIComp);
            UIComp.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
        }
        public static void SetShadowForm(Control UIComp)
        {
            var Frm = UIComp as Form;
            Siticone.UI.WinForms.SiticoneShadowForm Shadow = new Siticone.UI.WinForms.SiticoneShadowForm();
            Shadow.SetShadowForm(Frm);
        }
        public static void SetDrag(Control UIComp)
        {
            Siticone.UI.WinForms.SiticoneDragControl Drag = new Siticone.UI.WinForms.SiticoneDragControl();
            Drag.SetDrag(UIComp);
        }
        public static void SetEclipse(Control UIComp)
        {
            Siticone.UI.WinForms.SiticoneElipse Eclipse = new Siticone.UI.WinForms.SiticoneElipse();
            Eclipse.BorderRadius = 9;
            Eclipse.SetElipse(UIComp);
        }
        public static void SetEclipse2(Control UIComp, int Amount)
        {
            Siticone.UI.WinForms.SiticoneElipse Eclipse = new Siticone.UI.WinForms.SiticoneElipse();
            Eclipse.BorderRadius = Amount;
            Eclipse.SetElipse(UIComp);
        }
        public static void MouseEnter(object sender, EventArgs e)
        {
            var lbl = sender as Label;
            lbl.BackColor = Color.FromArgb(69, 74, 107);
        }
        public static void MouseLeave(object sender, EventArgs e)
        {
            var lbl = sender as Label;
            lbl.BackColor = ColorTranslator.FromHtml(Workspace.Other.Colors.ButtonHex());
        }
    }
}
