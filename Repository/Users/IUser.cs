using Domain.Users;
using Infrastructure.Models;
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
        Task UpdateRefreshTokenInDatabase(string username, string token, DateTime expiresAt, int companyId, string dateFrom, string dateTo);
        Task Refresh(string username, int companyId, string dateFrom, string dateTo, IOptions<JWTOptionsClass> options);
        Task<UserCompanyProfileClass> GetUserProfileAfterLogin();
        Task<bool> IsLoggedIn();
        Task<List<UserClaims>> GetUserClaims(string username);
        Task LogOut();
    } // interface IUser...
}
