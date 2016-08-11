using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace EyeGuardian
{
    static class Program
    {
        private static Assembly _oAssembly = null;
        private static AssemblyName _oAssemblyName = null;
        private static string _sAssemblyLocation = null;
        private static FileVersionInfo _oAssemblyFileVersionInfo = null;

        public static Assembly oAssembly
        {
            get { return _oAssembly; }
        }

        public static AssemblyName oAssemblyName
        {
            get { return _oAssemblyName; }
        }

        public static string oAssemblyLocation
        {
            get { return _sAssemblyLocation; }
        }

        public static FileVersionInfo oAssemblyFileVersionInfo
        {
            get { return _oAssemblyFileVersionInfo; }
        }

        public static string sAppName = null;
        public static string sAppVer = null;
        public static string sAppDescription = null;
        public static string sAppCopyright = null;
        public static string sAppCompany = null;

        private static double _fSecRested_All = 0.0;
        private static double _fSecWorked_All = 0.0;

        private static int _iTime4Rest_Frac = 0;
        private static int _iProgress4Rest = 0;
        private const int _iProgress4Rest_Max = 100;

        private const int _iTime4Work_Frac = 1000;
        private static int _iProgress4Work = 0;
        private static int _iProgress4Work_Max = 0;

        public static double fSecRested_All
        {
            get { return _fSecRested_All; }
            set { _fSecRested_All = value; }
        }
        public static double fSecWorked_All
        {
            get { return _fSecWorked_All; }
            set { _fSecWorked_All = value; }
        }

        private static int iTime4Rest
        {
            get { return Properties.Settings.Default.iMillisecond2Rest; }
        }
        private static int iTime4Work
        {
            get { return Properties.Settings.Default.iMillisecond2Work; }
        }

        public static int iTimeRested
        {
            get { return _iTime4Rest_Frac * (_iProgress4Rest_Max - _iProgress4Rest); }
        }
        public static int iTimeWorked
        {
            get { return _iTime4Work_Frac * (_iProgress4Work_Max - _iProgress4Work); }
        }

        private static bool _blFormG_Shown = false;
        private static bool _blFormG_Live = false;
        private static bool _blFormG_Switch = false;
        private static bool _blFormG_Interupt = false;
        private static bool _blFormG_Interupted = false;

        public static bool blResting
        {
            get { return _blFormG_Shown; }
        }

        private static ManualResetEvent _oSyncEvent_FormG_Switched = null;

        private static FormGuardian _oFormG = null;

        private static Thread _oThread_FormG = null;

        private static Timer _oTimer = null;

        private static WinGlobalHook _oWinGlobalHook = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (IsInstanceRunning())
            {
                MessageBox.Show("An instance is already running!");
                return;
            }

            AppInfoFill();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (_oWinGlobalHook = new WinGlobalHook())
            {
                _blFormG_Live = true;

                _oSyncEvent_FormG_Switched = new ManualResetEvent(false);

                _oThread_FormG = Thread_FormG_Gen(0);
                _oThread_FormG.Start();

                using (_oTimer = Timer_Gen(0))
                {
                    _iProgress4Work_Max = iTime4Work / _iTime4Work_Frac;

                    _oTimer.Change(0, Timeout.Infinite);

                    Application.Run(new TaskTrayAppContext());

                    _oTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }

                _blFormG_Live = false;

                if (_oThread_FormG.IsAlive)
                {
                    _oThread_FormG.Join();
                }
            }
        }

        private static bool IsInstanceRunning()
        {
            return Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Length > 1;
        }

        private static void AppInfoFill(bool blFileVersion = false)
        {
            _oAssembly = Assembly.GetExecutingAssembly();
            _oAssemblyName = oAssembly.GetName();
            _sAssemblyLocation = oAssembly.Location;
            _oAssemblyFileVersionInfo = FileVersionInfo.GetVersionInfo(oAssemblyLocation);

            if (blFileVersion)
            {
                sAppName = oAssemblyFileVersionInfo.ProductName;
                sAppVer = oAssemblyFileVersionInfo.ProductVersion;
                sAppDescription = oAssemblyFileVersionInfo.FileDescription;
                sAppCopyright = oAssemblyFileVersionInfo.LegalCopyright;
                sAppCompany = oAssemblyFileVersionInfo.CompanyName;
            }
            else
            {
                sAppName = oAssemblyName.Name;
                sAppVer = oAssemblyName.Version.ToString();
                sAppDescription = ((AssemblyDescriptionAttribute)oAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
                sAppCopyright = ((AssemblyCopyrightAttribute)oAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
                sAppCompany = ((AssemblyCompanyAttribute)oAssembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)[0]).Company;
            }
        }

        private static Timer Timer_Gen(int iReserved)
        {
            return new Timer(_ => Callback_Timer(iReserved), null, Timeout.Infinite, Timeout.Infinite);
        }

        private static void Callback_Timer(int iReserved)
        {
            if (_blFormG_Interupted)
            {
                _blFormG_Interupted = false;

                _iProgress4Work = 0;
                _iProgress4Work_Max = iTime4Work / _iTime4Work_Frac;

                _oTimer.Change(_iTime4Work_Frac, Timeout.Infinite);
            }
            else
            {
                if (_blFormG_Shown)
                {
                    if (_iProgress4Rest < _iProgress4Rest_Max)
                    {
                        _iProgress4Rest++;

                        fSecRested_All += _iTime4Rest_Frac / 1000.0;

                        _oTimer.Change(_iTime4Rest_Frac, Timeout.Infinite);
                    }
                    else
                    {
                        _iProgress4Work = 0;
                        _iProgress4Work_Max = iTime4Work / _iTime4Work_Frac;

                        _blFormG_Switch = true;
                        _oSyncEvent_FormG_Switched.WaitOne();
                        _oSyncEvent_FormG_Switched.Reset();

                        _oTimer.Change(_iTime4Work_Frac, Timeout.Infinite);
                    }
                }
                else
                {
                    if (_iProgress4Work < _iProgress4Work_Max)
                    {
                        _iProgress4Work++;

                        fSecWorked_All += _iTime4Work_Frac / 1000.0;

                        _oTimer.Change(_iTime4Work_Frac, Timeout.Infinite);
                    }
                    else
                    {
                        _iProgress4Rest = 0;
                        _iTime4Rest_Frac = iTime4Rest / _iProgress4Rest_Max;

                        _blFormG_Switch = true;
                        _oSyncEvent_FormG_Switched.WaitOne();
                        _oSyncEvent_FormG_Switched.Reset();

                        _oTimer.Change(_iTime4Rest_Frac, Timeout.Infinite);
                    }
                }

            }
        }

        private static Thread Thread_FormG_Gen(int iReserved)
        {
            return new Thread(_ => Callback_Thread_FormG(iReserved));
        }

        private static void Callback_Thread_FormG(int iReserved)
        {
            _oFormG = new FormGuardian();
            _oFormG.Copyright = sAppCopyright;
            WinForm.GoFullscreen(_oFormG, true);
            WinForm.GoSemiTrans(_oFormG, true);

            int iProgBarValue = 0;

            while (_blFormG_Live)
            {
                if (_oFormG.IsDisposed)
                {
                    _oFormG = new FormGuardian();
                    _oFormG.Copyright = sAppCopyright;
                    WinForm.GoFullscreen(_oFormG, true);
                    WinForm.GoSemiTrans(_oFormG, true);

                    _blFormG_Shown = false;
                    _blFormG_Interupted = true;

                    _oTimer.Change(0, Timeout.Infinite);
                }
                else if (_blFormG_Interupt)
                {
                    _blFormG_Interupt = false;

                    if (_oFormG.Visible)
                    {
                        _oFormG.Hide();

                        _blFormG_Shown = false;
                        _blFormG_Interupted = true;

                        _oTimer.Change(0, Timeout.Infinite);
                    }
                }
                else
                {
                    if (_blFormG_Switch)
                    {
                        _blFormG_Switch = false;

                        if (_oFormG.Visible)
                        {
                            _oFormG.Hide();
                            GoWinOp(false);
                            _blFormG_Shown = false;
                            _oSyncEvent_FormG_Switched.Set();
                        }
                        else
                        {
                            GoWinOp(true);
                            _oFormG.Show();
                            _blFormG_Shown = true;
                            _oSyncEvent_FormG_Switched.Set();
                        }
                    }
                    else
                    {
                        if (_iProgress4Rest >= 0 && _iProgress4Rest <= _iProgress4Rest_Max)
                        {
                            iProgBarValue = _iProgress4Rest * 100 / _iProgress4Rest_Max;

                            if (_oFormG.CusProgBarNoAniValue != iProgBarValue)
                            {
                                _oFormG.CusProgBarNoAniValue = iProgBarValue;
                                Debug.WriteLine("PROG ---> {0}", iProgBarValue);
                            }
                        }

                        Application.DoEvents();
                    }
                }

            }

            _oFormG.Dispose();
            //oFormG.Close();
        }

        public static void RestNow(bool blEvenAtRestAlready = false)
        {
            if (_blFormG_Live)
            {
                if (!_blFormG_Shown)
                {
                    _iProgress4Work = _iProgress4Work_Max;
                    _oTimer.Change(0, Timeout.Infinite);
                }
                else if (blEvenAtRestAlready)
                {
                    _iProgress4Rest = 0;
                    _iTime4Rest_Frac = iTime4Rest / _iProgress4Rest_Max;
                    _oTimer.Change(0, Timeout.Infinite);
                }
            }
        }

        public static void WorkNow(bool blEvenAtWorkAlready = false)
        {
            if (_blFormG_Live)
            {
                if (_blFormG_Shown)
                {
                    _iProgress4Rest = _iProgress4Rest_Max;
                    _oTimer.Change(0, Timeout.Infinite);
                }
                else if (blEvenAtWorkAlready)
                {
                    _iProgress4Work = 0;
                    _iProgress4Work_Max = iTime4Work / _iTime4Work_Frac;
                    _oTimer.Change(0, Timeout.Infinite);
                }
            }
        }

        private static void GoWinOp(bool isGo = true)
        {
            //WinGlobalHook.GoMouseHook(isGo);
            _oWinGlobalHook.GoKeyboardHook(isGo);
            _oWinGlobalHook.HookKeyDown += Callback_HookKeyDown;
            _oWinGlobalHook.HookKeyUp += Callback_HookKeyUp;
            //WinGeneral.GoTaskbarHide(isGo);
            //WinGeneral.GoInputBlock(isGo);
            //WinCtrlAltDel.DisableTaskMgr(isGo);
        }

        private static void Callback_HookKeyDown(object sender, WinGlobalHook.HookKeyEventArgs e)
        {
            Debug.WriteLine("HookKeyDown {0}", e.Modifiers);
            _blFormG_Interupt = true;
        }

        private static void Callback_HookKeyUp(object sender, WinGlobalHook.HookKeyEventArgs e)
        {
            Debug.WriteLine("HookKeyUp {0}", e.Modifiers);
        }
    }
}
