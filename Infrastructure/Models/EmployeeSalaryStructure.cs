using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class EmployeeSalaryStructure
{
    public int StructureId { get; set; }

    public int CompanyId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime? EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public string PayFrequency { get; set; } = null!;

    public decimal AnnualCtc { get; set; }

    public decimal Basic { get; set; }

    public string IsActive { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
