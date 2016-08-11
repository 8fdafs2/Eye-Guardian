using System;

namespace EyeGuardian
{
    static class WinGeneral
    {
        public static bool GoInputBlock(bool blBlock = true)
        {
            return WinImport.BlockInput(blBlock);
        }

        public static void GoTaskbarHide(bool blHide = true)
        {
            WinImport.ShowWindowCommands swc = 0;

            if (blHide)
            {
                swc = WinImport.ShowWindowCommands.Hide;
            }
            else
            {
                swc = WinImport.ShowWindowCommands.Show;
            }
            WinImport.ShowWindow(WinImport.FindWindow("Shell_TrayWnd", null), swc);
            WinImport.ShowWindow(WinImport.FindWindowEx(IntPtr.Zero, IntPtr.Zero, (IntPtr)0xC017, null), swc);
        }
    }
}
