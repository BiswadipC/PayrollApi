using Domain.Department;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(await  this.idepartment.GetDepartments());
        } // GetDepartments...

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            return Ok(await idepartment.GetDepartmentById(id));
        } // GetDepartmentById...

        [HttpPost("")]
        public async Task<IActionResult> Save(DepartmentResponse response)
        {
            await this.idepartment.Save(response);
            return Ok(new {Message = "Success"});
        } // Save...
    } // class...
}
