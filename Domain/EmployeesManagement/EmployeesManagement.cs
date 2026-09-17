using Domain.Common;
using Domain.SalaryComponent;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.EmployeesManagement
{
    public class EmployeesMainResponse
    {
        public int EmployeeId {  get; private set; }
        public int CompanyId {  get; private set; }
        public string EmployeeCode {  get; private set; } = string.Empty;
        public string EmployeeName { get; private set; } = string.Empty;
        public string DOB { get; private set; } = string.Empty;
        public string Gender {  get; private set; } = string.Empty;
        public string? Email {  get; private set; } = string.Empty;
        public string Phone {  get; private set; } = string.Empty;
        public string HireDate {  get; private set; } = string.Empty;
        public string? TerminationDate {  get; private set; } = string.Empty;
        public int? EmployeeTypeId {  get; private set; }
        public string EmployeeTypeName {  get; private set; } = string.Empty;
        public int? DepartmentId { get; private set; }
        public string DepartmentName { get; private set; } = string.Empty;
        public int? DesignationId { get; private set; }
        public string DesignationName { get; private set; } = string.Empty;
        public int? ManagerId { get; private set; }
        public string? ManagerName { get; private set; } = string.Empty;
        public string AddressLine1 {  get; private set; } = string.Empty;
        public string? AddressLine2 { get; private set; } = string.Empty;
        public string? City {  get; private set; } = string.Empty;
        public string? State {  get; private set; } = string.Empty;
        public string Country {  get; private set; } = string.Empty;
        public string Pin {  get; private set; } = string.Empty;
        public List<EmployeesBankResponse> ListEmployeesBankResponse {  get; private set; } = new List<EmployeesBankResponse>();
        public EmployeesSalaryStructuresResponse EmployeesSalaryStructures {  get; private set; } = new EmployeesSalaryStructuresResponse();
        public List<EmployeeSalaryComponentsResponse> ListEmployeeSalaryComponentsResponse { get; private set; } = new List<EmployeeSalaryComponentsResponse>();

        public EmployeesMainResponse(int employeeId, int companyId, string employeeCode, string employeeName, string dOB, string gender, string? email, 
            string phone, string hireDate, string? terminationDate, int? employeeTypeId, string employeeTypeName, int? departmentId, string departmentName, int? designationId, 
            string designationName, int? managerId, string? managerName, string addressLine1, string? addressLine2, string? city, string? state, 
            string country, string pin, List<EmployeesBankResponse> listEmployeesBankResponse, EmployeesSalaryStructuresResponse employeesSalaryStructures, 
            List<EmployeeSalaryComponentsResponse> listEmployeeSalaryComponentsResponse)
        {
            List<string> errors = new List<string>();
            /******************************************************************** business rules ***********************************************************************/
            if (string.IsNullOrWhiteSpace(employeeName))
            {
                errors.Add("Employee Name cannot be blank.");
            }
            if (string.IsNullOrWhiteSpace(dOB))
            {
                errors.Add("Employee \'Date of Birth\' cannot be blank.");
            }
            if(!DateOnly.TryParseExact(dOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date1))
            {
                errors.Add("Invalid value for \'Date of Birth\'");
            }
            if(string.IsNullOrWhiteSpace(gender))
            {
                errors.Add("Gender is missing.");
            }
            if(!DateOnly.TryParseExact(hireDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date2))
            {
                errors.Add("Invalid value for \'Date of Joining\'");
            }
            if(!employeeTypeId.HasValue || employeeTypeId == 0)
            {
                errors.Add("Specify an employee type.");
            }
            if(!designationId.HasValue || designationId == 0)
            {
                errors.Add("Designation cannot be blank.");
            }
            if(!departmentId.HasValue || departmentId == 0)
            {
                errors.Add("Department cannot be blank.");
            }

            if (listEmployeesBankResponse != null && listEmployeesBankResponse.Count() > 0)
            {
                foreach (var b in listEmployeesBankResponse)
                {                    
                    if(b.BankId == 0)
                    {
                        errors.Add("Select a Bank to proceed.");
                    }
                    if (b.BranchId == 0)
                    {
                        errors.Add("Select a Branch to proceed.");
                    }
                    if (string.IsNullOrEmpty(b.AccountNo))
                    {
                        errors.Add("Account No. cannot be blank.");
                    }
                    break;
                } // foreach loop...

                bool hasDuplicateBanks = (from x in listEmployeesBankResponse
                 group new { x } by
                 new { x.BankId } into bankGrp
                 select new
                 {
                     BankId = bankGrp.Key.BankId,
                     CountBanks = bankGrp.Count()
                 }).Any(m => m.CountBanks > 1);

                if(hasDuplicateBanks)
                {
                    errors.Add("Duplicate bank entry not allowed.");
                }

                bool hasDuplicateBranches = (from x in listEmployeesBankResponse
                 group new { x } by new
                 {
                     BranchId = x.BranchId,
                 } into branchGrp
                 select new
                 {
                     BranchId = branchGrp.Key.BranchId,
                     CountBranches = branchGrp.Count()
                 }).Any(m => m.CountBranches > 1);

                if (hasDuplicateBranches)
                {
                    errors.Add("Duplicate branch entry not allowed.");
                }
            } // end if...

            if(listEmployeeSalaryComponentsResponse != null && listEmployeeSalaryComponentsResponse.Count() > 0)
            {
                foreach(var comp in listEmployeeSalaryComponentsResponse)
                {
                    if(comp.ComponentId == 0)
                    {
                        errors.Add("select a component to proceed.");
                    }
                    if(comp.Amount <= 0)
                    {
                        errors.Add("Invalid component amount. Enter a valid amount.");
                    }
                } // foreach loop...
            } // end if...

            if(listEmployeeSalaryComponentsResponse != null && listEmployeeSalaryComponentsResponse.Count() > 0)
            {
                bool hasDuplicateComponents = (from comp in listEmployeeSalaryComponentsResponse
                 group new { comp } by new { ComponentId = comp.ComponentId } into compGrp
                 select new
                 {
                     ComponentId = compGrp.Key.ComponentId,
                     CountComponents = compGrp.Count()
                 }).Any(x => x.CountComponents > 1);

                if (hasDuplicateComponents)
                {
                    errors.Add("Duplicate Component entry not allowed.");
                }
            } // end if...

            if(errors.Any())
            {
                throw new BadRequestClass(new Dictionary<string, string[]>()
                {
                    {GlobalConstantClass.BadRequestKey, errors.ToArray() }
                });
            }
            /***********************************************************************************************************************************************************/
            EmployeeId = employeeId;
            CompanyId = companyId;
            EmployeeCode = employeeCode;
            EmployeeName = employeeName;
            DOB = dOB;
            Gender = gender;
            Email = email;
            Phone = phone;
            HireDate = hireDate;
            TerminationDate = terminationDate;
            EmployeeTypeId = employeeTypeId.HasValue ? employeeTypeId.Value : null;
            EmployeeTypeName = employeeTypeName;
            DepartmentId = departmentId;
            DepartmentName = departmentName;
            DesignationId = designationId;
            DesignationName = designationName;
            ManagerId = managerId;
            ManagerName = managerName;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            Country = country;
            Pin = pin;
            ListEmployeesBankResponse = listEmployeesBankResponse ?? new List<EmployeesBankResponse>();
            EmployeesSalaryStructures = employeesSalaryStructures ?? new EmployeesSalaryStructuresResponse();
            ListEmployeeSalaryComponentsResponse = listEmployeeSalaryComponentsResponse ?? new List<EmployeeSalaryComponentsResponse>();
        }
    } // EmployeesMainResponse...

    public class EmployeesBankResponse
    {
        public int AccountId {  get; set; }
        public int BankId {  get; set; }
        public string BankName { get; set; } = string.Empty;
        public int BranchId {  get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string IFSCCode { get;  set; } = string.Empty;
        public string AccountHolderName {  get; set; } = string.Empty;
        public string AccountNo {  get; set; } = string.Empty;
    } // EmployeesBankResponse...

    public class EmployeesSalaryStructuresResponse
    {
        public int StructureId {  get; set; }
        public string EffectiveFrom {  get; set; } = string.Empty;
        public string? EffectiveTo {  get; set; } = string.Empty;
        public string PayFrequency {  get; set; } = string.Empty;
        public decimal AnnualCTC {  get; set; } = decimal.Zero;
        public decimal? Basic {  get; set; } = decimal.Zero;
        public string IsActive {  get; set; } = string.Empty;
    } // EmployeesSalaryStructuresResponse...

    public class EmployeeSalaryComponentsResponse
    {
        public int EmployeeSalaryComponentId {  get; set; }
        public int ComponentId { get; set; }
        public string ComponentCode {  get; set; } = string.Empty;
        public string ComponentName {  get; set; } = string.Empty;
        public string? Formula {  get; set; } = string.Empty;
        public decimal Amount {  get; set; } = decimal.Zero;
    } // EmployeeSalaryComponentsResponse...
}
