using Application.Repository.User;
using Domain.Common;
using Domain.User;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.User
{
    namespace NUser
    {
        internal sealed class DALClass : IUser
        {
            private readonly PayrollContext context;

            public DALClass(PayrollContext context)
            {
                this.context = context;
            } // constructor...

            public async Task CreateUser(CreateUserResponse userResponse)
            {
                var trans = await context.Database.BeginTransactionAsync();

                try
                {
                    if(context.Users.Any(m => m.UserName.ToLower() == userResponse.UserName.ToLower()))
                    {
                        throw new BadRequestClass(new Dictionary<string, string[]>
                        {
                            {GlobalConstantClass.BadRequestKey, new[]{$"UserName - \'{userResponse.UserName}\' already exists."} }
                        });
                    }

                    string isAdmin = context.Users.Count() == 0 ? "Yes" : "No";

                    Infrastructure.Models.User user = new Models.User();
                    user.UserName = userResponse.UserName;
                    user.Password = userResponse.Password;
                    user.IsAdmin = isAdmin;
                    await context.Users.AddAsync(user);
                    await context.SaveChangesAsync();

                    foreach (var data in context.Modules)
                    {
                        Infrastructure.Models.UserModulesPolicyMapping mapping1 = new UserModulesPolicyMapping();
                        mapping1.UserId = user.UserId;
                        mapping1.UserName = userResponse.UserName;
                        mapping1.ModuleName = data.ModuleName;
                        mapping1.PolicyName = $"{data.ModuleName}-View";
                        mapping1.PermissionType = isAdmin == "Yes" ? "View" : "None";
                        await context.UserModulesPolicyMappings.AddAsync(mapping1);
                        await context.SaveChangesAsync();

                        Infrastructure.Models.UserModulesPolicyMapping mapping2 = new UserModulesPolicyMapping();
                        mapping2.UserId = user.UserId;
                        mapping2.UserName = userResponse.UserName;
                        mapping2.ModuleName = data.ModuleName;
                        mapping2.PolicyName = $"{data.ModuleName}-Edit";
                        mapping2.PermissionType = isAdmin == "Yes" ? "Edit" : "None";
                        await context.UserModulesPolicyMappings.AddAsync(mapping2);
                        await context.SaveChangesAsync();
                    }

                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    throw;
                }
                finally
                {
                    trans.Dispose();
                }
            } // CreateUser...

            public async Task<UserResponse> GetUserByUserName(string username)
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == username);
                return new UserResponse(user!.UserId, user.UserName, user.Password, user.IsAdmin);
            } // GetUserByUserName...
        } // class...
    } // namespace NUser...
}
