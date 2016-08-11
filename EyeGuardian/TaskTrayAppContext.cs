using System;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace EyeGuardian
{
    public class TaskTrayAppContext : ApplicationContext
    {
        private NotifyIcon _oNotifyIcon = null;
        private FormConf _oFormConf = null;

        private MenuItem _oMItemRest = null;
        private MenuItem _oMItemWork = null;
        private MenuItem _oMItemConf = null;
        private MenuItem _oMItemAbout = null;
        private MenuItem _oMItemExit = null;

        private Timer _oTimer = null;

        private ManualResetEvent _oSyncCallback_DClick = null;

        private int _iCntClick = 0;

        public TaskTrayAppContext()
        {
            _oNotifyIcon = new NotifyIcon();
            _oFormConf = new FormConf();

            _oMItemRest = new MenuItem("Rest", new EventHandler(Callback_RestNow));
            _oMItemWork = new MenuItem("Work", new EventHandler(Callback_WorkNow));
            _oMItemConf = new MenuItem("Config", new EventHandler(Callback_ConfShow));
            _oMItemAbout = new MenuItem("About", new EventHandler(Callback_AboutShow));
            _oMItemExit = new MenuItem("Exit", new EventHandler(Callback_Exit));

            _oNotifyIcon.Icon = Properties.Resources.AppIcon;
            _oNotifyIcon.MouseMove += new MouseEventHandler(Callback_TipShow);
            _oNotifyIcon.MouseDown += new MouseEventHandler(Callback_MLBDown);
            _oNotifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { _oMItemRest, _oMItemWork, _oMItemConf, _oMItemAbout, _oMItemExit });
            _oNotifyIcon.Visible = true;

            _oSyncCallback_DClick = new ManualResetEvent(false);

            _oTimer = Timer_Gen(0);
        }

        private Timer Timer_Gen(int iReserved)
        {
            return new Timer(_ => Callback_Timer(iReserved), null, Timeout.Infinite, Timeout.Infinite);
        }

        private void Callback_Timer(int iReserved)
        {

            if (_iCntClick < 2)
            {
                _iCntClick = 0;
                BalloonTipShow();
            }
            else
            {
                _iCntClick = 0;
                _oSyncCallback_DClick.WaitOne();
                _oSyncCallback_DClick.Reset();
                Callback_RestNow(_oNotifyIcon, new EventArgs());
            }
        }

        private void Callback_MLBDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _iCntClick++;
                if (_iCntClick < 2)
                {
                    _oTimer.Change(300, Timeout.Infinite);
                }
                else
                {
                    _oTimer.Change(0, Timeout.Infinite);
                    _oSyncCallback_DClick.Set();
                }
            }
        }

        private void Callback_TipShow(object sender, MouseEventArgs e)
        {
            if (Program.blResting)
            { _oNotifyIcon.Text = string.Format("Work after {0}", TimeLib.Millisecond2String(Program.iTimeRested)); }
            else
            { _oNotifyIcon.Text = string.Format("Rest after {0}", TimeLib.Millisecond2String(Program.iTimeWorked)); }
        }

        private void BalloonTipShow()
        {
            string sTipText = string.Format("Rested for {0};\nWorked for {1};",
                TimeLib.Second2String((int)Program.fSecRested_All),
                TimeLib.Second2String((int)Program.fSecWorked_All));

            _oNotifyIcon.ShowBalloonTip(3000, "You have already:", sTipText, ToolTipIcon.Info);
        }

        void Callback_RestNow(object sender, EventArgs e)
        {
            if (sender == _oMItemRest ||
                (sender == _oNotifyIcon && Properties.Settings.Default.blRestOnDoubleClick))
            {
                Program.RestNow();
            }
        }

        void Callback_WorkNow(object sender, EventArgs e)
        {
            if (sender == _oMItemRest ||
                (sender == _oNotifyIcon && Properties.Settings.Default.blWorkOnDoubleClick))
            {
                Program.WorkNow();
            }
        }

        void Callback_ConfShow(object sender, EventArgs e)
        {
            // If we are already showing the window meerly focus it.
            if (_oFormConf.Visible)
                //configWindow.Focus();
                _oFormConf.Activate();
            else
                _oFormConf.ShowDialog();
        }

        void Callback_AboutShow(object sender, EventArgs e)
        {
            MessageBox.Show(Program.sAppName + " v" + Program.sAppVer + "\n" + Program.sAppDescription + "\n" + Program.sAppCopyright, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void Callback_Exit(object sender, EventArgs e)
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            _oNotifyIcon.Visible = false;

            Application.Exit();
        }
    }
}
