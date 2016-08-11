using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace EyeGuardian
{
    static class WinCtrlAltDel
    {
        public static void DisableTaskMgr(bool isDisable = true)
        {
            RegistryKey regkey = default(RegistryKey);
            string keyValueInt = null;
            //0x00000000 (0)
            string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";

            try
            {
                if (isDisable)
                {
                    keyValueInt = "1";
                }
                else
                {
                    keyValueInt = "";
                }
                regkey = Registry.CurrentUser.CreateSubKey(subKey);
                regkey.SetValue("DisableTaskMgr", keyValueInt);
                regkey.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
