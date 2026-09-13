using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class EmployeeType
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;
}
