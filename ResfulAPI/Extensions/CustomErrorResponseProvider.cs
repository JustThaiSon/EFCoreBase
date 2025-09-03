using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Net;
using MyProject.Helper.Utils;
using MyProject.Helper.Constants.Globals;

namespace ResfulAPI.Extensions
{
    public class CustomErrorResponseProvider : IErrorResponseProvider
    {
        private readonly string _langCode;

        public CustomErrorResponseProvider(IConfiguration configuration)
        {
            _langCode = configuration["ProjectSettings:LanguageCode"] ?? "vi";
        }

        public IActionResult CreateResponse(ErrorResponseContext context)
        {
            var errorRes = new CommonResponse<object>();
            errorRes.ResponseCode = (int)ResponseCodeEnum.ERR_API_NOT_FOUND;
            errorRes.Message = ResourceUtil.GetMessage(errorRes.ResponseCode, _langCode);

            return new ObjectResult(errorRes)
            {
                StatusCode = (int)HttpStatusCode.NotFound
            };
        }
    }
}
