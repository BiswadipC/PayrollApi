using Domain.Bank;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Bank
{
    public interface IBank
    {
        Task<List<BankResponse>> GetBanks();
        Task<List<BranchResponse>> GetBranchesByBankId(int bankId);
        Task<BankResponse> GetBankByBankId(int bankId);
        Task Save(BankResponse bank);
    } // interface...
}
