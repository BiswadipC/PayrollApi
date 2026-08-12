using Domain.EmployeeType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Payroll.Filters.Employeetypes
{
    public class EmployeeTypeActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var employeeType = context.ActionArguments["response"] as EmployeeTypeResponse;

            if(employeeType != null)
            {
                if(string.IsNullOrWhiteSpace(employeeType.TypeName))
                {
                    context.ModelState.AddModelError("NotFound", $"Employee Type Name cannot be blank.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                    return;
                }
            } // end if...
        } // OnActionExecuting...
    } // class...
}
