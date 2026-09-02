using Domain.Designation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Designation;

namespace Payroll.Controllers
{
    [Route("designations")]
    [ApiController]

    public class DesignationApiController : ControllerBase
    {
        private readonly IDesignation idesignation;

        public DesignationApiController(IDesignation idesignation)
        {
            this.idesignation = idesignation;
        } // constructor...

        [HttpGet("")]
        [Authorize(Policy = "DESIGNATION-View")]
        public async Task<IActionResult> GetDesignations()
        {
            string token = HttpContext.Request.Headers["TestHeader"].FirstOrDefault() ?? string.Empty;
            return Ok(await idesignation.GetDesignations());
        } // GetDesignations...

        [HttpGet("{id}")]
        [Authorize(Policy = "DESIGNATION-Edit")]

        public async Task<IActionResult> GetDesignationById(int id)
        {
            return Ok(await idesignation.GetDesignationById(id));
        } // GetDesignationById...

        [HttpPost("")]
        [Authorize(Policy = "DESIGNATION-Edit")]
        public async Task<IActionResult> Save(DesignationResponse response)
        {
            await idesignation.Save(response);
            return Ok(new
            {
                Message = "Success"
            });
        } // Save...
    } // class...
}
