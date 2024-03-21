using Inventory.Application.Books;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Inventory;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "The following exception occured: {Message}", exception.Message);

        var details = exception switch
        {
            BookNotFoundException => BookNotFoundErrorDetails,
            BadHttpRequestException => BadRequestErrorDetails,
			_ => ServerErrorDetails
        };

        httpContext.Response.StatusCode = details.Status ?? 500;
        await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

        return true;
    }

    private static readonly ProblemDetails ServerErrorDetails = new()
    {
        Title = "Server Error",
        Status = StatusCodes.Status500InternalServerError
    };

    private static readonly ProblemDetails BookNotFoundErrorDetails = new()
    {
        Title = "Book not found",
        Status = StatusCodes.Status404NotFound
    };

    private static readonly ProblemDetails BadRequestErrorDetails = new()
    {
        Title = "Bad Request",
        Status = StatusCodes.Status400BadRequest
    };

}
