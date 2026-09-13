using Application.DTO.User;
using Domain.Common;
using Domain.Company;
using Domain.User;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Authentication
{
    public interface IAuthentication
    {
        Task<bool> AuthenticateUser(AuthenticateUserResponse response);
        Task GenerateJWT(CompanyResponse companyResponse, string username, IOptions<JWTOptionsClass> options);
        Task<string> GenerateRefreshToken(IOptions<JWTOptionsClass> options);
        Task UpdateRefreshTokenInDatabase(string username, string refreshToken, CompanyResponse company, IOptions<JWTOptionsClass> options);
        Task Refresh(string username, CompanyResponse company, IOptions<JWTOptionsClass> options);
        Task<List<UserClaimsResponse>> GetUserClaims(string username);
    } // interface...
}
