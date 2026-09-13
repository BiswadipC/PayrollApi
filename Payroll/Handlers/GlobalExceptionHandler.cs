using Domain.Common;
using Microsoft.AspNetCore.Diagnostics;
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
            logger.LogError("An unhandled exception occured.");

            switch (exception)
            {
                case NotFoundClass notFound:
                    var notFoundProblemDetails = new ProblemDetails()
                    {
                        Title = exception.Message,
                        Status = 404,
                        Extensions = new Dictionary<string, object?>()
                        {
                            {"errors", notFound.errors }
                        }
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    await httpContext.Response.WriteAsJsonAsync(notFoundProblemDetails, cancellationToken);
                    return true;

                case BadRequestClass badRequestClass:
                    var badRequestProblemDetails = new ProblemDetails()
                    {
                        Title = exception.Message,
                        Status = StatusCodes.Status400BadRequest,
                        Extensions = new Dictionary<string, object?>()
                        {
                            {"errors", badRequestClass.errors }
                        }
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    await httpContext.Response.WriteAsJsonAsync(badRequestProblemDetails, cancellationToken);
                    return true;

                case UnAuthorizedClass unAuthorizedClass:
                    var unAuthorizedProblemDetails = new ProblemDetails()
                    {
                        Title = exception.Message,
                        Status = StatusCodes.Status400BadRequest,
                        Extensions = new Dictionary<string, object?>()
                        {
                            {"errors", unAuthorizedClass.errors }
                        }
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsJsonAsync(unAuthorizedProblemDetails, cancellationToken);
                    return true;

                case ForbiddenClass forbiddenClass:
                    var forbiddenProblemDetails = new ProblemDetails()
                    {
                        Title = exception.Message,
                        Status = StatusCodes.Status400BadRequest,
                        Extensions = new Dictionary<string, object?>()
                        {
                            {"errors", forbiddenClass.errors }
                        }
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await httpContext.Response.WriteAsJsonAsync(forbiddenProblemDetails, cancellationToken);
                    return true;

                default:
                    var problemDetails = new ProblemDetails()
                    {
                        Title = exception.Message,
                        Status = StatusCodes.Status500InternalServerError,
                        Extensions = new Dictionary<string, object?>()
                        {
                            {"errors", new[] {exception.Message} }
                        }
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                    return true;
            } // end of switch...
        } // TryHandleAsync...
    } // class...
}
