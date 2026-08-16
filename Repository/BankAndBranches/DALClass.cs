using Domain.BankAndBranches;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.BankAndBranches
{
    namespace NBankAndBranches
    {
        internal sealed class DALClass : IBankAndBranches
        {
            private readonly PayrollContext context;

            public DALClass(PayrollContext context)
            {
                this.context = context;
            } // constructor...

            public async Task<List<BankResponse>> GetBanks()
            {
                var banks = await context.Banks.Select(x => new BankResponse()
                {
                    BankId = x.BankId,
                    BankName = x.BankName
                }).ToListAsync();

                return banks;
            } // GetBanks...

            public async Task<List<BranchResponse>> GetBranchesByBankId(int bankId)
            {
                var branches = await context.Branches.Where(m => m.BankId == bankId).Select(x => new BranchResponse()
                {
                    BranchId = x.BranchId,
                    BranchCode = x.BranchCode,
                    BranchName = x.BranchName,
                    IFSCCode = x.Ifsccode,
                    Address = x.Address ?? string.Empty,
                    PhoneNo = x.PhoneNo ?? string.Empty
                }).ToListAsync();

                return branches;
            } // GetBranchesByBankId...

            public async Task<BankResponse> GetBankByBankId(int bankId)
            {
                var bank = await context.Banks.Select(m => new BankResponse()
                {
                    BankId = m.BankId,
                    BankName = m.BankName
                }).FirstOrDefaultAsync(m => m.BankId == bankId);
                bank!.Branches = await GetBranchesByBankId(bankId);

                return bank;
            } // GetBankByBankId...

            public async Task<string> Save(BankResponse bank)
            {
                string message = string.Empty;
                var trans = await context.Database.BeginTransactionAsync();

                try
                {
                    if(bank.BankId == 0)
                    {
                        if(context.Banks.Any(m => m.BankName.ToUpper() == bank.BankName.ToUpper()))
                        {
                            message = $"Bank Name already exists - {bank.BankName}";
                            return message;
                        }

                        Bank b = new Bank();
                        b.BankName = bank.BankName;
                        await context.AddAsync(b);
                        await context.SaveChangesAsync();

                        foreach (var branch in bank.Branches)
                        {
                            Branch br = new Branch();
                            br.BranchName = branch.BranchName;
                            br.BranchCode = branch.BranchCode;
                            br.Ifsccode = branch.IFSCCode;
                            br.Address = branch.Address;
                            br.PhoneNo = branch.PhoneNo;
                            br.BankId = b.BankId;
                            await context.AddAsync(br);
                            await context.SaveChangesAsync();
                        } // foreach loop...
                    } // new...
                    else
                    {
                        if(context.Banks.Any(m => m.BankName.ToUpper() == bank.BankName.ToUpper() && m.BankId != bank.BankId))
                        {
                            message = $"Bank Name already exists - {bank.BankName}";
                            return message;
                        }

                        var existingBank = await context.Banks.FirstOrDefaultAsync(m => m.BankId == bank.BankId);
                        existingBank!.BankName = bank.BankName;
                        context.Update(existingBank);
                        await context.SaveChangesAsync();

                        foreach (var branch in bank.Branches)
                        {
                            if(branch.BranchId == 0)
                            {
                                Branch br = new Branch();
                                br.BranchName = branch.BranchName;
                                br.BranchCode = branch.BranchCode;
                                br.Ifsccode = branch.IFSCCode;
                                br.Address = branch.Address;
                                br.PhoneNo = branch.PhoneNo;
                                br.BankId = bank.BankId;
                                await context.AddAsync(br);
                                await context.SaveChangesAsync();
                            }
                            else
                            {
                                var existingBranch = await context.Branches.FirstOrDefaultAsync(x => x.BranchId == branch.BranchId);
                                existingBranch!.BranchName = branch.BranchName;
                                existingBranch!.BranchCode = branch.BranchCode;
                                existingBranch!.Ifsccode = branch.IFSCCode;
                                existingBranch!.Address = branch.Address;
                                existingBranch!.PhoneNo = branch.PhoneNo;
                                context.Update(existingBranch);
                                await context.SaveChangesAsync();
                            } // end if...
                        } // foreach loop...
                    } // edit...

                    await trans.CommitAsync();
                    message = "Success";
                }
                catch(Exception ex)
                {
                    await trans.RollbackAsync();
                    message = ex.ToString();
                }
                finally
                {
                    trans.Dispose();
                }

                return message;
            } // Save...
        } // class...
    } // namespace NBankAndBranches...
}
