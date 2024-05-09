using Microsoft.AspNetCore.Diagnostics;
using Shared.Exceptions;
using Shared.Results;

namespace DDD_CQRS_Sample.Api.Infrastructure
{
    public class GlobalExceptionHandler() : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            Result response;

            if (exception is ValidationException validationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                response = Result.Failure(validationException.Errors);
            }
            else
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response = Result.Failure(new Error("InternalServerError", "An unexpected error has occurred"));
            }

            await httpContext.Response.WriteAsJsonAsync(response);

            return true;
        }
    }
}
