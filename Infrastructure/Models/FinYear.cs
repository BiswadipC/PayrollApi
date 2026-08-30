using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class FinYear
{
    public int YaarId { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }
}
