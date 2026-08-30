using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class RefreshToken
{
    public int IdNo { get; set; }

    public string Token { get; set; } = null!;

    public int? UserId { get; set; }

    public string? IsValid { get; set; }

    public DateTime? ExpiresAt { get; set; }
}
