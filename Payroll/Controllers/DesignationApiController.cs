using Application.DTO.Designation;
using Application.Repository.Designation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Payroll.Controllers
{
    [Route("designations")]
    [ApiController]

    public class DesignationApiController : ControllerBase
    {
        private readonly DesignationService service;

        public DesignationApiController(DesignationService service)
        {
            this.service = service;
        } // constructor...

        [HttpGet("")]
        //[Authorize(Policy = "DESIGNATION-View")]
        public async Task<IActionResult> GetDesignations()
        {
            var designationsDTO = await service.GetDesignations();
            return Ok(designationsDTO);
        } // GetDesignations...

        [HttpGet("{id:int}")]
        [Authorize(Policy = "DESIGNATION-Edit")]
        public async Task<IActionResult> GetDesignationById(int id)
        {
            var designation = await service.GetDesignationById(id);
            return Ok(designation);
        } // GetDesignationById...

        [HttpPost("")]
        [Authorize(Policy = "DESIGNATION-Edit")]
        public async Task<IActionResult> Save(DesignationDTO dto)
        {
            await service.Save(dto);
            return Ok(new {Message = "Success"});
        } // Save...
    } // class...
}
