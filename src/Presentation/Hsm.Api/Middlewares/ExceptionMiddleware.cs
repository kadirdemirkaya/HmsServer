using Hsm.Domain.Models.Error;
using Hsm.Domain.Models.Response;
using System.Net;

namespace Hsm.Api.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate _next, ILogger<ExceptionMiddleware> _logger)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var errorMessage = ex switch
            {
                _ => "Internal server error from error middleware"
            };

            await httpContext.Response.WriteAsync(ApiResponseModel<ErrorDetail>.CreateFailure<ErrorDetail>(new ErrorDetail
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = errorMessage
            }.ToString()).ToString());
        }
    }
}
