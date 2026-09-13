using Domain.Users;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Repository.BankAndBranches;
using Repository.CompanyAndFinYear;
using Repository.Department;
using Repository.Designation;
using Repository.EmployeesManagement;
using Repository.SalaryComponent;
using Repository.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Repository.Common
{
    public static class DependencyInjectionClass
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PayrollContext>(options => options.UseSqlite(configuration.GetConnectionString("SqlConnection")));
            services.AddScoped<IDbConnection>(db => new SqlConnection(configuration.GetConnectionString("SqlConnection")));

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
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DESIGNATION-View", policy =>
                {
                    policy.RequireClaim("DESIGNATION-View", "View");
                });
                options.AddPolicy("DESIGNATION-Edit", policy =>
                {
                    policy.RequireClaim("DESIGNATION-Edit", "Edit");
                });

                options.AddPolicy("BANK-View", policy =>
                {
                    policy.RequireClaim("BANK-View", "View");
                });
                options.AddPolicy("BANK-Edit", policy =>
                {
                    policy.RequireClaim("BANK-Edit", "Edit");
                });

                options.AddPolicy("SALARY COMPONENT-View", policy =>
                {
                    policy.RequireClaim("SALARY COMPONENT-View", "View");
                });
                options.AddPolicy("SALARY COMPONENT-Edit", policy =>
                {
                    policy.RequireClaim("SALARY COMPONENT-Edit", "Edit");
                });
            });

            services.AddScoped<IDesignation, Designation.NDesignation.DALClass>();
            services.AddScoped<IDepartment, Department.NDepartment.DALClass>();
            services.AddScoped<IBankAndBranches, BankAndBranches.NBankAndBranches.DALClass>();
            services.AddScoped<ISalaryComponent, SalaryComponent.NSalaryComponent.DALClass>();            
            services.AddScoped<IUser, Users.NUsers.DALClass>();
            services.Configure<JWTOptionsClass>(configuration.GetSection("JWT"));

            services.AddScoped<ICompanyAndFinYear, CompanyAndFinYear.NCompanyAndFinYear.DALClass>();
            services.AddScoped<IEmployeeManagement, EmployeesManagement.NEmployeesManagement.DALClass>();
        } // AddDependencies...
    } // class...
}
