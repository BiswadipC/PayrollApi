using Application.DTO.Department;
using Application.Repository.Department;
using Domain.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Payroll.Controllers
{
    [Route("departments")]
    [ApiController]

    public class DepartmentApiController : ControllerBase
    {
        private readonly DepartmentService service;

        public DepartmentApiController(DepartmentService service)
        {
            this.service = service;
        } // constructor...

        [HttpGet("")]
        [Authorize(Policy = "DEPARTMENT-View")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await service.GetDepartments();
            return Ok(departments);
        } // GetDepartments...

        [HttpGet("{id:int}")]
        [Authorize(Policy = "DEPARTMENT-Edit")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await service.GetDepartmentById(id);
            return Ok(department);
        } // GetDepartmentById...

        [HttpPost("")]
        [Authorize(Policy = "DEPARTMENT-Edit")]
        public async Task<IActionResult> Save(DepartmentDTO dto)
        {
            await service.Save(dto);
            return Ok(new { Message = "Success" });
        } // Save...
    } // class...
}
