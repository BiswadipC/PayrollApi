using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int CompanyId { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public DateTime? DateOfBirty { get; set; }

    public string? Gender { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime HireDate { get; set; }

    public DateTime? TerminationDate { get; set; }

    public int EmployeeTypeId { get; set; }

    public string IsActive { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public int? DesignationId { get; set; }

    public int? ManagerId { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Designation? Designation { get; set; }

    public virtual ICollection<EmployeeBankAccount> EmployeeBankAccounts { get; set; } = new List<EmployeeBankAccount>();

    public virtual ICollection<EmployeeSalaryComponent> EmployeeSalaryComponents { get; set; } = new List<EmployeeSalaryComponent>();

    public virtual ICollection<EmployeeSalaryStructure> EmployeeSalaryStructures { get; set; } = new List<EmployeeSalaryStructure>();

    public virtual ICollection<Employee> InverseManager { get; set; } = new List<Employee>();

    public virtual Employee? Manager { get; set; }
}
