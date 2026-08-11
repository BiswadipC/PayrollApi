using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{typeId}")]
        public async Task<IActionResult> GetEmployeeTypeByTypeId(int typeId)
        {
            var employeeType = await iet.GetEmployeeTypeByTypeId(typeId);
            return Ok(employeeType);
        } // GetEmployeeTypeByTypeId...
    } // class...
}
