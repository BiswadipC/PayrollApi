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

                case BadRequestException badRequestException:
                    var badRequstProblemDetails = new ProblemDetails()
                    {
                        Type = "Bad Request Exception",
                        Status = StatusCodes.Status400BadRequest,
                        Extensions = new Dictionary<string, object?>
                        {
                            {"errors", badRequestException.errors }
                        }
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await httpContext.Response.WriteAsJsonAsync(badRequstProblemDetails, cancellationToken);
                    return true;

                case UnAuthorizedException unAuthorizedException:
                    var unAuthorizedProblemDetails = new ProblemDetails()
                    {
                        Type = "UnAuthorized Exception",
                        Status = StatusCodes.Status401Unauthorized,
                        Extensions = new Dictionary<string, object?>
                        {
                            {"errors", unAuthorizedException.errors }
                        }
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsJsonAsync(unAuthorizedProblemDetails, cancellationToken);
                    return true;

                case AccessDeniedException accessDeniedException:
                    var accessDeniedProblemDetails = new ProblemDetails()
                    {
                        Type = "Access Denied Exception",
                        Status = StatusCodes.Status403Forbidden,
                        Extensions = new Dictionary<string, object?>
                        {
                            {"errors", accessDeniedException.errors }
                        }
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await httpContext.Response.WriteAsJsonAsync(accessDeniedProblemDetails, cancellationToken);
                    return true;
                
                 default:
                    var otherErrors = new Dictionary<string, string[]>();
                    otherErrors.Add(exception.Message, new[] { exception.InnerException!.Message });
                    var problemDetails = new ProblemDetails()
                    {
                        Type = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Extensions = new Dictionary<string, object?>
                        {
                            {"errors", otherErrors }
                        }
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                    return true;
            } // end of switch...
        } // ValueTask...
    } // GlobalExceptionHandler...
}
