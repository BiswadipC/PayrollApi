using Domain.EmployeeType;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.Filters.Employeetypes;
using Repository.EmployeeType;

namespace Payroll.Controllers
{
    [Route("employeetypes")]
    [ApiController]

    public class EmployeeTypeApiController : ControllerBase
    {
        private readonly IEmployeeType iet;

        public EmployeeTypeApiController(IEmployeeType iet)
        {
            this.iet = iet;
        } // constructor...

        [HttpGet("")]
        public async Task<IActionResult> GetEmployeeTypes()
        {
            var eTypes = await iet.GetEmployeeTypes();
            return Ok(eTypes);
        } // GetEmployeeTypes...

        [HttpGet("{typeId:int}")]
        public async Task<IActionResult> GetEmployeeTypeByTypeId(int typeId)
        {
            var employeeType = await iet.GetEmployeeTypeByTypeId(typeId);
            return Ok(employeeType);
        } // GetEmployeeTypeByTypeId...

        [HttpPost("")]
        [EmployeeTypeActionFilter]
        public async Task<IActionResult> Save(EmployeeTypeResponse response)
        {
            string str = await iet.Save(response);
            if(str == "Success")
            {
                return Ok(new { Message = str });
            }

            ModelState.AddModelError("BadRequest", str);
            var problemDetails = new ValidationProblemDetails(ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            return new BadRequestObjectResult(problemDetails);
        } // Save...

        [HttpGet("testtype")]
        public async Task<IActionResult> TestType()
        {
            return Ok(new {
                Message = "Hello From C#",
                Version = "OIDC-TEST-001"
            });
        }
    } // class...
}
