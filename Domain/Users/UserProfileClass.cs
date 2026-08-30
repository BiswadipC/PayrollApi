using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Users
{
    public class UserPermissionClass
    {
        public string PolicyName { get; set; } = string.Empty;
        public string PermissionType {  get; set; } = string.Empty;
    } // UserPermissionClass class...

    //public class UserProfileClass
    //{
    //    private static readonly IHttpContextAccessor httpContextAccessor;

    //    public UserProfileClass(IHttpContextAccessor httpContextAccessor)
    //    {
    //        this.httpContextAccessor = UserProfileClass.httpContextAccessor;
    //    } // constructor...

    //    public string UserId {  get; set; } = string.Empty;
    //    public string UserName { get; set; } = string.Empty;
    //    public string IsAdmin {  get; set; } = string.Empty;
    //    public string CompanyId {  get; set; } = string.Empty;
    //    public string DateFrom {  get; set; } = string.Empty;
    //    public string DateTo {  get; set; } = string.Empty;
    //    public List<UserPermissionClass> Permissions { get; set; } = new List<UserPermissionClass>();

    //    public string GetUserId { get; } = "aaa";
    //} // class...
}
