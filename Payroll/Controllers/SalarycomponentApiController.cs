using Domain.SalaryComponent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.SalaryComponent;

namespace Payroll.Controllers
{
    [Route("salarycomponents")]
    [ApiController]

    public class SalarycomponentApiController : ControllerBase
    {
        private readonly ISalaryComponent isc;

        public SalarycomponentApiController(ISalaryComponent isc)
        {
            this.isc = isc;
        } // constructor...

        [HttpGet("")]
        public async Task<IActionResult> GetSalaryComponents()
        {
            return Ok(await isc.GetSalaryComponents());
        } // GetSalaryComponents...

        [HttpGet("{componentId}")]
        public async Task<IActionResult> GetSalaryComponentByComponentId(int componentId)
        {
            return Ok(await isc.GetSalaryComponentByComponentId(componentId));
        } // GetSalaryComponentByComponentId...

        [HttpPost("")]
        public async Task<IActionResult> Save(SalaryComponentResponse response)
        {
            await isc.Save(response);
            return Ok(new
            {
                Message = "Success"
            });
        } // Save...
    } // class...
}
