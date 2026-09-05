using Dapper;
using Domain.Common;
using Domain.EmployeesManagement;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repository.EmployeesManagement
{
    namespace NEmployeesManagement
    {
        internal sealed class DALClass : IEmployeeManagement
        {
            private readonly PayrollContext context;
            private readonly IDbConnection db;
            private readonly IHttpContextAccessor httpContextAccessor;

            public DALClass(PayrollContext context, IDbConnection db, IHttpContextAccessor httpContextAccessor)
            {
                this.context = context;
                this.db = db;
                this.httpContextAccessor = httpContextAccessor;
            } // constructor...

            public async Task<List<EmployeesBankResponse>> GetEmployeeBanksByEmployeeId(int empId)
            {
                string str = @"select e.AccountId AccountId, e.BankId BankId, b.BankName BankName, e.BranchId BranchId, br.BranchName BranchName,
	                                   br.IFSCCode IFSCCode, e.AccountHolderName AccountHolderName, e.AccountNo AccountNo
                                 from EmployeeBankAccounts e inner join Banks b on  e.BankId = b.BankId
                                 inner join Branches br on e.BranchId = br.BranchId
                                where e.EmployeeId = @empId";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("empId", empId);

                var responses = await db.QueryAsync<EmployeesBankResponse>(str, dp);
                return responses.ToList();
            } // GetEmployeeBanksByEmployeeId...

            public async Task<EmployeesSalaryStructuresResponse> GetEmployeeSalaryStructureByEmployeeId(int empId)
            {
                var response = await context.EmployeeSalaryStructures.Select(x => new EmployeesSalaryStructuresResponse()
                {
                    StructureId = x.StructureId,
                    EffectiveFrom = x.EffectiveFrom == null ? string.Empty : x.EffectiveFrom.Value.ToString("dd/MM/yyyy"),
                    EffectiveTo = x.EffectiveTo == null ? string.Empty : x.EffectiveTo.Value.ToString("dd/MM/yyyy"),
                    PayFrequency = x.PayFrequency,
                    AnnualCTC = x.AnnualCtc,
                    Basic = x.Basic,
                    IsActive = x.IsActive
                }).FirstOrDefaultAsync();

                return response!;
            } // GetEmployeeSalaryStructureByEmployeeId...

            public async Task<EmployeesMainResponse> GetEmployeeResponseByEmployeeId(int empId)
            {
                throw new Exception();
            } // GetEmployeeResponseByEmployeeId...

            public async Task<List<EmployeeSalaryComponentsResponse>> GetEmployeeSalaryComponentsByEmployeeId(int empId)
            {
                var results = await (from es in context.EmployeeSalaryComponents
                               join cs in context.SalaryComponents
                               on es.ComponentId  equals cs.ComponentId
                               where es.EmployeeId == empId
                               select new EmployeeSalaryComponentsResponse
                               {
                                   EmployeeSalaryComponentId = es.SalaryComponentId,
                                   ComponentId = es.ComponentId,
                                   ComponentCode = cs.ComponentCode,
                                   ComponentName = cs.ComponentName,
                                   Formula = es.Formula,
                                   Amount = es.Amount
                               }).ToListAsync();

                return results;
            } // GetEmployeeSalaryComponentsByEmployeeId...

            public async Task<EmployeeSalaryComponentsResponse> GetFormulaResponse(FormulaRequestDTO formulaDTO)
            {
                var existingEmployeeSalaryComponent = formulaDTO.employeeSalaryComponents;
                var employeeSalaryComponents = formulaDTO.ListEmployeeSalaryComponentsResponse;

                IDictionary<string, decimal> Components = new Dictionary<string, decimal>();
                decimal Amount = 0;
                EmployeeSalaryComponentsResponse newRsponse = new EmployeeSalaryComponentsResponse();

                if (!string.IsNullOrWhiteSpace(existingEmployeeSalaryComponent.Formula))
                {
                    string formula = existingEmployeeSalaryComponent.Formula;
                    foreach (var data in employeeSalaryComponents)
                    {
                        Components.Add(data.ComponentCode, data.Amount);
                    } // end of foreach loop...

                    var matches = Regex.Matches(formula, @"[A-Za-z_][A-Za-z0-9_]*");
                    bool b = true;
                    string unMatchedCode = string.Empty;
                    foreach(var match in matches)
                    {
                        if(!Components.ContainsKey(match.ToString()!))
                        {
                            b = false;
                            unMatchedCode = match.ToString()!;
                            break;
                        }
                    } // end of foreach loop...

                    if(!b)
                    {
                        throw new BadRequestException(new Dictionary<string, string[]>()
                        {
                            {GlobalConstantsClass.BadRequestKey, new[] {$"\'{unMatchedCode}\' is an invalid component."} }
                        });
                    }

                    foreach (var component in Components)
                    {
                        formula = formula.Replace(component.Key, component.Value.ToString());
                    } // end of foreach loop...

                    DataTable dt = new DataTable();
                    Amount = Convert.ToDecimal(dt.Compute(formula, string.Empty), CultureInfo.InvariantCulture);

                    newRsponse.EmployeeSalaryComponentId = existingEmployeeSalaryComponent.EmployeeSalaryComponentId;
                    newRsponse.ComponentId = existingEmployeeSalaryComponent.ComponentId;
                    newRsponse.ComponentCode = existingEmployeeSalaryComponent.ComponentCode;
                    newRsponse.ComponentName = existingEmployeeSalaryComponent.ComponentName;
                    newRsponse.Formula = existingEmployeeSalaryComponent.Formula;
                    newRsponse.Amount = Amount;

                    return newRsponse;
                } // if Formula...

                throw new BadRequestException(new Dictionary<string, string[]>
                {
                    {GlobalConstantsClass.BadRequestKey, new [] {"No \'Formula\' specified. Please enter a valid Formula."} }
                });
            } // GetFormulaResponse...

            private async Task ErrorHandling(EmployeesMainResponse response)
            {
                List<string> badRequestErrors = new List<string>();
                Dictionary<string, string[]> badRequestErrorsDictionary = new Dictionary<string, string[]>(); 
                
                if(string.IsNullOrWhiteSpace(response.EmployeeCode))
                {
                    badRequestErrors.Add("Employee Code cannot be blank.");
                }
                if (string.IsNullOrWhiteSpace(response.EmployeeName))
                {
                    badRequestErrors.Add("Employee Name cannot be blank.");
                }
                if (string.IsNullOrWhiteSpace(response.DOB))
                {
                    badRequestErrors.Add("Employee \'Date of Birth\' cannot be blank.");
                }
                if (!DateTime.TryParse(response.DOB, out DateTime result))
                {
                    badRequestErrors.Add("Employee \'Date of Birth\' has an invalid format.");
                }
                if (string.IsNullOrWhiteSpace(response.Gender))
                {
                    badRequestErrors.Add("Employee Gender cannot be blank.");
                }
                if (string.IsNullOrWhiteSpace(response.Phone))
                {
                    badRequestErrors.Add("Employee Phone No. cannot be blank.");
                }

                if (string.IsNullOrWhiteSpace(response.HireDate))
                {
                    badRequestErrors.Add("Employee \'Joining Date\' cannot be blank.");
                }
                if (!DateTime.TryParse(response.HireDate, out DateTime result1))
                {
                    badRequestErrors.Add("Employee \'Joining Date\' has an invalid format.");
                }

                if(badRequestErrors.Any())
                {
                    badRequestErrorsDictionary.Add(GlobalConstantsClass.BadRequestKey, badRequestErrors.ToArray());
                }
            } // ErrorHandling...

            private async Task CreateEmployee(EmployeesMainResponse response)
            {
                Employee employee = new Employee();
                employee.CompanyId = response.CompanyId;
                employee.EmployeeCode = response.EmployeeCode;
                employee.EmployeeName = response.EmployeeName;
                employee.DateOfBirty = DateTime.ParseExact(response.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                employee.Gender = response.Gender;
                employee.Email = response.Email ?? string.Empty;
                employee.Phone = response.Phone;
                employee.HireDate = DateTime.ParseExact(response.HireDate,"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                employee.TerminationDate = string.IsNullOrWhiteSpace(response.TerminationDate) ? null : DateTime.ParseExact(response.TerminationDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                employee.EmployeeTypeId = response.EmployeeTypeId;
                employee.IsActive = "Yes";
                employee.DepartmentId = response.DepartmentId;
                employee.DesignationId = response.DesignationId;
                employee.ManagerId = response.ManagerId;
                employee.AddressLine1 = response.AddressLine1;
                employee.AddressLine2 = response.AddressLine2 ?? string.Empty;
                employee.City = response.City ?? string.Empty;
                employee.State = response.State ?? string.Empty;
                employee.Country = response.Country;
                employee.PostalCode = response.Pin;
                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();

                foreach(var data in response.ListEmployeesBankResponse)
                {
                    EmployeeBankAccount bank = new EmployeeBankAccount();
                    bank.EmployeeId = employee.EmployeeId;
                    bank.CompanyId = employee.CompanyId;
                    bank.BankId = data.BankId;
                    bank.BranchId = data.BranchId;
                    bank.AccountHolderName = data.AccountHolderName;
                    bank.AccountNo = data.AccountNo;
                    await context.EmployeeBankAccounts.AddAsync(bank);
                } // end of foreach loop...
                await context.SaveChangesAsync();

                EmployeeSalaryStructure structure = new EmployeeSalaryStructure();
                structure.EmployeeId = employee.EmployeeId;
                structure.CompanyId = employee.CompanyId;
                structure.EffectiveFrom = DateTime.ParseExact(response.EmployeesSalaryStructures.EffectiveFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                structure.EffectiveTo = string.IsNullOrWhiteSpace(response.EmployeesSalaryStructures.EffectiveTo) ? null : DateTime.ParseExact(response.EmployeesSalaryStructures.EffectiveTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                structure.PayFrequency = response.EmployeesSalaryStructures.PayFrequency;
                structure.AnnualCtc = response.EmployeesSalaryStructures.AnnualCTC;
                structure.Basic = response.EmployeesSalaryStructures.Basic ?? 0.00m;
                structure.IsActive = "Yes";
                await context.EmployeeSalaryStructures.AddAsync(structure);
                await context.SaveChangesAsync();

                foreach(var data in response.ListEmployeeSalaryComponentsResponse)
                {
                    EmployeeSalaryComponent esc = new EmployeeSalaryComponent();
                    esc.EmployeeId = employee.EmployeeId;
                    esc.CompanyId = employee.CompanyId;
                    esc.ComponentId = data.ComponentId;
                    esc.Formula = data.Formula;
                    esc.Amount = data.Amount;
                    await context.EmployeeSalaryComponents.AddAsync(esc);
                } // end of foreach loop...
                await context.SaveChangesAsync();
            } // CreateEmployee...

            private async Task UpdateEmployee(EmployeesMainResponse response)
            {
                var existingEmployee = await context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == response.EmployeeId);
                if (existingEmployee != null)
                {
                    existingEmployee.EmployeeCode = response.EmployeeCode;
                    existingEmployee.EmployeeName = response.EmployeeName;
                    existingEmployee.DateOfBirty = DateTime.ParseExact(response.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    existingEmployee.Gender = response.Gender;
                    existingEmployee.Email = response.Email ?? string.Empty;
                    existingEmployee.Phone = response.Phone;
                    existingEmployee.HireDate = DateTime.ParseExact(response.HireDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    existingEmployee.TerminationDate = string.IsNullOrWhiteSpace(response.TerminationDate) ? null : DateTime.ParseExact(response.TerminationDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    existingEmployee.EmployeeTypeId = response.EmployeeTypeId;
                    existingEmployee.IsActive = "Yes";
                    existingEmployee.DepartmentId = response.DepartmentId;
                    existingEmployee.DesignationId = response.DesignationId;
                    existingEmployee.ManagerId = response.ManagerId;
                    existingEmployee.AddressLine1 = response.AddressLine1;
                    existingEmployee.AddressLine2 = response.AddressLine2 ?? string.Empty;
                    existingEmployee.City = response.City ?? string.Empty;
                    existingEmployee.State = response.State ?? string.Empty;
                    existingEmployee.Country = response.Country;
                    existingEmployee.PostalCode = response.Pin;
                    context.Employees.Update(existingEmployee);
                    await context.SaveChangesAsync();
                } // end if...

                if (existingEmployee != null)
                {
                    foreach (var data in response.ListEmployeesBankResponse)
                    {
                        if(data.AccountId == 0)
                        {
                            EmployeeBankAccount bank = new EmployeeBankAccount();
                            bank.EmployeeId = existingEmployee.EmployeeId;
                            bank.CompanyId = existingEmployee.CompanyId;
                            bank.BankId = data.BankId;
                            bank.BranchId = data.BranchId;
                            bank.AccountHolderName = data.AccountHolderName;
                            bank.AccountNo = data.AccountNo;
                            await context.EmployeeBankAccounts.AddAsync(bank);
                        }
                        else
                        {
                            var existingEmployeeBankAccount = await context.EmployeeBankAccounts.FirstOrDefaultAsync(x => x.AccountId == data.AccountId);
                            existingEmployeeBankAccount!.BankId = data.BankId;
                            existingEmployeeBankAccount.BranchId = data.BranchId;
                            existingEmployeeBankAccount.AccountHolderName = data.AccountHolderName;
                            existingEmployeeBankAccount.AccountNo = data.AccountNo;
                            context.Update(existingEmployeeBankAccount);
                        } // end if...
                    } // end of foreach loop...
                    await context.SaveChangesAsync();
                } // end if...

                var existingEmployeeSalaryStructure = await context.EmployeeSalaryStructures.FirstOrDefaultAsync(m => m.StructureId == response.EmployeesSalaryStructures.StructureId);
                if(existingEmployeeSalaryStructure != null)
                {
                    existingEmployeeSalaryStructure.EffectiveFrom = DateTime.ParseExact(response.EmployeesSalaryStructures.EffectiveFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    existingEmployeeSalaryStructure.EffectiveTo = string.IsNullOrWhiteSpace(response.EmployeesSalaryStructures.EffectiveTo) ? null : DateTime.ParseExact(response.EmployeesSalaryStructures.EffectiveTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    existingEmployeeSalaryStructure.PayFrequency = response.EmployeesSalaryStructures.PayFrequency;
                    existingEmployeeSalaryStructure.AnnualCtc = response.EmployeesSalaryStructures.AnnualCTC;
                    existingEmployeeSalaryStructure.Basic = response.EmployeesSalaryStructures.Basic ?? 0.00m;
                    context.Update(existingEmployeeSalaryStructure);
                    await context.SaveChangesAsync();
                } // end if...

                if(existingEmployee != null)
                {
                    var existingEmployeeSalaryComponents = await context.EmployeeSalaryComponents.Where(m => m.EmployeeId == existingEmployee.EmployeeId).ToListAsync();
                    if(existingEmployeeSalaryComponents != null && existingEmployeeSalaryComponents.Count() > 0)
                    {
                        context.EmployeeSalaryComponents.RemoveRange(existingEmployeeSalaryComponents);
                        await context.SaveChangesAsync();
                    }

                    foreach (var data in response.ListEmployeeSalaryComponentsResponse)
                    {
                        EmployeeSalaryComponent esc = new EmployeeSalaryComponent();
                        esc.EmployeeId = existingEmployee.EmployeeId;
                        esc.CompanyId = existingEmployee.CompanyId;
                        esc.ComponentId = data.ComponentId;
                        esc.Formula = data.Formula;
                        esc.Amount = data.Amount;
                        await context.EmployeeSalaryComponents.AddAsync(esc);
                    } // end of foreach loop...
                    await context.SaveChangesAsync();
                } // end if...
            } // UpdateEmployee...

            public async Task SaveEmployeeRecord(EmployeesMainResponse response)
            {
                var trans = await context.Database.BeginTransactionAsync();
                await ErrorHandling(response);

                try
                {
                    if(response.EmployeeId == 0)
                    {
                        await CreateEmployee(response);
                    }
                    else
                    {
                        await UpdateEmployee(response);
                    }

                    await trans.CommitAsync();                    
                }
                catch(Exception ex)
                {
                    await trans.RollbackAsync();
                    await trans.DisposeAsync();
                    throw;
                }
                finally
                {
                    await trans.DisposeAsync();
                }
            } // SaveEmployeeRecord...
        } // class...
    } // namespace NEmployeesManagement...
}
