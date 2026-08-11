using Domain.Department;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Payroll.Filters.Departments
{
    public class SaveDepartmentActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var department = context.ActionArguments["response"] as DepartmentResponse;

            if(department != null)
            {
                if (string.IsNullOrWhiteSpace(department.Name))
                {
                    context.ModelState.AddModelError("NotFound", "Department Name cannot be blank.");
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
