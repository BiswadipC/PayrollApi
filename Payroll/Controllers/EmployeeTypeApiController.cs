using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.EmployeeType;

namespace Payroll.Controllers
{
    [Route("employeetypes")]
    [ApiController]

    public class EmployeeTypeApiController : ControllerBase
    {
        private readonly IEmployeeType iet;

        public EmployeeTypeApiController(IEmployeeType iet)
        {
            this.iet = iet;
        } // constructor...


    } // class...
}
