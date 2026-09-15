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

        public DALClass(PayrollContext context)
        {
            this.context = context;
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

        public Task Save(EmployeesMainResponse response)
        {
            throw new NotImplementedException();
        }
    } // class...
}
