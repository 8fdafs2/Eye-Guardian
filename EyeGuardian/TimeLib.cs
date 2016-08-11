using System;

namespace EyeGuardian
{
    public static class TimeLib
    {
        private static DateTime oDT_Base = new DateTime(2000, 1, 1, 0, 0, 0, 0);

        public static string Millisecond2String(int iTime)
        {
            return oDT_Base.AddMilliseconds(iTime).ToString("HH:mm:ss");
        }

        public static DateTime Millisecond2DateTime(int iTime)
        {
            return oDT_Base.AddMilliseconds(iTime);
        }

        public static int DateTime2Millisecond(DateTime oDT)
        {
            return (int)(oDT.TimeOfDay.TotalMilliseconds);
        }

        public static string Second2String(int iSec)
        {
            return oDT_Base.AddSeconds(iSec).ToString("HH:mm:ss");
        }

        public static DateTime Second2DateTime(int iSec)
        {
            return oDT_Base.AddSeconds(iSec);
        }

        public static int DateTime2Second(DateTime oDT)
        {
            return (int)(oDT.TimeOfDay.TotalSeconds);
        }
    }
}
