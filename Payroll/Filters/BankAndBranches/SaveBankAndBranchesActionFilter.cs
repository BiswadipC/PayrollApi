using Domain.BankAndBranches;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Payroll.Filters.BankAndBranches
{
    public class SaveBankAndBranchesActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var bank = context.ActionArguments["response"] as BankResponse;
            
            if(bank != null)
            {
                if (string.IsNullOrWhiteSpace(bank.BankName))
                {
                    context.ModelState.AddModelError("NotFound", "Bank Name cannot be blank.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                    return;
                }

                if(bank.Branches == null || bank.Branches.Count() == 0)
                {
                    context.ModelState.AddModelError("NotFound", "Bank must have atleast one Branch.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                    return;
                }
            }
            
            
        } // OnActionExecuting...
    } // class...
}
