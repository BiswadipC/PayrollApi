using Domain.BankAndBranches;
using Domain.Common;
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
                var bank = await (from cs in context.Banks
                 select new BankResponse
                 {
                     BankId = cs.BankId,
                     BankName = cs.BankName
                 }).ToListAsync();

                return bank;
            } // GetBanks...

            public async Task<List<BranchResponse>> GetBranchesByBankId(int bankId)
            {
                var branches = await (from cs in context.Branches
                                    where cs.BankId == bankId
                                select new BranchResponse
                                {
                                    BranchId = cs.BranchId,
                                    BranchCode = cs.BranchCode,
                                    BranchName = cs.BranchName,
                                    IFSCCode = cs.Ifsccode,
                                    Address = cs.Address ?? string.Empty,
                                    PhoneNo = cs.PhoneNo ?? string.Empty
                                }).ToListAsync();

                return branches;
            } // GetBranchesByBankId...

            public async Task<BankResponse> GetBankByBankId(int bankId)
            {
                var errors = new Dictionary<string, string[]>();

                if(!context.Banks.Any(m => m.BankId == bankId))
                {
                    errors.Add(Domain.Common.GlobalConstantsClass.PageNotFoundKey, new[] { GlobalConstantsClass.PageNotFoundError });
                    throw new NotFoundException(errors);
                }

                var bank = await context.Banks.Select(m => new BankResponse
                {
                    BankId = m.BankId,
                    BankName = m.BankName
                }).FirstOrDefaultAsync(x => x.BankId == bankId);

                bank!.Branches = await GetBranchesByBankId(bankId);
                return bank;
            } // GetBankByBankId...

            private async Task ErrorHandling(BankResponse bank)
            {
                var errors = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(bank.BankName))
                {
                    errors.Add(GlobalConstantsClass.BadRequestKey + "1", new[] { "Bank Name cannot be blank." });
                }
                else if (bank.BankName.Trim().Length < 3)
                {
                    errors.Add(GlobalConstantsClass.BadRequestKey + "2", new[] { "Bank Name must be of minimum 3 characters length." });
                }

                if (bank.BankId == 0 && context.Banks.Any(x => x.BankName.ToUpper() == bank.BankName.ToUpper()))
                {
                    errors.Add($"{GlobalConstantsClass.BadRequestKey}3", new[] { $"Bank Name \'{bank.BankName}\' already exists." });
                }
                else if(bank.BankId > 0 && (context.Banks.Any(m => m.BankName.ToUpper() == bank.BankName.ToUpper() && m.BankId != bank.BankId)))
                {
                    errors.Add($"{GlobalConstantsClass.BadRequestKey}3", new[] { $"Bank Name \'{bank.BankName}\' already exists." });
                }

                if (!bank.Branches.Any())
                {
                    errors.Add($"{GlobalConstantsClass.BadRequestKey}4", new[] { "Bank must have atleast one branch." });
                }

                var duplicates = (from cs in bank.Branches
                                  where !string.IsNullOrWhiteSpace(cs.IFSCCode)
                                  group cs by cs.IFSCCode.Trim() into csGroup
                                  where csGroup.Count() > 1
                                  select csGroup.Key
                                  ).ToList();

                if(duplicates.Any())
                {
                    errors.Add($"{GlobalConstantsClass.BadRequestKey}5", new[] { "Duplicate IFSC Code Found." });
                }

                foreach (var branch in bank.Branches)
                {
                    if (branch.BranchId == 0 && context.Branches.Any(m => m.Ifsccode == branch.IFSCCode))
                    {
                        errors.Add($"{GlobalConstantsClass.BadRequestKey}6", new[] { $"Duplicate IFSC Code found - {branch.IFSCCode}" });
                        break;
                    }
                    else if (branch.BranchId > 0 && (context.Branches.Any(m => m.Ifsccode == branch.IFSCCode && m.BranchId != branch.BranchId)))
                    {
                        errors.Add($"{GlobalConstantsClass.BadRequestKey}6", new[] { $"Duplicate IFSC Code found - {branch.IFSCCode}" });
                        break;
                    }
                } // end of foreach loop...

                if (errors.Any())
                {
                    throw new BadRequestException(errors);
                }
            } // ErrorHandling...

            private async Task CreateBank(BankResponse bank)
            {
                await ErrorHandling(bank);
                
                Bank b = new Bank();
                b.BankName = bank.BankName;
                await context.Banks.AddAsync(b);
                await context.SaveChangesAsync();
                
                foreach(var br in bank.Branches)
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
                    if(br.BranchId == 0)
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
                    else
                    {
                        await UpdateBank(bank);
                    }

                    await trans.CommitAsync();
                }
                catch (Exception ex)
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
    } // namespace NBankAndBranches...
}
