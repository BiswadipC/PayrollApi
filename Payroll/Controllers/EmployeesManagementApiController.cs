using Application.DTO.EmployeesManagement;
using Application.Repository.EmployeesManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Payroll.Controllers
{
    [Route("employees")]
    [ApiController]

    public class EmployeesManagementApiController : ControllerBase
    {
        private readonly EmployeesManagementService service;

        public EmployeesManagementApiController(EmployeesManagementService service)
        {
            this.service = service;
        } // constructor...

        [HttpGet("")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await service.GetEmployees();
            return Ok(employees);
        } // GetAllEmployees...

        [HttpGet("employee-banks/{employeeId:int}")]
        public async Task<IActionResult> GetEmployeeBanksByEmployeeId(int employeeId)
        {
            var employeeBanks = await service.GetemployeesBanksByEmployeeId(employeeId);
            return Ok(employeeBanks);
        } // GetEmployeeBanksByEmployeeId...

        [HttpGet("employee-salary-structure/{employeeId:int}")]
        public async Task<IActionResult> GetEmployeeSalaryStructureByEmployeeId(int employeeId)
        {
            var structure = await service.GetEmployeesSalaryStructureByEmployeeId(employeeId);
            return Ok(structure);
        } // GetEmployeeSalaryStructureByEmployeeId...

        [HttpGet("employee-salary-component/{employeeId:int}")]
        public async Task<IActionResult> GetEmployeeSalaryComponentsByEmployeeId(int employeeId)
        {
            var components = await service.GetEmployeesSalaryComponentsByEmployeeId(employeeId);
            return Ok(components);
        } // GetEmployeeSalaryComponentsByEmployeeId...

        [HttpGet("{employeeId:int}")]
        public async Task<IActionResult> GetEmployeeByEmployeeId(int employeeId)
        {
            var employee = await service.GetEmployeeByEmployeeId(employeeId);
            return Ok(employeeId);
        } // GetEmployeeByEmployeeId...

        [HttpPost("")]
        public async Task<IActionResult> Save(EmployeesMainDTO dto)
        {
            await service.Save(dto);
            return Ok(new { Message = "Success" });
        } // Save...
    } // class...
}
