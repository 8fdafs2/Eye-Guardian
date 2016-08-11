using System;
using System.Configuration;

namespace EyeGuardian
{
    public static class SettingsPropertyCollectionExtensions
    {
        public static T GetDefault<T>(this SettingsPropertyCollection oSettingsPropertyCollection, string sProperty)
        {
            string sVal = (string)Properties.Settings.Default.Properties[sProperty].DefaultValue;

            return (T)Convert.ChangeType(sVal, typeof(T));
        }
    }
}
