using Application.Repository.Bank;
using Domain.Bank;
using Domain.Common;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.Bank
{
    namespace NBank
    {
        internal class DALClass : IBank
        {
            private readonly PayrollContext context;

            public DALClass(PayrollContext context)
            {
                this.context = context;
            } // constructor...

            public async Task<List<BankResponse>> GetBanks()
            {
                var banks = await context.Banks.Select(x => new BankResponse(x.BankId, x.BankName, new List<BranchResponse>(), "GET")).ToListAsync();
                return banks;
            } // GetBanks...

            public async Task<List<BranchResponse>> GetBranchesByBankId(int bankId)
            {
                var responses = await context.Branches.Where(m => m.BankId == bankId).
                    Select(x => new BranchResponse(x.BranchId, x.BranchCode, x.BranchName, x.Ifsccode, x.Address ?? string.Empty, x.PhoneNo ?? string.Empty)).ToListAsync();
                return responses;
            } // GetBranchesByBankId...

            public async Task<BankResponse> GetBankByBankId(int bankId)
            {
                var bank = await context.Banks.FirstOrDefaultAsync(x => x.BankId ==  bankId);
                var bankResponse = new BankResponse(bank!.BankId, bank.BankName, (await GetBranchesByBankId(bank.BankId)), "GET");
                return bankResponse;
            } // GetBankByBankId...

            private async Task ErrorHandling(BankResponse bank)
            {
                var errors = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(bank.BankName))
                {
                    errors.Add(GlobalConstantClass.BadRequestKey + "1", new[] { "Bank Name cannot be blank." });
                }
                else if (bank.BankName.Trim().Length < 3)
                {
                    errors.Add(GlobalConstantClass.BadRequestKey + "2", new[] { "Bank Name must be of minimum 3 characters length." });
                }

                if (bank.BankId == 0 && context.Banks.Any(x => x.BankName.ToUpper() == bank.BankName.ToUpper()))
                {
                    errors.Add($"{GlobalConstantClass.BadRequestKey}3", new[] { $"Bank Name \'{bank.BankName}\' already exists." });
                }
                else if (bank.BankId > 0 && (context.Banks.Any(m => m.BankName.ToUpper() == bank.BankName.ToUpper() && m.BankId != bank.BankId)))
                {
                    errors.Add($"{GlobalConstantClass.BadRequestKey}3", new[] { $"Bank Name \'{bank.BankName}\' already exists." });
                }

                if (!bank.Branches.Any())
                {
                    errors.Add($"{GlobalConstantClass.BadRequestKey}4", new[] { "Bank must have atleast one branch." });
                }

                var duplicates = (from cs in bank.Branches
                                  where !string.IsNullOrWhiteSpace(cs.IFSCCode)
                                  group cs by cs.IFSCCode.Trim() into csGroup
                                  where csGroup.Count() > 1
                                  select csGroup.Key
                                  ).ToList();

                if (duplicates.Any())
                {
                    errors.Add($"{GlobalConstantClass.BadRequestKey}5", new[] { "Duplicate IFSC Code Found." });
                }

                foreach (var branch in bank.Branches)
                {
                    if (branch.BranchId == 0 && context.Branches.Any(m => m.Ifsccode == branch.IFSCCode))
                    {
                        errors.Add($"{GlobalConstantClass.BadRequestKey}6", new[] { $"Duplicate IFSC Code found - {branch.IFSCCode}" });
                        break;
                    }
                    else if (branch.BranchId > 0 && (context.Branches.Any(m => m.Ifsccode == branch.IFSCCode && m.BranchId != branch.BranchId)))
                    {
                        errors.Add($"{GlobalConstantClass.BadRequestKey}6", new[] { $"Duplicate IFSC Code found - {branch.IFSCCode}" });
                        break;
                    }
                } // end of foreach loop...

                if (errors.Any())
                {
                    throw new BadRequestClass(errors);
                }
            } // ErrorHandling...

            private async Task CreateBank(BankResponse bank)
            {
                await ErrorHandling(bank);

                Infrastructure.Models.Bank b = new Models.Bank();
                b.BankName = bank.BankName;
                await context.Banks.AddAsync(b);
                await context.SaveChangesAsync();

                foreach (var br in bank.Branches)
                {
                    Branch branch = new Branch();
                    branch.BankId = b.BankId;
                    branch.BranchCode = br.BranchCode;
                    branch.BranchName = br.BranchName;
                    branch.Ifsccode = br.IFSCCode;
                    branch.Address = br.Address;
                    branch.PhoneNo = br.PhoneNo;
                    await context.Branches.AddAsync(branch);
                    await context.SaveChangesAsync();
                }
            } // CreateBank...

            private async Task UpdateBank(BankResponse bank)
            {
                await ErrorHandling(bank);

                var existingBank = context.Banks.FirstOrDefault(m => m.BankId == bank.BankId);
                existingBank!.BankName = bank.BankName;
                context.Banks.Update(existingBank);
                await context.SaveChangesAsync();

                foreach (var br in bank.Branches)
                {
                    if (br.BranchId == 0)
                    {
                        Branch branch = new Branch();
                        branch.BankId = bank.BankId;
                        branch.BranchCode = br.BranchCode;
                        branch.BranchName = br.BranchName;
                        branch.Ifsccode = br.IFSCCode;
                        branch.Address = br.Address;
                        branch.PhoneNo = br.PhoneNo;
                        await context.Branches.AddAsync(branch);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        var existingBranch = await context.Branches.FirstOrDefaultAsync(x => x.BranchId == br.BranchId);
                        existingBranch!.BranchCode = br.BranchCode;
                        existingBranch!.BranchName = br.BranchName;
                        existingBranch.Ifsccode = br.IFSCCode;
                        existingBranch?.Address = br.Address;
                        existingBranch?.PhoneNo = br.PhoneNo;
                        context.Branches.Update(existingBranch!);
                        await context.SaveChangesAsync();
                    } // end if...
                }
            } // UpdateBank...

            public async Task Save(BankResponse bank)
            {
                var trans = await context.Database.BeginTransactionAsync();

                try
                {
                    if(bank.BankId == 0)
                    {
                        await CreateBank(bank);
                    }
                    else if(bank.BankId > 0)
                    {
                        await UpdateBank(bank);
                    }

                    await trans.CommitAsync();
                }
                catch(Exception e)
                {
                    await trans.RollbackAsync();
                    trans.Dispose();
                    throw;
                }
                finally
                {
                    trans.Dispose();
                }
            } // Save...
        } // class...
    } // namespace NBank...    
}
