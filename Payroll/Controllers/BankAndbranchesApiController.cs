using Domain.BankAndBranches;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.BankAndBranches;

namespace Payroll.Controllers
{
    [Route("bankandbranches")]
    [ApiController]

    public class BankAndbranchesApiController : ControllerBase
    {
        private readonly IBankAndBranches ibb;

        public BankAndbranchesApiController(IBankAndBranches ibb)
        {
            this.ibb = ibb;
        } // BankAndbranchesApiController...

        [HttpGet("")]
        public async Task<IActionResult> GetBanks()
        {
            var banks = await ibb.GetBanks();
            return Ok(banks);
        } // GetBanks...

        [HttpGet("branches/{bankId}")]
        public async Task<IActionResult> GetBranchesByBankId(int bankId)
        {
            var branches = await ibb.GetBranchesByBankId(bankId);
            return Ok(branches);
        } // GetBranchesByBankId...

        [HttpGet("{bankId}")]
        public async Task<IActionResult> GetBankByBankId(int bankId)
        {
            var bank = await ibb.GetBankByBankId(bankId);
            return Ok(bank);
        } // GetBankByBankId...

        [HttpPost("")]
        public async Task<IActionResult> Save(BankResponse response)
        {
            string str = await ibb.Save(response);
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
