using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class EmployeeBankAccount
{
    public int AccountId { get; set; }

    public int? CompanyId { get; set; }

    public int EmployeeId { get; set; }

    public int BankId { get; set; }

    public int BranchId { get; set; }

    public string? AccountHolderName { get; set; }

    public string AccountNo { get; set; } = null!;

    public virtual Bank Bank { get; set; } = null!;

    public virtual Branch Branch { get; set; } = null!;

    public virtual Company? Company { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
