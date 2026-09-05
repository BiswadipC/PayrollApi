using Domain.EmployeesManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.EmployeesManagement;

namespace Payroll.Controllers
{
    [Route("employees")]
    [ApiController]

    public class EmployeeManagementApiController : ControllerBase
    {
        private readonly IEmployeeManagement iem;

        public EmployeeManagementApiController(IEmployeeManagement iem)
        {
            this.iem = iem;
        } // constructor...

        [HttpPost("GetPercFormulaResponse")]
        public async Task<IActionResult> GetPercFormulaResponse([FromBody] FormulaRequestDTO formulaDTO)
        {
            var newComponent = await iem.GetFormulaResponse(formulaDTO);
            return Ok(newComponent);
        } // GetPercFormulaResponse...
    } // class...
}
