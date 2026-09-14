using Application.DTO.SalaryComponent;
using Application.Repository.SalaryComponent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Payroll.Controllers
{
    [Route("salarycomponents")]
    [ApiController]

    public class SalaryComponentController : ControllerBase
    {
        private readonly SalaryComponentService service;

        public SalaryComponentController(SalaryComponentService service)
        {
            this.service = service;
        } // constructor...

        [HttpGet("")]
        [Authorize(Policy = "SALARY COMPONENT-View")]
        public async Task<IActionResult> GetSalaryComponents()
        {
            var components = await service.GetSalaryComponents();
            return Ok(components);
        } // GetSalaryComponents...

        [HttpGet("{componentId:int}")]
        [Authorize(Policy = "SALARY COMPONENT-Edit")]
        public async Task<IActionResult> GetSalaryComponentByComponentId(int componentId)
        {
            var component = await service.GetSalaryComponentByComponentId(componentId);
            return Ok(component);
        } // GetSalaryComponentByComponentId...

        [HttpPost]
        [Authorize(Policy = "SALARY COMPONENT-Edit")]
        public async Task<IActionResult> Save(SalaryComponentDTO dto)
        {
            await service.Save(dto);
            return Ok(new { Message = "Success" });
        }
    } // class...
}
