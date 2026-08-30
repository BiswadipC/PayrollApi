using Domain.Users;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Users
{
    public interface IUser
    {
        Task CreateUser(UserCreationClass user);
        Task<bool> AuthenticateUser(UserAuthentication user);
        Task<string> CreateJWTWithCompanyFinYearSelection(string username, int companyId, string dateFrom, string dateTo, IOptions<JWTOptionsClass> options);
        Task<UserResponse> GetUserByUserName(string username);
        Task<string> CreateRefreshToken(IOptions<JWTOptionsClass> options);
        Task UpdateRefreshTokenInDatabase(string username, string token, DateTime expiresAt);
        Task Refresh(string username, int companyId, string dateFrom, string dateTo, IOptions<JWTOptionsClass> options);
        Task LogOut();
    } // interface IUser...
}
