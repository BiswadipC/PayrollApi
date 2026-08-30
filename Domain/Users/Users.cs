using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Users
{
    public class UserResponse
    {
        public int UserId {  get; set; }
        public string UserName { get; set; } = string.Empty;
        public string IsAdmin { get; set; } = string.Empty;
    } // class...

    public class UserCreationClass
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ReTypePassword { get; set; } = string.Empty;
    } // UserCreationClass class...

    public class UserAuthentication
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    } // class...

    public class UserModulesPolicyMappingResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ModuleName {  get; set; } = string.Empty;
        public string PolicyName { get; set; } = string.Empty;
        public string PermissionType { get; set; } = string.Empty;
    } // class...
}
