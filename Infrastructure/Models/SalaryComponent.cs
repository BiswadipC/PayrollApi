using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class SalaryComponent
{
    public int ComponentId { get; set; }

    public int CompanyId { get; set; }

    public string ComponentCode { get; set; } = null!;

    public string ComponentName { get; set; } = null!;

    public string ComponentType { get; set; } = null!;

    public string CalculationType { get; set; } = null!;

    public string Taxable { get; set; } = null!;

    public string IsActive { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<EmployeeSalaryComponent> EmployeeSalaryComponents { get; set; } = new List<EmployeeSalaryComponent>();
}
