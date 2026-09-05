using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Payroll.Controllers
{
    [ApiController]

    public class TestApiController : ControllerBase
    {
        [HttpGet("test")]
        public async Task GetHRA()
        {
            Dictionary<string, decimal> values = new Dictionary<string, decimal>()
            {
                {"Basic", 30000 },
                {"DA", 3000 }
            };

            string formula = "(Basic + DA) / 10";
            decimal Amount = 0;
            bool b = true;

            var matches = Regex.Matches(formula,@"[A-Za-z_][A-Za-z0-9_]*");
            foreach(var match in matches)
            {
                if(!values.ContainsKey(match.ToString()!))
                {
                    b = false;
                    break;
                }
            }


        } // GetHRA...
    } // class...
}
