#pragma warning disable SA1200 // Using directives should be placed correctly
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using TodoListApp.Core.Errors;
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApp.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, message) = exception switch
        {
            AuthException => (StatusCodes.Status401Unauthorized, exception.Message),
            UnauthorizedException => (StatusCodes.Status401Unauthorized, exception.Message),
            ForbiddenException => (StatusCodes.Status403Forbidden, exception.Message),
            NotFoundException or EntityNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            ValidationFailedException or InvalidArgumentException or ParameterInvalidException => (StatusCodes.Status400BadRequest, exception.Message),
            NotAllowedException => (StatusCodes.Status405MethodNotAllowed, exception.Message),
            DatabaseException or PersistenceException => (StatusCodes.Status500InternalServerError, "Database error occurred."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        var result = JsonSerializer.Serialize(new
        {
            error = message,
            //statusCode,
            //exceptionType = exception.GetType().Name,
            //detail = exception.Message,
            //stackTrace = exception.StackTrace,
        });

        await httpContext.Response.WriteAsync(result, cancellationToken);
        return true;
    }
}
