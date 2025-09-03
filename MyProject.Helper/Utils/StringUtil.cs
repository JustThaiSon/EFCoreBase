using System.Text;
using System.Text.RegularExpressions;

namespace MyProject.Helper.Utils
{
    public static class StringUtil
    {
        public static string MaskUserName(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return "";
            }

            int length = username.Length;
            //neu dai it nhat 11 ky tu -> lay 8 ky tu dau và ***
            if (length > 10)
            {
                username = string.Format("{0}***", username.Substring(0, 8));
            }
            //neu dai it nhat 7 ky tu -> thi *** 3 ky tu cuoi
            else if (length > 6)
            {
                username = string.Format("{0}***", username.Substring(0, length - 3));
            }
            //neu dai it nhat 4 ky tu -> lay 3 ky tu dau va ***
            else if (length > 3)
            {
                username = string.Format("{0}***", username.Substring(0, 3));
            }
            else
            {
                username = string.Format("{0}***", username.Substring(0, 1));
            }

            return username;
        }

        public static string RemoveLastStr(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
                return string.Empty;

            inputStr = inputStr.Substring(0, inputStr.Length - 1);
            return inputStr;
        }

        public static string FilterInjectionChars(string input)
        {
            input = input.Replace('\u0001', ' ');
            input = input.Replace('\u0002', ' ');
            input = input.Replace('\u0003', ' ');
            input = input.Replace('\u0004', ' ');
            input = input.Replace('\t', ' ');
            return input.Trim();
        }

        public static string MinifyBody(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
                return string.Empty;

            return Regex.Replace(body, @"\s+", string.Empty);
        }
    }

    
    
}
