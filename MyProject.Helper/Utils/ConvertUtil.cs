using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Helper.Utils
{
    public static class ConvertUtil
    {
        public static string PhoneFormat(this string s)
        {
            return String.IsNullOrEmpty(s) ? null : s.StartsWith("0") ? "84" + s.Substring(1) : s;
        }

        public static string PhoneDisplayFormat(this string s)
        {
            return String.IsNullOrEmpty(s) ? null : s.StartsWith("84") ? "0" + s.Substring(2) : s;
        }

        public static int ToInt(object obj)
        {
            if (obj == null) return -1;
            int value = 0;
            int.TryParse(obj.ToString(), out value);
            return value;
        }

        public static long ToLong(object obj)
        {
            if (obj == null) return -1;
            long value = 0;
            long.TryParse(obj.ToString(), out value);
            return value;
        }

        public static string ToString(object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

        public static bool ToBool(object obj)
        {
            if (obj == null) return false;
            bool value = false;
            bool.TryParse(obj.ToString(), out value);
            return value;
        }

        public static decimal ToDecimal(object obj)
        {
            if (obj == null) return 0;
            decimal value = 0;
            decimal.TryParse(obj.ToString(), out value);
            return value;
        }

        public static float ToFloat(object obj)
        {
            if (obj == null) return -1.0f;
            float value = 0.0f;
            float.TryParse(obj.ToString(), out value);
            return value;
        }

        public static DateTime ToDateTime(object obj)
        {
            if (obj == null) return new DateTime();
            DateTime value = default(DateTime);
            DateTime.TryParse(obj.ToString(), out value);
            return value;
        }

        public static string DateTimeHms(DateTime d)
        {
            return d.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string DateTimeHm(DateTime d)
        {
            return d.ToString("dd/MM/yyyy HH:mm");
        }

        public static string DateTimeH(DateTime d)
        {
            return d.ToString("dd/MM/yyyy HH");
        }
    }

}
