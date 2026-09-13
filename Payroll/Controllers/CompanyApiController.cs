using Application.Repository.Company;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Payroll.Controllers
{
    [Route("companies")]
    [ApiController]

    public class CompanyApiController : ControllerBase
    {
        private readonly CompanyService service;

        public CompanyApiController(CompanyService service)
        {
            this.service = service;
        } // constructor...

        [HttpGet("")]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await service.GetAllCompanies();
            return Ok(companies);
        } // GetAllCompanies...

        [HttpGet("GetFinYearsByCompanyId/{companyId:int}")]
        public async Task<IActionResult> GetFinYearsByCompanyId(int companyId)
        {
            var years = await service.GetFinYearsByCompanyId(companyId);
            return Ok(years);
        } // GetFinYearsByCompanyId...

        [HttpGet("{companyId:int}/{finYearId:int}")]
        public async Task<IActionResult> GetCompanyFinYearByCompanyIdFinYearId(int companyId, int finYearId)
        {
            var company = await service.GetCompanyFinYearByCompanyIdFinYearId(companyId, finYearId);
            return Ok(company);
        } // GetCompanyFinYearByCompanyIdFinYearId...
    } // class...
}
