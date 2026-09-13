using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class JWTOptionsClass
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audiance {  get; set; } = string.Empty;
        public string SecurityKey {  get; set; } = string.Empty;
        public int JWTTokenInMinutes {  get; set; }
        public int RefreshTokenInDays { get; set; }
    } // class...
}
