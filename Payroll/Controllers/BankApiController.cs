using Application.DTO.Bank;
using Application.Repository.Bank;
using Domain.Bank;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Payroll.Controllers
{
    [Route("banks")]
    [ApiController]

    public class BankApiController : ControllerBase
    {
        private readonly BankService service;

        public BankApiController(BankService service)
        {
            this.service = service;
        } // constructor...

        [HttpGet("")]
        [Authorize(Policy = "BANK-View")]
        public async Task<IActionResult> GetBanks()
        {
            var banks = await service.GetBanks();
            return Ok(banks);
        } // GetBanks...

        [HttpGet("branches/{bankId:int}")]        
        public async Task<IActionResult> GetBranchesByBankId(int bankId)
        {
            var branches = await service.GetBranchesByBankId(bankId);
            return Ok(branches);
        } // GetBranchesByBankId...

        [HttpGet("{bankId:int}")]
        [Authorize(Policy = "BANK-Edit")]
        public async Task<IActionResult> GetBankByBankId(int bankId)
        {
            var bank = await service.GetBankByBankId(bankId);
            return Ok(bank);
        } // GetBankByBankId...

        [HttpPost("")]
        [Authorize(Policy = "BANK-Edit")]
        public async Task<IActionResult> Save(BankDTO bank)
        {
            await service.Save(bank);
            return Ok(new
            {
                Message = "Success"
            });
        } // Save...
    } // class...
}
