using Domain.EmployeesManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.EmployeesManagement
{
    public class EmployeesMainDTO
    {
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string DOB { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string HireDate { get; set; } = string.Empty;
        public string? TerminationDate { get; set; } = string.Empty;
        public int? EmployeeTypeId { get; set; }
        public string EmployeeTypeName { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int? DesignationId { get; set; }
        public string DesignationName { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
        public List<EmployeesBankDTO> ListEmployeesBankDTO { get; set; } = new List<EmployeesBankDTO>();
        public EmployeesSalaryStructuresDTO EmployeesSalaryStructures { get; set; } = new EmployeesSalaryStructuresDTO();
        public List<EmployeeSalaryComponentsDTO> ListEmployeeSalaryComponentsDTO { get; set; } = new List<EmployeeSalaryComponentsDTO>();
    } // class...

    public class EmployeesBankDTO
    {
        public int AccountId { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string IFSCCode { get; set; } = string.Empty;
        public string AccountHolderName { get; set; } = string.Empty;
        public string AccountNo { get; set; } = string.Empty;
    } // EmployeesBankDTO...

    public class EmployeesSalaryStructuresDTO
    {
        public int StructureId { get; set; }
        public string EffectiveFrom { get; set; } = string.Empty;
        public string? EffectiveTo { get; set; } = string.Empty;
        public string PayFrequency { get; set; } = string.Empty;
        public decimal AnnualCTC { get; set; } = decimal.Zero;
        public decimal? Basic { get; set; } = decimal.Zero;
        public string IsActive { get; set; } = string.Empty;
    } // EmployeesSalaryStructuresDTO...

    public class EmployeeSalaryComponentsDTO
    {
        public int EmployeeSalaryComponentId { get; set; }
        public int ComponentId { get; set; }
        public string ComponentCode { get; set; } = string.Empty;
        public string ComponentName { get; set; } = string.Empty;
        public string? Formula { get; set; } = string.Empty;
        public decimal Amount { get; set; } = decimal.Zero;
    } // EmployeeSalaryComponentsDTO...
}
