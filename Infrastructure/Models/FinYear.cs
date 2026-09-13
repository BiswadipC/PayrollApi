using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class FinYear
{
    public int YearId { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public int? CompanyId { get; set; }

    public virtual Company? Company { get; set; }
}
