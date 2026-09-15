using Application.DTO.EmployeesManagement;
using AutoMapper;
using Domain.EmployeesManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.EmployeesManagement
{
    public class EmployeesManagementService
    {
        private readonly IEmployeesManagement iemployees;
        private readonly IMapper mapper;

        public EmployeesManagementService(IEmployeesManagement iemployees, IMapper mapper)
        {
            this.iemployees = iemployees;
            this.mapper = mapper;
        } // constructor...

        public async Task<List<EmployeesMainDTO>> GetEmployees()
        {
            var employees = await iemployees.GetEmployees();
            var dtos = mapper.Map<List<EmployeesMainDTO>>(employees);
            return dtos.ToList();
        } // GetEmployees...

        public async Task<List<EmployeesBankDTO>> GetemployeesBanksByEmployeeId(int employeeId)
        {
            var employeeBanks = await iemployees.GetemployeesBanksByEmployeeId(employeeId);
            var dtos = mapper.Map<List<EmployeesBankDTO>>(employeeBanks);
            return dtos.ToList();
        } // GetemployeesBanksByEmployeeId...

        public async Task<EmployeesSalaryStructuresDTO> GetEmployeesSalaryStructureByEmployeeId(int employeeId)
        {
            var employeeSalaryStructure = await iemployees.GetEmployeeSalaryStructureByEmployeeId(employeeId);
            var dto = mapper.Map<EmployeesSalaryStructuresDTO>(employeeSalaryStructure);
            return dto;
        } // GetEmployeesSalaryStructureByEmployeeId...

        public async Task<List<EmployeeSalaryComponentsDTO>> GetEmployeesSalaryComponentsByEmployeeId(int employeeId)
        {
            var employeeSalaryComponents = await iemployees.GetemployeesSalarycomponentsByEmployeeId(employeeId);
            var dtos = mapper.Map<List<EmployeeSalaryComponentsDTO>>(employeeSalaryComponents);
            return dtos;
        } // GetEmployeesSalaryComponentsByEmployeeId...

        public async Task<EmployeesMainDTO> GetEmployeeByEmployeeId(int employeeId)
        {
            var employee = await iemployees.GetEmployeeByEmployeeId(employeeId);
            var dto = mapper.Map<EmployeesMainDTO>(employee);
            return dto;
        } // GetEmployeeByEmployeeId...

        public async Task Save(EmployeesMainDTO dto)
        {
            var response = mapper.Map<EmployeesMainResponse>(dto);
            await iemployees.Save(response);
        } // Save...
    } // class...
}
