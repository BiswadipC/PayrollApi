using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class EmployeeSalaryComponent
{
    public int SalaryComponentId { get; set; }

    public int CompanyId { get; set; }

    public int EmployeeId { get; set; }

    public int ComponentId { get; set; }

    public decimal Amount { get; set; }

    public string? Formula { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual SalaryComponent Component { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
