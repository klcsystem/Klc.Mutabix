using System.Net;
using System.Text.Json;
using FluentValidation;
using Klc.Mutabix.Application.Common.Models;

namespace Klc.Mutabix.Api.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ValidationException validationEx =>
                (HttpStatusCode.BadRequest,
                 string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage))),
            UnauthorizedAccessException =>
                (HttpStatusCode.Unauthorized, exception.Message),
            InvalidOperationException =>
                (HttpStatusCode.Conflict, exception.Message),
            KeyNotFoundException =>
                (HttpStatusCode.NotFound, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "Beklenmeyen bir hata olustu")
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            logger.LogError(exception, "Unhandled exception");
        else
            logger.LogWarning(exception, "Handled exception: {Message}", exception.Message);

        context.Response.StatusCode = (int)statusCode;
        var response = ApiResponse.Fail(message);
        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
