using Domain.BankAndBranches;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.BankAndBranches;

namespace Payroll.Controllers
{
    [Route("bankandbranches")]
    [ApiController]

    public class BankAndBranchesApiController : ControllerBase
    {
        private readonly IBankAndBranches ibb;

        public BankAndBranchesApiController(IBankAndBranches ibb)
        {
            this.ibb = ibb;
        } // constructor...

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
        public async Task<IActionResult> Save(BankResponse bank)
        {
            await ibb.Save(bank);
            return Ok(new
            {
                Message = "Success"
            });
        } // Save...
    } // class...
}
