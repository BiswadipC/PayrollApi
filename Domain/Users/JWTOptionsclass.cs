using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Users
{
    public class JWTOptionsClass
    {
        public string Issuer {  get; set; } = string.Empty;
        public string Audience {  get; set; } = string.Empty;
        public string SecretKey {  get; set; } = string.Empty;
        public int AccessTokenExpirationMinutes {  get; set; }
        public int RefreshTokenExpirationDays {  get; set; }
    } // JWTOptionsClass...
}
