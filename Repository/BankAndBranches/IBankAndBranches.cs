using Domain.BankAndBranches;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.BankAndBranches
{
    public interface IBankAndBranches
    {
        Task<List<BankResponse>> GetBanks();
        Task<List<BranchResponse>> GetBranchesByBankId(int bankId);
        Task<BankResponse> GetBankByBankId(int bankId);
        Task Save(BankResponse bank);
    } // interface...
}
