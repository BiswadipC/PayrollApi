using Domain.EmployeesManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.EmployeesManagement
{
    public interface IEmployeesManagement
    {
        Task<List<EmployeesMainResponse>> GetEmployees();
        Task<List<EmployeesBankResponse>> GetemployeesBanksByEmployeeId(int employeeId);
        Task<EmployeesSalaryStructuresResponse> GetEmployeeSalaryStructureByEmployeeId(int employeeId);
        Task<List<EmployeeSalaryComponentsResponse>> GetemployeesSalarycomponentsByEmployeeId(int employeeId);
        Task<EmployeesMainResponse> GetEmployeeByEmployeeId(int employeeId);
        Task Save(EmployeesMainResponse response);
    } // interface...
}
