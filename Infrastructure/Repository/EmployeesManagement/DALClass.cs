using Application.Repository.Company;
using Application.Repository.EmployeesManagement;
using Domain.EmployeesManagement;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.EmployeesManagement
{
    internal sealed class DALClass : IEmployeesManagement
    {
        private readonly PayrollContext context;
        private readonly ICompany ic;

        public DALClass(PayrollContext context, ICompany ic)
        {
            this.context = context;
            this.ic = ic;
        } // constructor...

        public async Task<List<EmployeesMainResponse>> GetEmployees()
        {
            var employees = await (from emp in context.Employees
             join type in context.EmployeeTypes
             on emp.EmployeeTypeId equals type.TypeId
             join desig in context.Designations
             on emp.DesignationId equals desig.IdNo
             join dept in context.Departments
             on emp.DepartmentId equals dept.IdNo
             join mgr in context.Employees
             on emp.ManagerId equals mgr.EmployeeId into mgrGroup
             from newMgr in mgrGroup.DefaultIfEmpty()
             select new EmployeesMainResponse(emp.EmployeeId, emp.CompanyId, emp.EmployeeCode, emp.EmployeeName, emp.DateOfBirty!.Value.ToString("dd/MM/yyyy"),
                emp.Gender!, emp.Email ?? string.Empty, emp.Phone ?? string.Empty, emp.HireDate.ToString("dd/MM/yyyy"),
                emp.TerminationDate.HasValue ? emp.TerminationDate!.Value.ToString("dd/MM/yyyy") : null, emp.EmployeeTypeId, type.TypeName,
                emp.DepartmentId, dept.Name!, emp.DesignationId, desig.Name, emp.ManagerId, newMgr.EmployeeName, emp.AddressLine1 ?? string.Empty,
                emp.AddressLine2 ?? string.Empty, emp.City, emp.State, emp.Country ?? string.Empty, emp.PostalCode ?? string.Empty, null, null, null)
             ).ToListAsync();

            return employees;
        } // GetEmployees...

        public async Task<List<EmployeesBankResponse>> GetemployeesBanksByEmployeeId(int employeeId)
        {
            var responses = await (from eb in context.EmployeeBankAccounts
             join b in context.Banks
             on eb.BankId equals b.BankId
             join br in context.Branches
             on eb.BranchId equals br.BranchId
             select new EmployeesBankResponse()
             {
                 AccountId = eb.AccountId,
                 BankId = eb.BankId,
                 BankName = b.BankName,
                 BranchId = eb.BranchId,
                 BranchName = br.BranchName,
                 IFSCCode = br.Ifsccode,
                 AccountHolderName = eb.AccountHolderName ?? string.Empty,
                 AccountNo = eb.AccountNo,
             }).ToListAsync();

            return responses;
        } // GetemployeesBanksByEmployeeId...

        public async Task<EmployeesSalaryStructuresResponse> GetEmployeeSalaryStructureByEmployeeId(int employeeId)
        {
            var structure = await (from s in context.EmployeeSalaryStructures
             where s.EmployeeId == employeeId
             select new EmployeesSalaryStructuresResponse()
             {
                 StructureId = s.StructureId,
                 EffectiveFrom = s.EffectiveFrom.HasValue ? s.EffectiveFrom.Value.ToString("dd/MM/yyyy") : string.Empty,
                 EffectiveTo = s.EffectiveTo.HasValue ? s.EffectiveTo.Value.ToString("dd/MM/yyyy") : string.Empty,
                 PayFrequency = s.PayFrequency,
                 AnnualCTC = s.AnnualCtc,
                 Basic = s.Basic,
                 IsActive = s.IsActive
             }).FirstOrDefaultAsync();

            return structure ?? new EmployeesSalaryStructuresResponse();
        } // GetEmployeeSalaryStructureByEmployeeId...

        public async Task<List<EmployeeSalaryComponentsResponse>> GetemployeesSalarycomponentsByEmployeeId(int employeeId)
        {
            var components = await (from e in context.EmployeeSalaryComponents
             join c in context.SalaryComponents
             on e.ComponentId equals c.CompanyId
             select new EmployeeSalaryComponentsResponse()
             {
                 EmployeeSalaryComponentId = e.SalaryComponentId,
                 ComponentId = e.ComponentId,
                 ComponentCode = c.ComponentCode,
                 ComponentName = c.ComponentName,
                 Formula = e.Formula ?? string.Empty,
                 Amount = e.Amount
             }).ToListAsync();

            return components;
        } // GetemployeesSalarycomponentsByEmployeeId...

        public async Task<EmployeesMainResponse> GetEmployeeByEmployeeId(int employeeId)
        {
            var banks = await GetemployeesBanksByEmployeeId(employeeId);
            var structure = await GetEmployeeSalaryStructureByEmployeeId(employeeId);
            var components = await GetemployeesSalarycomponentsByEmployeeId(employeeId);

            var employee = await
            (from emp in context.Employees
             join type in context.EmployeeTypes
             on emp.EmployeeTypeId equals type.TypeId
             join desig in context.Designations
             on emp.DesignationId equals desig.IdNo
             join dept in context.Departments
             on emp.DepartmentId equals dept.IdNo
             join mgr in context.Employees
             on emp.ManagerId equals mgr.EmployeeId into mgrGroup
             from newMgr in mgrGroup.DefaultIfEmpty()
             where emp.EmployeeId == employeeId
             select new EmployeesMainResponse(emp.EmployeeId, emp.CompanyId, emp.EmployeeCode, emp.EmployeeName, emp.DateOfBirty!.Value.ToString("dd/MM/yyyy"),
                emp.Gender!, emp.Email ?? string.Empty, emp.Phone ?? string.Empty, emp.HireDate.ToString("dd/MM/yyyy"),
                emp.TerminationDate.HasValue ? emp.TerminationDate!.Value.ToString("dd/MM/yyyy") : null, emp.EmployeeTypeId, type.TypeName,
                emp.DepartmentId, dept.Name!, emp.DesignationId, desig.Name, emp.ManagerId, newMgr.EmployeeName, emp.AddressLine1 ?? string.Empty,
                emp.AddressLine2 ?? string.Empty, emp.City, emp.State, emp.Country ?? string.Empty, emp.PostalCode ?? string.Empty,
                banks, structure, components)
             ).FirstOrDefaultAsync();

            return employee!;
        } // GetEmployeeByEmployeeId...

        private async Task<string> CreateEmployeeCode(int companyId)
        {
            string employeeCode = string.Empty;
            var companyResponse = await ic.GetCompanyByCompanyId(companyId);
            string compCode = companyResponse.CompanyCode;

            var countEmployees = context.Employees.Count(x =>  x.CompanyId == companyId);
            if(countEmployees == 0)
            {
                employeeCode = compCode + "000001";
            }
            else
            {
                int maxno = ((context.Employees.Max(x => Convert.ToInt32(employeeCode.Substring(4))) + 1) + 1);
                if(maxno.ToString().Length == 1)
                {
                    employeeCode = compCode + "00000" + maxno.ToString();
                }
                else if(maxno.ToString().Length == 2)
                {
                    employeeCode = compCode + "0000" + maxno.ToString();
                }
                else if (maxno.ToString().Length == 3)
                {
                    employeeCode = compCode + "000" + maxno.ToString();
                }
                else if (maxno.ToString().Length == 4)
                {
                    employeeCode = compCode + "00" + maxno.ToString();
                }
                else if (maxno.ToString().Length == 5)
                {
                    employeeCode = compCode + "0" + maxno.ToString();
                }
                else if (maxno.ToString().Length == 6)
                {
                    employeeCode = compCode + maxno.ToString();
                }
            } // end if...

            return employeeCode;
        } // CreateEmployeeCode...

        private async Task CreateEmployee(EmployeesMainResponse response)
        {
            Employee employee = new Employee();
            employee.CompanyId = response.CompanyId;
            employee.EmployeeCode = await CreateEmployeeCode(response.CompanyId);
            employee.EmployeeName = response.EmployeeName;
            employee.DateOfBirty = DateTime.ParseExact(response.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            employee.Gender = response.Gender;
            employee.Email = response.Email;
            employee.Phone = response.Phone;
            employee.HireDate = DateTime.ParseExact(response.HireDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            employee.TerminationDate = string.IsNullOrWhiteSpace(response.TerminationDate) ? null :
                                DateTime.ParseExact(response.TerminationDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            employee.EmployeeTypeId = (int)response.EmployeeTypeId!;
            employee.IsActive = "Yes";
            employee.DepartmentId = response.DepartmentId;
            employee.DesignationId = response.DesignationId;
            employee.ManagerId = response.ManagerId.HasValue ? response.ManagerId : null;
            employee.AddressLine1 = response.AddressLine1;
            employee.AddressLine2 = response.AddressLine2;
            employee.City = response.City;
            employee.State = response.State;
            employee.Country = response.Country;
            employee.PostalCode = response.Pin;
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();

            if(response.ListEmployeesBankResponse != null && response.ListEmployeesBankResponse.Count() > 0)
            {
                foreach(var data in response.ListEmployeesBankResponse)
                {
                    EmployeeBankAccount bank = new EmployeeBankAccount();
                    bank.CompanyId = response.CompanyId;
                    bank.EmployeeId = employee.EmployeeId;
                    bank.BankId = data.BankId;
                    bank.BranchId = data.BranchId;
                    bank.AccountHolderName = data.AccountHolderName ?? string.Empty;
                    bank.AccountNo = data.AccountNo;
                    await context.EmployeeBankAccounts.AddAsync(bank);
                }
                await context.SaveChangesAsync();
            } // end if...

            EmployeeSalaryStructure structure = new EmployeeSalaryStructure();
            structure.CompanyId = response.CompanyId;
            structure.EmployeeId = employee.EmployeeId;
            structure.EffectiveFrom = string.IsNullOrWhiteSpace(response.EmployeesSalaryStructures.EffectiveFrom) ? null :
                                DateTime.ParseExact(response.EmployeesSalaryStructures.EffectiveFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            structure.EffectiveTo = string.IsNullOrWhiteSpace(response.EmployeesSalaryStructures.EffectiveTo) ? null :
                                DateTime.ParseExact(response.EmployeesSalaryStructures.EffectiveTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            structure.PayFrequency = string.IsNullOrWhiteSpace(response.EmployeesSalaryStructures.PayFrequency) ? "MONTHLY" : response.EmployeesSalaryStructures.PayFrequency;
            structure.AnnualCtc = response.EmployeesSalaryStructures.AnnualCTC;
            structure.Basic = response.EmployeesSalaryStructures.Basic ?? decimal.Zero;
            structure.IsActive = "Yes";
            await context.EmployeeSalaryStructures.AddAsync(structure);
            await context.SaveChangesAsync();

            if(response.ListEmployeeSalaryComponentsResponse != null && response.ListEmployeeSalaryComponentsResponse.Count() > 0)
            {
                foreach(var data in response.ListEmployeeSalaryComponentsResponse)
                {
                    EmployeeSalaryComponent component = new EmployeeSalaryComponent();
                    component.CompanyId = response.CompanyId;
                    component.EmployeeId = employee.EmployeeId;
                    component.ComponentId = data.ComponentId;
                    component.Formula = data.Formula ?? string.Empty;
                    component.Amount = data.Amount;
                    await context.EmployeeSalaryComponents.AddAsync(component);
                }
                await context.SaveChangesAsync();
            } // end if...
        } // CreateEmployee...

        private async Task UpdateEmployee(EmployeesMainResponse response)
        {
            var existingEmployee = await context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == response.EmployeeId);
            existingEmployee!.EmployeeName = response.EmployeeName;
            existingEmployee.DateOfBirty = DateTime.ParseExact(response.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            existingEmployee.Gender = response.Gender;
            existingEmployee.Email = response.Email;
            existingEmployee.Phone = response.Phone;
            existingEmployee.HireDate = DateTime.ParseExact(response.HireDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            existingEmployee.TerminationDate = string.IsNullOrWhiteSpace(response.TerminationDate) ? null :
                    DateTime.ParseExact(response.TerminationDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            existingEmployee.EmployeeTypeId = (int)response.EmployeeTypeId!;
            existingEmployee.DepartmentId = response.DepartmentId;
            existingEmployee.DesignationId = response.DesignationId;
            existingEmployee.ManagerId = response.ManagerId.HasValue ? response.ManagerId : null;
            existingEmployee.AddressLine1 = response.AddressLine1;
            existingEmployee.AddressLine2 = response.AddressLine2;
            existingEmployee.City = response.City;
            existingEmployee.State = response.State;
            existingEmployee.Country = response.Country;
            existingEmployee.PostalCode = response.Pin;
            context.Update(existingEmployee);
            await context.SaveChangesAsync();

            if (response.ListEmployeesBankResponse != null && response.ListEmployeesBankResponse.Count() > 0)
            {
                foreach (var data in response.ListEmployeesBankResponse)
                {
                    if (data.AccountId == 0)
                    {
                        EmployeeBankAccount bank = new EmployeeBankAccount();
                        bank.CompanyId = response.CompanyId;
                        bank.EmployeeId = existingEmployee.EmployeeId;
                        bank.BankId = data.BankId;
                        bank.BranchId = data.BranchId;
                        bank.AccountHolderName = data.AccountHolderName ?? string.Empty;
                        bank.AccountNo = data.AccountNo;
                        await context.EmployeeBankAccounts.AddAsync(bank);
                    }
                    else
                    {
                        var existingAccount = await context.EmployeeBankAccounts.FirstOrDefaultAsync(x => x.AccountId == data.AccountId);
                        existingAccount!.BankId = data.BankId;
                        existingAccount.BranchId = data.BranchId;
                        existingAccount.AccountHolderName = data.AccountHolderName ?? string.Empty;
                        existingAccount.AccountNo = data.AccountNo;
                        context.Update(existingAccount);
                    }
                } // end of foreach loop...
                await context.SaveChangesAsync();
            } // end if...

            var existingStructure = await context.EmployeeSalaryStructures.FirstOrDefaultAsync(m => m.StructureId == response.EmployeesSalaryStructures.StructureId);
            existingStructure!.EffectiveFrom = string.IsNullOrWhiteSpace(response.EmployeesSalaryStructures.EffectiveFrom) ? null :
                                DateTime.ParseExact(response.EmployeesSalaryStructures.EffectiveFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            existingStructure.EffectiveTo = string.IsNullOrWhiteSpace(response.EmployeesSalaryStructures.EffectiveTo) ? null :
                                DateTime.ParseExact(response.EmployeesSalaryStructures.EffectiveTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            existingStructure.PayFrequency = response.EmployeesSalaryStructures.PayFrequency ?? "MONTHLY";
            existingStructure.AnnualCtc = response.EmployeesSalaryStructures.AnnualCTC;
            existingStructure.Basic = response.EmployeesSalaryStructures.Basic ?? decimal.Zero;
            await context.EmployeeSalaryStructures.AddAsync(existingStructure);
            await context.SaveChangesAsync();

            if (response.ListEmployeeSalaryComponentsResponse != null && response.ListEmployeeSalaryComponentsResponse.Count() > 0)
            {
                var components = await context.EmployeeSalaryComponents.Where(m => m.EmployeeId == response.EmployeeId).ToListAsync();
                context.EmployeeSalaryComponents.RemoveRange(components);
                await context.SaveChangesAsync();

                foreach (var data in response.ListEmployeeSalaryComponentsResponse)
                {
                    EmployeeSalaryComponent component = new EmployeeSalaryComponent();
                    component.CompanyId = response.CompanyId;
                    component.EmployeeId = existingEmployee.EmployeeId;
                    component.ComponentId = data.ComponentId;
                    component.Formula = data.Formula ?? string.Empty;
                    component.Amount = data.Amount;
                    await context.EmployeeSalaryComponents.AddAsync(component);
                }
                await context.SaveChangesAsync();
            } // end if...
        } // UpdateEmployee...

        public async Task Save(EmployeesMainResponse response)
        {
            var trans = await context.Database.BeginTransactionAsync();

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
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                throw;
            }
            finally
            {
                trans.Dispose();
            }
        } // Save...
    } // class...
}
