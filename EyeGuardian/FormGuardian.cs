using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EyeGuardian
{
    public partial class FormGuardian : Form, IMessageFilter
    {
        public FormGuardian()
        {
            InitializeComponent();

            ProgBar_Gen();
        }

        #region Copyright

        public string Copyright
        {
            get
            {
                return lbl_Copyright.Text;
            }
            set
            {
                lbl_Copyright.Text = value;
            }
        }

        #endregion

        #region Use customized progressBar control

        ProgressBar cusProgBar = null;

        public void ProgBar_Gen()
        {
            cusProgBar = new CusProgBar();

            Controls.Add(cusProgBar);


            cusProgBar.ForeColor = Color.Azure;
            cusProgBar.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            cusProgBar.Location = new Point(12, 186);
            cusProgBar.Size = new Size(500, 10);
            cusProgBar.Visible = true;
        }

        public int CusProgBarNoAniValue
        {
            get
            {
                return cusProgBar.Value;
            }
            set
            {
                cusProgBar.Value = value;
            }
        }

        #endregion

        #region Use native progressBar control

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern uint SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

        private const int WM_USER = 0x400;
        private const int PBM_SETSTATE = WM_USER + 16;

        private enum ProgressBarState
        {
            PBST_NORMAL = 1,
            PBST_ERROR,
            PBST_PAUSED,
        }

        private const int iProgressBarNoAniMethod = 1;

        public int ProgressBarNoAniValue
        {
            get
            {
                return progressBar1.Value;
            }
            set
            {
                if (iProgressBarNoAniMethod == 1)
                {
                    SendMessage(progressBar1.Handle, PBM_SETSTATE, (int)ProgressBarState.PBST_PAUSED, 0);

                    progressBar1.Value = value;

                    SendMessage(progressBar1.Handle, PBM_SETSTATE, (int)ProgressBarState.PBST_NORMAL, 0);
                }
                else if (iProgressBarNoAniMethod == 2)
                {
                    // To get around this animation, we need to move the progress bar backwards.
                    if (value == progressBar1.Maximum)
                    {
                        // Special case (can't set value > Maximum).
                        progressBar1.Value = value;           // Set the value
                        progressBar1.Value = value - 1;       // Move it backwards
                    }
                    else
                    {
                        progressBar1.Value = value + 1;       // Move past
                    }
                    progressBar1.Value = value;               // Move to correct value
                }
            }
        }

        #endregion

        public bool PreFilterMessage(ref Message m)
        {
            //if (m.Msg == (int)WM.LBUTTONDOWN || m.Msg == (int)WM.LBUTTONUP || m.Msg == (int)WM.LBUTTONDBLCLK) return true;
            //if (m.Msg == (int)WM.RBUTTONDOWN || m.Msg == (int)WM.RBUTTONUP || m.Msg == (int)WM.RBUTTONDBLCLK) return true;
            //if (m.Msg == (int)WM.MBUTTONDOWN || m.Msg == (int)WM.MBUTTONUP || m.Msg == (int)WM.MBUTTONDBLCLK) return true;
            //if (m.Msg == (int)WM.KEYDOWN || m.Msg == (int)WM.KEYUP || m.Msg == (int)WM.SYSKEYDOWN || m.Msg == (int)WM.SYSKEYUP) return true;

            return false;
        }

        #region Disable cursor display

        Rectangle BoundRect;
        Rectangle OldRect = Rectangle.Empty;

        public void GoMouseDisable(bool blDisable = true)
        {
            if (blDisable)
            {
                OldRect = Cursor.Clip;
                // Arbitrary location.
                BoundRect = new Rectangle(50, 50, 1, 1);
                Cursor.Clip = BoundRect;
                Cursor.Hide();
                Application.AddMessageFilter(this);
            }
            else
            {
                Cursor.Clip = OldRect;
                Cursor.Show();
                Application.RemoveMessageFilter(this);
            }
        }

        #endregion

        #region Control callbacks

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
