using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Module
{
    public string ModuleName { get; set; } = null!;

    public virtual ICollection<UserModulesPolicyMapping> UserModulesPolicyMappings { get; set; } = new List<UserModulesPolicyMapping>();
}
