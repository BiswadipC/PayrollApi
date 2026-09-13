using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class UserModulesPolicyMapping
{
    public int IdNo { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string ModuleName { get; set; } = null!;

    public string PolicyName { get; set; } = null!;

    public string PermissionType { get; set; } = null!;

    public virtual Module ModuleNameNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
