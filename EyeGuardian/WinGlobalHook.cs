using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace EyeGuardian
{
    public class WinGlobalHook : IDisposable
    {
        public bool IsDisposed { get; private set; }

        private IntPtr _lKeyboardHookID = IntPtr.Zero;
        private IntPtr _lMouseHookID = IntPtr.Zero;

        private WinImport.LowLevelKeyboardProcDel _LowLevelKeyboardProc = null;
        private WinImport.LowLevelMouseProcDel _LowLevelMouseProc = null;

        [Flags]
        public enum ModifierKeys
        {
            /// <summary>Specifies that the key should be treated as is, without any modifier.
            /// </summary>
            None = 0x0000,
            /// <summary>Specifies that the Accelerator key (ALT) is pressed with the key.
            /// </summary>
            Alt = 0x0001,
            /// <summary>Specifies that the Control key is pressed with the key.
            /// </summary>
            Control = 0x0002,
            /// <summary>Specifies that the Shift key is pressed with the associated key.
            /// </summary>
            Shift = 0x0004,
            /// <summary>Specifies that the Window key is pressed with the associated key.
            /// </summary>
            Win = 0x0008,
        }

        public class HookKeyEventArgs : EventArgs
        {
            public Keys KeyData { get; private set; }
            public ModifierKeys Modifiers { get; private set; }

            public bool Alt { get; private set; }
            public bool Control { get; private set; }
            public bool Shift { get; private set; }
            public bool Win { get; private set; }

            public bool Handled { get; private set; }

            public HookKeyEventArgs(Keys KeyData, ModifierKeys Modifiers)
            {
                this.KeyData = KeyData;
                this.Modifiers = Modifiers;

                this.Alt = (Modifiers & ModifierKeys.Alt) != 0;
                this.Control = (Modifiers & ModifierKeys.Control) != 0;
                this.Shift = (Modifiers & ModifierKeys.Shift) != 0;
                this.Win = (Modifiers & ModifierKeys.Win) != 0;

                this.Handled = false;
            }
        }

        /// <summary>
        /// Occurs when one of the hooked keys is pressed
        /// </summary>
        public event HookKeyEventHandler HookKeyDown;
        /// <summary>
        /// Occurs when one of the hooked keys is released
        /// </summary>
        public event HookKeyEventHandler HookKeyUp;
        /// <summary>
        /// Represents the method that will handle a HookKeyEvent
        /// </summary>
        public delegate void HookKeyEventHandler(object sender, HookKeyEventArgs e);

        public WinGlobalHook()
        {
            IsDisposed = false;
            _LowLevelKeyboardProc = LowLevelKeyboardProc;
            _LowLevelMouseProc = LowLevelMouseProc;
        }

        public void GoKeyboardHook(bool blHook = true)
        {
            if (blHook)
            {
                if (_lKeyboardHookID == IntPtr.Zero)
                { _lKeyboardHookID = RegisterLowLevelKeyboardHook(_LowLevelKeyboardProc); }
            }
            else
            {
                if (_lKeyboardHookID != IntPtr.Zero)
                {
                    UnregisterLowLevelHook(_lKeyboardHookID);
                    _lKeyboardHookID = IntPtr.Zero;
                }
            }
        }

        public void GoMouseHook(bool blHook = true)
        {
            if (blHook)
            {
                if (_lMouseHookID == IntPtr.Zero)
                { _lMouseHookID = RegisterLowLevelMouseHook(_LowLevelMouseProc); }
            }
            else
            {
                if (_lMouseHookID != IntPtr.Zero)
                {
                    UnregisterLowLevelHook(_lMouseHookID);
                    _lMouseHookID = IntPtr.Zero;
                }
            }
        }

        private bool IsAsyncModiferKey(Keys KeyData)
        {
            return ((WinImport.GetAsyncKeyState((int)KeyData) & 0x8000) != 0);
        }

        private IntPtr LowLevelKeyboardProc(int nCode, WinImport.WM wParam, ref WinImport.KBDLLHOOKSTRUCT lParam)
        {
            bool blCallNext = true;

            bool blKeyDown = (wParam == WinImport.WM.KEYDOWN || wParam == WinImport.WM.SYSKEYDOWN);
            bool blKeyUp = (wParam == WinImport.WM.KEYUP || wParam == WinImport.WM.SYSKEYUP);

            if ((nCode >= 0) && (blKeyDown || blKeyUp))
            {
                // the virtual key codes and the winforms Keys have the same enumeration
                // so we can freely cast back and forth between them
                Keys KeyData = (Keys)lParam.vkCode;

                if (KeyData == Keys.W && IsAsyncModiferKey(Keys.Menu) && IsAsyncModiferKey(Keys.ControlKey) && IsAsyncModiferKey(Keys.ShiftKey))
                {
                    HookKeyEventArgs oHKEA = new HookKeyEventArgs(KeyData, ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift);

                    if (blKeyDown)
                    {
                        HookKeyDown(this, oHKEA);
                    }
                    else
                    {
                        HookKeyUp(this, oHKEA);
                    }

                    blCallNext = !oHKEA.Handled;
                }
                else if (KeyData == Keys.F4 && IsAsyncModiferKey(Keys.Menu))
                {
                    blCallNext = true;
                }
                else
                {
                    blCallNext = false;
                }
                //Debug.WriteLine(wParam);
            }

            // if any handler returned false, trap the message
            return (blCallNext) ? WinImport.CallNextHookEx(_lKeyboardHookID, nCode, wParam, ref lParam) : new IntPtr(1);
        }

        private IntPtr LowLevelMouseProc(int nCode, WinImport.WM wParam, ref WinImport.MSLLHOOKSTRUCT lParam)
        {
            bool callNext = true;

            bool isButtonDown = (wParam == WinImport.WM.LBUTTONDOWN || wParam == WinImport.WM.RBUTTONDOWN || wParam == WinImport.WM.MBUTTONDOWN);
            bool isButtonUp = (wParam == WinImport.WM.LBUTTONUP || wParam == WinImport.WM.RBUTTONUP || wParam == WinImport.WM.MBUTTONUP);
            bool isWheelRot = (wParam == WinImport.WM.MOUSEWHEEL || wParam == WinImport.WM.MOUSEHWHEEL);
            bool isMove = (wParam == WinImport.WM.MOUSEMOVE);

            if ((nCode >= 0) && (isButtonDown || isButtonUp || isWheelRot || isMove))
            {
                callNext = false;
                Debug.WriteLine(wParam);
            }

            // if any handler returned false, trap the message
            return (callNext) ? WinImport.CallNextHookEx(_lMouseHookID, nCode, wParam, ref lParam) : new IntPtr(1);
        }

        private static IntPtr GetProc()
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return WinImport.GetModuleHandle(curModule.ModuleName);
            }
        }

        /// <summary>
        /// Registers the user's LowLevelKeyboardProc with the system in order to
        /// intercept any keyboard events before processed in the regular fashion.
        /// This can be used to log all keyboard events or ignore them.
        /// </summary>
        /// <param name="hook">Callback function to call whenever a keyboard event occurs.</param>
        /// <returns>The IntPtr assigned by the Windows's sytem that defines the callback.</returns>
        private IntPtr RegisterLowLevelKeyboardHook(WinImport.LowLevelKeyboardProcDel hook)
        {
            return WinImport.SetWindowsHookEx(WinImport.HookType.WH_KEYBOARD_LL, hook, GetProc(), 0);
        }
        private IntPtr RegisterLowLevelMouseHook(WinImport.LowLevelMouseProcDel hook)
        {
            return WinImport.SetWindowsHookEx(WinImport.HookType.WH_MOUSE_LL, hook, GetProc(), 0);
        }

        /// <summary>
        /// Unregisters a previously registered callback from the low-level chain.
        /// </summary>
        /// <param name="hook">IntPtr previously assigned to the low-level chain.
        /// Users should have stored the value given by 
        /// <see cref="Drs.Interop.Win32.LowLevelKeyboard.RegisterLowLevelKeyboardHook"/>,
        /// and use that value as the parameter into this function.</param>
        /// <returns>True if the hook was removed, false otherwise.</returns>
        private bool UnregisterLowLevelHook(IntPtr hook)
        {
            return WinImport.UnhookWindowsHookEx(hook);
        }
        /// <summary>
        /// Disposes the current instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Wrapper of Dispose(). Disposes the current instance.
        /// </summary>
        public void Close()
        {
            Dispose();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~WinGlobalHook()
        {
            Dispose(false);
        }
        /// <summary>
        /// Overloaded Dipsoser
        /// </summary>
        /// <param name="disposing"></param>
        /// The Boolean parameter disposing indicates whether the method was invoked from the IDisposable.Dispose implementation or from the finalizer. 
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }
            if (disposing)
            {
            }

            GoKeyboardHook(false);
            GoMouseHook(false);

            IsDisposed = true;
        }
    }
}
