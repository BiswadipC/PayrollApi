using Domain.Designation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Payroll.Filters.Designations
{
    public class SaveDesignationActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var designation = context.ActionArguments["response"] as DesignationResponse;

            if (designation != null)
            {
                if (string.IsNullOrWhiteSpace(designation.Name))
                {
                    context.ModelState.AddModelError("NotFound", "Designation Name cannot be blank.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                    return;
                }

                if (designation.Name.Length <= 4)
                {
                    context.ModelState.AddModelError("BadRequest", "Designation Name must be minimum 4 characters length.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                    return;
                }
            }
        } // OnActionExecuting...
    } // class...
}
