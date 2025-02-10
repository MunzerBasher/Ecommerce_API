using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace EcommerceLogicalLayer.Extensions
{
    public class ExceptionHandle(ILogger<ExceptionHandle> logger) : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandle> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            _logger.LogError(exception,exception.Message);
            var problimDetails = new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://httpstatuses.com/500"
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(problimDetails, cancellationToken);  


            return true;
        }
    }
}
