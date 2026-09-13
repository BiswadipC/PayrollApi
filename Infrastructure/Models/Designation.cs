using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Designation
{
    public int IdNo { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
