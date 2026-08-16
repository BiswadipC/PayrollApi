using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Branch
{
    public int BranchId { get; set; }

    public string BranchName { get; set; } = null!;

    public string BranchCode { get; set; } = null!;

    public string Ifsccode { get; set; } = null!;

    public string? Address { get; set; }

    public string? PhoneNo { get; set; }

    public int BankId { get; set; }

    public virtual Bank Bank { get; set; } = null!;
}
