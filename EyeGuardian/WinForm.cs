using System.Drawing;
using System.Windows.Forms;

namespace EyeGuardian
{
    class WinForm
    {
        public static void GoFullscreen(Form oFrom, bool blFullScr)
        {
            if (blFullScr)
            {
                oFrom.TopMost = true;
                oFrom.WindowState = FormWindowState.Normal;
                oFrom.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                oFrom.Bounds = Screen.PrimaryScreen.Bounds;
            }
            else
            {
                oFrom.TopMost = false;
                oFrom.WindowState = FormWindowState.Maximized;
                oFrom.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            }
        }

        public static void GoSemiTrans(Form oFrom, bool blSemiTrans)
        {
            if (blSemiTrans)
            {
                oFrom.BackColor = Color.Gray;
                //oFrom.TransparencyKey = Color.Fuchsia;
                oFrom.Opacity = 0.9;
            }
            else
            {
                oFrom.BackColor = default(Color);
                //oFrom.TransparencyKey = Color.Fuchsia;
                oFrom.Opacity = 1;
            }
        }
    }
}
