using Application.Repository.Company;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Payroll.Controllers
{
    [Route("company_and_finyear")]
    [ApiController]

    public class CompanyFinYearApiController : ControllerBase
    {
        private readonly CompanyService service;

        public CompanyFinYearApiController(CompanyService service)
        {
            this.service = service;
        } // constructor...

        [HttpGet("companies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await this.service.GetAllCompanies();
            return Ok(companies);
        } // GetCompanies...

        [HttpGet("years/{companyId:int}")]
        public async Task<IActionResult> GetYears(int companyId)
        {
            var years = await this.service.GetFinYearsByCompanyId(companyId);
            return Ok(years);
        } // GetYears...
    } // class...
}
