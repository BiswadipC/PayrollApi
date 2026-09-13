using Application.Repository.Authentication;
using Application.Repository.Company;
using Application.Repository.User;
using Domain.Common;
using Domain.Company;
using Domain.User;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authentication
{
    namespace NAuthentication
    {
        internal sealed class DALClass : IAuthentication
        {
            private readonly PayrollContext context;
            private readonly IUser iuser;
            private readonly IHttpContextAccessor httpContextAccessor;

            public DALClass(PayrollContext context, IUser iuser, IHttpContextAccessor httpContextAccessor)
            {
                this.context = context;
                this.iuser = iuser;
                this.httpContextAccessor = httpContextAccessor;
            } // constructor...

            public async Task<bool> AuthenticateUser(AuthenticateUserResponse response)
            {
                return context.Users.Any(m => m.UserName == response.UserName && m.Password == response.Password);
            } // AuthenticateUser...

            public async Task GenerateJWT(CompanyResponse companyResponse, string username, IOptions<JWTOptionsClass> options)
            {
                SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecurityKey.ToString())),
                    SecurityAlgorithms.HmacSha256Signature);

                var user = await iuser.GetUserByUserName(username);

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("UserId", user.UserId.ToString()));
                claims.Add(new Claim("UserName", username));
                claims.Add(new Claim("CompanyId", companyResponse.CompanyId.ToString()));
                claims.Add(new Claim("DateFrom", companyResponse.Years.FirstOrDefault()!.FromDate));
                claims.Add(new Claim("DateTo", companyResponse.Years.FirstOrDefault()!.ToDate));
                claims.Add(new Claim("IsAdmin", user.IsAdmin));

                foreach (var data in context.UserModulesPolicyMappings.Where(m => m.UserId == user.UserId))
                {
                    claims.Add(new Claim(data.PolicyName, data.PermissionType));
                }

                ClaimsIdentity identity = new ClaimsIdentity(claims);

                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
                {
                    SigningCredentials = credentials,
                    Subject = identity,
                    Expires = DateTime.UtcNow.AddMinutes(options.Value.JWTTokenInMinutes),
                    Issuer = options.Value.Issuer,
                    Audience = options.Value.Audiance,
                    NotBefore = DateTime.UtcNow
                };

                JsonWebTokenHandler handler = new JsonWebTokenHandler();
                string token = handler.CreateToken(descriptor);

                httpContextAccessor.HttpContext!.Response.Cookies.Append("JWT", token, new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
            } // GenerateJWT...

            public async Task<string> GenerateRefreshToken(IOptions<JWTOptionsClass> options)
            {
                string token = Guid.NewGuid().ToString();
                httpContextAccessor.HttpContext!.Response.Cookies.Append("RT", token, new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(options.Value.RefreshTokenInDays)
                });

                return token;
            } // GenerateRefreshToken...

            public async Task UpdateRefreshTokenInDatabase(string username, string refreshToken, CompanyResponse company, IOptions<JWTOptionsClass> options)
            {
                var trans = await context.Database.BeginTransactionAsync();

                try
                {
                    var refreshTokensInDatabase = await context.RefreshTokens.Where(m => m.UserName == username && m.IsValid == "Yes").ToListAsync();
                    if (refreshTokensInDatabase.Any())
                    {
                        context.RemoveRange(refreshTokensInDatabase);
                        await context.SaveChangesAsync();
                    }

                    var userResponse = await iuser.GetUserByUserName(username);

                    RefreshToken rt = new RefreshToken();
                    rt.Token = refreshToken;
                    rt.UserId = userResponse.UserId;
                    rt.UserName = username;
                    rt.CompanyId = company.CompanyId;
                    rt.DateFrom = company.Years.FirstOrDefault()!.FromDate;
                    rt.DateTo = company.Years.FirstOrDefault()!.ToDate;
                    rt.IsAdmin = userResponse.IsAdmin;
                    rt.IsValid = "Yes";
                    rt.ExpiresAt = DateTime.UtcNow.AddDays(options.Value.RefreshTokenInDays);
                    await context.RefreshTokens.AddAsync(rt);
                    await context.SaveChangesAsync();

                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    throw;
                }
                finally
                {
                    trans?.Dispose();
                }
            } // UpdateRefreshTokenInDatabase...

            public async Task Refresh(string username, CompanyResponse company, IOptions<JWTOptionsClass> options)
            {
                string refreshToken = httpContextAccessor.HttpContext!.Request.Cookies["RT"] ?? string.Empty;
                if(string.IsNullOrWhiteSpace(refreshToken))
                {
                    throw new UnAuthorizedClass(new Dictionary<string, string[]>
                    {
                        {GlobalConstantClass.UnAuthorizedKey, new[]{GlobalConstantClass.UnAuthorizedError } }
                    });
                }

                var refreshTokenInDatabase = await context.RefreshTokens.FirstOrDefaultAsync(x => x.UserName == username && 
                        x.CompanyId == company.CompanyId && x.Token == refreshToken && x.IsValid == "Yes");
                if(refreshTokenInDatabase == null)
                {
                    throw new UnAuthorizedClass(new Dictionary<string, string[]>
                    {
                        {GlobalConstantClass.UnAuthorizedKey, new[]{GlobalConstantClass.UnAuthorizedError } }
                    });
                }

                if(refreshTokenInDatabase.ExpiresAt!.Value <= DateTime.UtcNow)
                {
                    refreshTokenInDatabase.IsValid = "No";
                    context.RefreshTokens.Update(refreshTokenInDatabase);
                    await context.SaveChangesAsync();

                    throw new UnAuthorizedClass(new Dictionary<string, string[]>
                    {
                        {GlobalConstantClass.UnAuthorizedKey, new[]{GlobalConstantClass.UnAuthorizedError } }
                    });
                }

                await GenerateJWT(company, username, options);
            } // Refresh...

            public async Task<List<UserClaimsResponse>> GetUserClaims(string username)
            {
                var claims = await context.UserModulesPolicyMappings.Where(x => x.UserName == username)
                                    .Select(m => new UserClaimsResponse(m.PolicyName, m.PermissionType)).ToListAsync();
                return claims;
            } // class...
        } // class...
    }
}
