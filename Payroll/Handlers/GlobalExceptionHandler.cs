using Domain.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Payroll.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            this.logger = logger;
        } // constructor...

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("Error: {message}", exception.Message);

            switch (exception)
            {
                case NotFoundException notFoundException:
                    var notFoundProblemDetails = new ProblemDetails()
                    {
                        Type = "Not Found Exception",
                        Status = StatusCodes.Status404NotFound,
                        Extensions = new Dictionary<string, object?>
                        {
                            {"errors", notFoundException.errors }
                        }
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    await httpContext.Response.WriteAsJsonAsync(notFoundProblemDetails, cancellationToken);
                    return true;

                case ConflictException conflictException:
                    var conflictProblemDetails = new ProblemDetails()
                    {
                        Type = "Conflict Exception",
                        Status = StatusCodes.Status409Conflict,
                        Extensions = new Dictionary<string, object?>
                        {
                            {"errors", conflictException.errors }
                        }
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                    await httpContext.Response.WriteAsJsonAsync(conflictProblemDetails, cancellationToken);
                    return true;

                case BadRequestException badRequestException:
                    var badRequestProblemDetails = new ProblemDetails()
                    {
                        Type = "BadRequest Exception",
                        Status = StatusCodes.Status400BadRequest,
                        Extensions = new Dictionary<string, object?>
                        {
                            {"errors", badRequestException.errors }
                        }
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await httpContext.Response.WriteAsJsonAsync(badRequestProblemDetails, cancellationToken);
                    return true;

                default:
                    var errors = new Dictionary<string, string[]>
                    {
                        {"Unhandled Exception", new[] {exception.Message} }
                    };

                    var problemDetails = new ProblemDetails()
                    {
                        Type = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Extensions = new Dictionary<string, object?>
                        {
                            {"errors", errors }
                        }
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                    return true;
            } // end of switch...
        } // ValueTask...
    } // GlobalExceptionHandler...
}
