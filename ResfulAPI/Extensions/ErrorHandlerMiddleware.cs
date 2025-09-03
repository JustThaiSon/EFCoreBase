using FluentValidation;
using MyProject.Helper.Constants.Globals;
using MyProject.Helper.Utils;
using System.Net;
using System.Text.Json;

namespace ResfulAPI.Extensions
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly string _langCode;

        public ErrorHandlerMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _langCode = configuration["ProjectSettings:LanguageCode"] ?? "vi";

            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (!context.Request.Body.CanSeek)
                {
                    context.Request.EnableBuffering();
                }

                await _next(context);
            }
            catch (ValidationException vex)
            {
                var response = context.Response;

                if (response.HasStarted)
                {
                    return;
                }

                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;

                var res = new CommonResponse<object>();
                res.ResponseCode = (int)ResponseCodeEnum.ERR_WRONG_INPUT;
                res.Message = string.Join(",", vex.Errors.Select(failure => failure.ErrorMessage));

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var result = JsonSerializer.Serialize(res, options);
                await response.WriteAsync(result);

            }
            catch (Exception error)
            {
                var response = context.Response;

                if (response.HasStarted)
                {
                    return;
                }

                response.ContentType = "application/json";

                switch (error)
                {
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                // Write log exception
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var logger = scope.ServiceProvider.GetService<ILogger<ErrorHandlerMiddleware>>();

                    logger.LogError(error, "Unhandled exception occurred");
                }

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var res = new CommonResponse<object>();
                    res.ResponseCode = (int)ResponseCodeEnum.ERR_SYMTEM;
                    res.Message = ResourceUtil.GetMessage(res.ResponseCode, _langCode);

                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    var result = JsonSerializer.Serialize(res, options);
                    await response.WriteAsync(result);
                }
            }
        }
    }
}
