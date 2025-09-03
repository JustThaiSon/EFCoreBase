using MyProject.Helper.Constants.Globals;
using System.Globalization;
using System.Resources;

namespace MyProject.Helper.Utils
{
    public static class ResourceUtil
    {
        public static string GetMessage(int code, string language)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            string codeString = code.ToString();
            string message = string.Empty;

            ResourceManager rm = new ResourceManager("MyProject.Helper.ResourceFiles.MessageResource", typeof(ResponseCodeEnum).Assembly);
            if (rm == null) return message;

            return rm.GetString(codeString) ?? "";
        }
    }
}
