using Domain.Department;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.Filters.Departments;
using Repository.Department;

namespace Payroll.Controllers
{
    [Route("departments")]
    [ApiController]

    public class DepartmentApiController : ControllerBase
    {
        private readonly IDepartment idepartment;

        public DepartmentApiController(IDepartment idepartment)
        {
            this.idepartment = idepartment;
        } // constructor...

        [HttpGet("")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await idepartment.GetDepartments();
            return Ok(departments);
        } // GetDepartments...

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await idepartment.GetDepartmentById(id);
            return Ok(department);
        } // GetDepartmentById...

        [HttpPost("")]
        [SaveDepartmentActionFilter]
        public async Task<IActionResult> Save(DepartmentResponse response)
        {
            string str = await idepartment.Save(response);

            if (str == "Success")
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
    } // class...
}
