using Domain.EmployeesManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.EmployeesManagement
{
    public interface IEmployeeManagement
    {
        Task<List<EmployeesBankResponse>> GetEmployeeBanksByEmployeeId(int empId);
        Task<EmployeesSalaryStructuresResponse> GetEmployeeSalaryStructureByEmployeeId(int empId);
        Task<List<EmployeeSalaryComponentsResponse>> GetEmployeeSalaryComponentsByEmployeeId(int empId);
        Task<EmployeesMainResponse> GetEmployeeResponseByEmployeeId(int empId);
        Task SaveEmployeeRecord(EmployeesMainResponse response);
        Task<EmployeeSalaryComponentsResponse> GetFormulaResponse(FormulaRequestDTO formulaDTO);        
    } // interface...
}
