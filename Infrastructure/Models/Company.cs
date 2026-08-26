using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyCode { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string? Gstin { get; set; }

    public string? RegistrationNo { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? Pin { get; set; }

    public string? CurrencyCode { get; set; }

    public virtual ICollection<SalaryComponent> SalaryComponents { get; set; } = new List<SalaryComponent>();
}
