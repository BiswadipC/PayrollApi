using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.CompanyAndFinYear;

namespace Payroll.Controllers
{
    [Route("company_and_finyear")]
    [ApiController]

    public class CompanyFinYearApiController : ControllerBase
    {
        private readonly ICompanyAndFinYear ic;

        public CompanyFinYearApiController(ICompanyAndFinYear ic)
        {
            this.ic = ic;
        } // constructor...

        [HttpGet("companies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await this.ic.GetCompanies();
            return Ok(companies);
        } // GetCompanies...

        [HttpGet("years")]
        public async Task<IActionResult> GetYears()
        {
            var years = await this.ic.GetFinYears();
            return Ok(years);
        } // GetYears...
    } // class...
}
