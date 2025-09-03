using Microsoft.AspNetCore.Http;

namespace MyProject.Helper.Utils
{
    public static class HttpContextHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static long GetUserId()
        {
            if (_httpContextAccessor?.HttpContext == null)
            {
                return 0;
            }

            return (long)(_httpContextAccessor.HttpContext.Items["UserId"] ?? 0);
        }
    }
}
