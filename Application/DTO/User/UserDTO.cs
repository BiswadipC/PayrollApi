using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.User
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string IsAdmin { get; set; } = string.Empty;
    } // class..

    public class CreateUserDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ReTypePassword {  get; set; } = string.Empty;
    } // class...

    public class AuthenticateUserDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    } // class...

    public class UserModulesPolicyDTO
    {
        public int IdNo { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string PolicyName { get; set; } = string.Empty;
        public string PermissionType { get; set; } = string.Empty;
    } // class...

    public class UserClaimsDTO
    {
        public string PolicyName { get; set; } = string.Empty;
        public string PermissionType { get; private set; } = string.Empty;
    } // class...
}
