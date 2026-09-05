using Azure.Core;
using Domain.Common;
using Domain.Users;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repository.Users
{
    namespace NUsers
    {
        internal sealed class  DALClass : IUser
        {
            private readonly PayrollContext context;
            private readonly IConfiguration configuration;
            private readonly IHttpContextAccessor httpContextAccessor;

            public DALClass(PayrollContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            {
                this.context = context;
                this.configuration = configuration;
                this.httpContextAccessor = httpContextAccessor;
            } // constructor...

            private async Task ErrorHandlingForCreateUsers(UserCreationClass user)
            {
                var errors = new List<string>();
                var badRequestDictionary = new Dictionary<string, string[]>()
                    ;
                if (string.IsNullOrWhiteSpace(user.UserName))
                {
                    errors.Add("Username cannot be blank.");
                }

                if(string.IsNullOrWhiteSpace(user.Password))
                {
                    errors.Add("Password cannot be blank.");
                }

                if (string.IsNullOrWhiteSpace(user.ReTypePassword))
                {
                    errors.Add("Re-Type Password cannot be blank.");
                }

                if(user.UserName.Length < 4)
                {
                    errors.Add("Username must be of minimum 4 characters length.");
                }

                if (user.Password.Length < 4)
                {
                    errors.Add("Password must be of minimum 4 characters length.");
                }

                if(!user.Password.Equals(user.ReTypePassword))
                {
                    errors.Add("Both Passwords must match.");
                }

                if(context.Users.Any(m => m.UserName.ToUpper() == user.UserName.ToUpper())) 
                {
                    errors.Add($"Username \'{user.UserName}\' already exists.");
                }

                if(errors.Any())
                {
                    badRequestDictionary.Add(GlobalConstantsClass.BadRequestKey, errors.ToArray());
                    throw new BadRequestException(badRequestDictionary);
                }
            } // ErrorHandling...

            public async Task CreateUser(UserCreationClass user)
            {
                await ErrorHandlingForCreateUsers(user);

                var trans = await context.Database.BeginTransactionAsync();
                string isAdmin = "No";

                try
                {
                    isAdmin = context.Users.Any() ? "No" : "Yes";

                    User u = new User();
                    u.UserName = user.UserName;
                    u.Password = user.Password;
                    u.IsAdmin = isAdmin;
                    await context.Users.AddAsync(u);
                    await context.SaveChangesAsync();

                    foreach(var module in context.Modules)
                    {
                        if(isAdmin == "Yes")
                        {
                            UserModulesPolicyMapping umpm1 = new UserModulesPolicyMapping();
                            umpm1.UserId = u.UserId;
                            umpm1.UserName = u.UserName;
                            umpm1.ModuleName = module.ModuleName;
                            umpm1.PolicyName = module.ModuleName + "-" + "View";
                            umpm1.PermissionType = "View";
                            await context.UserModulesPolicyMappings.AddAsync(umpm1);
                            await context.SaveChangesAsync();

                            UserModulesPolicyMapping umpm2 = new UserModulesPolicyMapping();
                            umpm2.UserId = u.UserId;
                            umpm2.UserName = u.UserName;
                            umpm2.ModuleName = module.ModuleName;
                            umpm2.PolicyName = module.ModuleName + "-" + "Edit";
                            umpm2.PermissionType = "Edit";
                            await context.UserModulesPolicyMappings.AddAsync(umpm2);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            UserModulesPolicyMapping umpm1 = new UserModulesPolicyMapping();
                            umpm1.UserId = u.UserId;
                            umpm1.UserName = u.UserName;
                            umpm1.ModuleName = module.ModuleName;
                            umpm1.PolicyName = module.ModuleName + "-" + "View";
                            umpm1.PermissionType = "None";
                            await context.UserModulesPolicyMappings.AddAsync(umpm1);
                            await context.SaveChangesAsync();

                            UserModulesPolicyMapping umpm2 = new UserModulesPolicyMapping();
                            umpm2.UserId = u.UserId;
                            umpm2.UserName = u.UserName;
                            umpm2.ModuleName = module.ModuleName;
                            umpm2.PolicyName = module.ModuleName + "-" + "Edit";
                            umpm2.PermissionType = "None";
                            await context.UserModulesPolicyMappings.AddAsync(umpm2);
                            await context.SaveChangesAsync();
                        } // end if...                        
                    } // end of foreach loop...

                    await trans.CommitAsync();
                    await trans.DisposeAsync();
                }
                catch(Exception e)
                {
                    await trans.RollbackAsync();
                    await trans.DisposeAsync();
                    throw;
                }                
            } // CreateUser...

            public async Task<bool> AuthenticateUser(UserAuthentication user)
            {
                var errors = new List<string>();
                var errorsDictionary = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(user.UserName))
                {
                    errors.Add("Username cannot be blank.");
                }

                if (string.IsNullOrWhiteSpace(user.Password))
                {
                    errors.Add("Password cannot be blank.");
                }

                if(!context.Users.Any(m => m.UserName == user.UserName && m.Password == user.Password))
                {
                    errors.Add("Invalid Username and or Password.");
                }

                if(errors.Any())
                {
                    errorsDictionary.Add(GlobalConstantsClass.UnAuthorizedKey, errors.ToArray());
                    throw new UnAuthorizedException(errorsDictionary);
                }
                else
                {
                    return true;
                }
            } // AuthenticateUser...

            public async Task<UserResponse> GetUserByUserName(string username)
            {
                var user = await context.Users
                    .Select(x => new UserResponse
                    {
                        UserId = x.UserId,
                        UserName = x.UserName,
                        IsAdmin = x.IsAdmin
                    })
                    .FirstOrDefaultAsync(m => m.UserName == username);

                return user ?? new UserResponse();
            } // GetUserByUserName...

            public async Task<string> CreateJWTWithCompanyFinYearSelection(string username, int companyId, string dateFrom, string dateTo, IOptions<JWTOptionsClass> options)
            {
                SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)),
                            SecurityAlgorithms.HmacSha256Signature);

                List<Claim> claims = new List<Claim>();
                var userResponse = await GetUserByUserName(username);

                claims.Add(new Claim("UserId", userResponse.UserId.ToString()));
                claims.Add(new Claim("UserName", username));
                claims.Add(new Claim("DateFrom", dateFrom));
                claims.Add(new Claim("DateTo", dateTo));
                claims.Add(new Claim("CompanyId", companyId.ToString()));
                claims.Add(new Claim("IsAdmin", userResponse.IsAdmin));

                foreach(var data in context.UserModulesPolicyMappings.Where(m => m.UserId == userResponse.UserId))
                {
                    claims.Add(new Claim(data.PolicyName, data.PermissionType));
                }
                var identity = new ClaimsIdentity(claims);

                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
                {
                    SigningCredentials = credentials,
                    Subject = identity,
                    Expires = DateTime.UtcNow.AddMinutes(options.Value.AccessTokenExpirationMinutes),
                    Issuer = options.Value.Issuer,
                    Audience = options.Value.Audience,
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

                return token;
            } // CreateJWTWithCompanyFinYearSelection...

            public async Task<string> CreateRefreshToken(IOptions<JWTOptionsClass> options)
            {
                string token = Guid.NewGuid().ToString();
                httpContextAccessor.HttpContext!.Response.Cookies.Append("RT", token, new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(options.Value.RefreshTokenExpirationDays)
                });

                return token;
            } // CreateRefreshToken...
            
            public async Task UpdateRefreshTokenInDatabase(string username, string token, DateTime expiresAt, int companyId, string dateFrom, string dateTo)
            {
                var trans = await context.Database.BeginTransactionAsync();

                try
                {
                    var rts = await context.RefreshTokens.Where(m => m.UserName == username).ToListAsync();
                    foreach (var r in rts)
                    {
                        context.Remove(r);
                    }
                    await context.SaveChangesAsync();

                    var userResponse = await GetUserByUserName(username);

                    RefreshToken rt = new RefreshToken();
                    rt.UserId = userResponse.UserId;
                    rt.Token = token;
                    rt.IsValid = "Yes";
                    rt.ExpiresAt = expiresAt;
                    rt.UserName = username;
                    rt.CompanyId = companyId;
                    rt.DateFrom = dateFrom;
                    rt.DateTo = dateTo;
                    rt.IsAdmin = userResponse.IsAdmin;
                    await context.RefreshTokens.AddAsync(rt);
                    await context.SaveChangesAsync();

                    await trans.CommitAsync();
                    await trans.DisposeAsync();

                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    await trans.DisposeAsync();
                    throw;
                }               
            } // UpdateRefreshTokenInDatabase...

            public async Task Refresh(string username, int companyId, string dateFrom, string dateTo, IOptions<JWTOptionsClass> options)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>();

                var refreshToken = httpContextAccessor.HttpContext!.Request.Cookies["RT"] ?? string.Empty;
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    errors.Add(GlobalConstantsClass.UnAuthorizedKey, new[] { GlobalConstantsClass.UnAuthorizedError });
                    throw new UnAuthorizedException(errors);
                }

                var RT = await context.RefreshTokens.Where(x => x.UserName == username && x.CompanyId == companyId && x.Token == refreshToken && x.IsValid == "Yes").FirstOrDefaultAsync();
                if (RT == null)
                {
                    errors.Add(GlobalConstantsClass.UnAuthorizedKey, new[] { GlobalConstantsClass.UnAuthorizedError });
                    throw new UnAuthorizedException(errors);
                }

                if (DateTime.UtcNow >= RT.ExpiresAt)
                {
                    RT.IsValid = "No";
                    context.RefreshTokens.Update(RT);
                    await context.SaveChangesAsync();

                    errors.Add(GlobalConstantsClass.UnAuthorizedKey, new[] { GlobalConstantsClass.UnAuthorizedError });
                    throw new UnAuthorizedException(errors);
                }

                string token = await CreateJWTWithCompanyFinYearSelection(username, companyId, dateFrom, dateTo, options);
            } // Refresh...
            
            public async Task<UserCompanyProfileClass> GetUserProfileAfterLogin()
            {
                int userId = Convert.ToInt32(httpContextAccessor.HttpContext!.User.FindFirst("UserId")!.Value);
                string username = httpContextAccessor.HttpContext.User.FindFirst("UserName")!.Value;
                string isAdmin = httpContextAccessor.HttpContext.User.FindFirst("IsAdmin")!.Value;
                int companyId = Convert.ToInt32(httpContextAccessor.HttpContext!.User.FindFirst("CompanyId")!.Value);
                string dateFrom = httpContextAccessor.HttpContext.User.FindFirst("DateFrom")!.Value;
                string dateTo = httpContextAccessor.HttpContext.User.FindFirst("DateTo")!.Value;

                var userProfile = new UserCompanyProfileClass()
                {
                    UserId = userId,
                    UserName = username,
                    IsAdmin = isAdmin,
                    companyId = companyId,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                };

                return userProfile;
            } // GetUserProfileAfterLogin...

            public async Task<List<UserClaims>> GetUserClaims(string username)
            {
                var userCliams = await context.UserModulesPolicyMappings.Where(x => x.UserName == username).Select(m => new UserClaims()
                {
                    PolicyName = m.PolicyName,
                    PermissionType = m.PermissionType
                }).ToListAsync();

                return userCliams;
            } // GetUserClaims...

            public async Task<bool> IsLoggedIn()
            {
                return httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated;
            } // IsLoggedIn...

            public async Task LogOut()
            {
                httpContextAccessor.HttpContext!.Response.Cookies.Delete("JWT", new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                });

                httpContextAccessor.HttpContext!.Response.Cookies.Delete("RT", new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                });
            } // LogOut...
        } // class...
    } // namespace NUsers...
}
