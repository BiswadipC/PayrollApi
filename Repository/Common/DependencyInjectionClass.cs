using Domain.Users;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Repository.BankAndBranches;
using Repository.CompanyAndFinYear;
using Repository.Department;
using Repository.Designation;
using Repository.SalaryComponent;
using Repository.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Common
{
    public static class DependencyInjectionClass
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PayrollContext>(options => options.UseSqlite(configuration.GetConnectionString("SqlConnection")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection("JWT").Get<JWTOptionsClass?>();

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions!.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions!.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies["JWT"] != null)
                        {
                            context.Token = context.Request.Cookies["JWT"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddAuthorization();

            services.AddScoped<IDesignation, Designation.NDesignation.DALClass>();
            services.AddScoped<IDepartment, Department.NDepartment.DALClass>();
            services.AddScoped<IBankAndBranches, BankAndBranches.NBankAndBranches.DALClass>();
            services.AddScoped<ISalaryComponent, SalaryComponent.NSalaryComponent.DALClass>();            
            services.AddScoped<IUser, Users.NUsers.DALClass>();
            services.Configure<JWTOptionsClass>(configuration.GetSection("JWT"));

            services.AddScoped<ICompanyAndFinYear, CompanyAndFinYear.NCompanyAndFinYear.DALClass>();
        } // AddDependencies...
    } // class...
}
