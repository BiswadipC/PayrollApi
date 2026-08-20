using Domain.Designation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payroll.Filters.Designations;
using Repository.Designation;

namespace Payroll.Controllers
{
    [Route("designations")]
    [ApiController]

    public class DesignationApiController : ControllerBase
    {
        private readonly IDesignation idesignation;
        private readonly ILogger<DesignationApiController> logger;

        public DesignationApiController(IDesignation idesignation, ILogger<DesignationApiController> logger)
        {
            this.idesignation = idesignation;
            this.logger = logger;
        } // DesignationApiController...

        [HttpGet("")]
        public async Task<IActionResult> GetDesignations()
        {            
            var designations = await idesignation.GetDesignations();
            return Ok(designations);
        } // GetDesignations...

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDesignationById(int id)
        {
            var designation = await idesignation.GetDesignationById(id) ?? new DesignationResponse();
            return Ok(designation);
        } // GetDesignationById...

        [HttpPost("")]
        [SaveDesignationActionFilter]
        public async Task<IActionResult> Save(DesignationResponse response)
        {
            string str = await idesignation.Save(response);
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
    } // class...
}
