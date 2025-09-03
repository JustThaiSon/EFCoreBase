using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyProject.Helper.Utils
{
    public static class StringExt
    {
        public static string CusSubString(this string s, int length)
        {
            try
            {
                if (String.IsNullOrEmpty(s)) return string.Empty;
                if (s.Length <= length) return s;
                string result = s.Substring(0, length).ToString();
                return String.Format("{0}...", result);
            }
            catch
            {
                return string.Empty;
            }


        }
        //public static string PhoneFormat(this string s)
        //{
        //    return String.IsNullOrEmpty(s) ? null : s.StartsWith("0") ? "84" + s.Substring(1) : s;
        //}
        //public static string PhoneDisplayFormat(this string s)
        //{
        //    return String.IsNullOrEmpty(s) ? null : s.StartsWith("84") ? "0" + s.Substring(2) : s;
        //}
        public static string Replace(this string s, string oldValue, string newValue, StringComparison comparisonType)
        {
            if (s == null)
                return null;

            if (String.IsNullOrEmpty(oldValue))
                return s;

            StringBuilder result = new StringBuilder(Math.Min(4096, s.Length));
            int pos = 0;

            while (true)
            {
                int i = s.IndexOf(oldValue, pos, comparisonType);
                if (i < 0)
                    break;

                result.Append(s, pos, i - pos);
                result.Append(newValue);

                pos = i + oldValue.Length;
            }
            result.Append(s, pos, s.Length - pos);

            return result.ToString();
        }
        public static string GenerateRandomString(int length)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(characters.Length);
                result.Append(characters[index]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Ensures the string ends with specific character
        /// </summary>
        /// <param name="text"><see cref="string"/>Takes a string parameter</param>
        /// <returns><see cref="string"/>returns a string parameter</returns>
        public static string EnsureEnds(this string text, char Symbols)
        {
            if (!text.EndsWith(Symbols))
            {
                return string.Format("{0}{1}", text, Symbols);
            }

            return text;
        }

        public static bool IsValidEmail(this string email)
        {
            // source: http://thedailywtf.com/Articles/Validating_Email_Addresses.aspx
            Regex rx = new Regex(
            @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
            return rx.IsMatch(email);
        }
        public static bool ValidatePhoneNumber(string phoneNumber)
        {
            var regex = new Regex(@"^(0\d{9}|\+\d{1,3}\d{9})$");

            return regex.IsMatch(phoneNumber);
        }
        public static bool ValidateEmail(string email)
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }


        public static string LongToMoneyK(this long money)
        {
            if (money > 1000)
            {
                return string.Format("{0}{1}", money / 1000, "K");
            }

            return money.ToString();
        }
    }
}
