using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.User
{
    public class UserResponse
    {
        public int UserId {  get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public string IsAdmin { get; private set; } = string.Empty;

        public UserResponse(int userId, string userName, string password, string isAdmin)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            IsAdmin = isAdmin;
        } // constructor...
    } // class..

    public class CreateUserResponse
    {
        public string UserName { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public string ReTypePassword { get; private set; } = string.Empty;

        public CreateUserResponse(string userName, string password, string reTypePassword)
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add("Username cannot be blank.");
            }
            else if (userName.Trim().Length < 4)
            {
                errors.Add("Username cannot be less than 4 characters length.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password cannot be blank.");
            }            
            else if (password.Trim().Length < 4)
            {
                errors.Add("Password cannot be less than 4 characters length.");
            }

            if(!password.Equals(reTypePassword))
            {
                errors.Add("Both password must match.");
            }

            if(errors.Any())
            {
                throw new BadRequestClass(new Dictionary<string, string[]>
                {
                    {GlobalConstantClass.BadRequestKey, errors.ToArray() }
                });
            }

            UserName = userName;
            Password = password;
            ReTypePassword = reTypePassword;
        } // constructor...
    } // class..

    public class AuthenticateUserResponse
    {
        public string UserName { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;

        public AuthenticateUserResponse(string userName, string password)
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add("Username cannot be blank.");
            }
            
            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password cannot be blank.");
            }

            if (errors.Any())
            {
                throw new BadRequestClass(new Dictionary<string, string[]>
                {
                    {GlobalConstantClass.BadRequestKey, errors.ToArray() }
                });
            }

            UserName = userName;
            Password = password;
        } // constructor...
    } // class..

    public class UserModulesPolicyResponse
    {
        public int IdNo {  get; private set; }
        public int UserId { get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public string ModuleName {  get; private set; } = string.Empty;
        public string PolicyName {  get; private set; } = string.Empty;
        public string PermissionType {  get; private set; } = string.Empty;

        public UserModulesPolicyResponse(int idNo, int userId, string userName, string moduleName, string policyName, string permissionType)
        {
            IdNo = idNo;
            UserId = userId;
            UserName = userName;
            ModuleName = moduleName;
            PolicyName = policyName;
            PermissionType = permissionType;
        } // constructor...
    } // class...

    public class UserClaimsResponse
    {
        public string PolicyName { get; private set; } = string.Empty;
        public string PermissionType { get; private set; } = string.Empty;

        public UserClaimsResponse(string policyName, string permissionType)
        {
            PolicyName = policyName;
            PermissionType = permissionType;
        } // constructor...
    } //class...
}
