using Application.Repository.Authentication;
using Application.Repository.Bank;
using Application.Repository.Company;
using Application.Repository.Department;
using Application.Repository.Designation;
using Application.Repository.SalaryComponent;
using Application.Repository.User;
using Domain.Common;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Common
{
    public static class DependencyInjection
    {
        public static void AddDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                var jwtOptionsClass = configuration.GetSection("JWT").Get<JWTOptionsClass?>();

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptionsClass!.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptionsClass.Audiance,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptionsClass.SecurityKey)),
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

                options.AddPolicy("DEPARTMENT-View", policy =>
                {
                    policy.RequireClaim("DEPARTMENT-View", "View");
                });
                options.AddPolicy("DEPARTMENT-Edit", policy =>
                {
                    policy.RequireClaim("DEPARTMENT-Edit", "Edit");
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

            services.AddDbContext<PayrollContext>(options => options.UseSqlite(configuration.GetConnectionString("SqlConnection")));
            services.AddScoped<IDbConnection>(db => new SqlConnection(configuration.GetConnectionString("SqlConnection")));
            services.AddAutoMapper(x => x.AddMaps(typeof(Application.Common.MapProfile).Assembly));
            
            services.AddScoped<IDesignation, Infrastructure.Repository.Designation.NDesignation.DALClass>();
            services.AddScoped<DesignationService>();
            services.AddScoped<IDepartment, Infrastructure.Repository.Department.NDepartment.DALClass>();
            services.AddScoped<DepartmentService>();
            services.AddScoped<IBank, Infrastructure.Repository.Bank.NBank.DALClass>();
            services.AddScoped<BankService>();
            services.AddScoped<ISalaryComponent, Infrastructure.Repository.SalaryComponent.NSalaryComponent.DALClass>();
            services.AddScoped<Application.Repository.SalaryComponent.SalaryComponentService>();
            services.AddScoped<IUser, Infrastructure.Repository.User.NUser.DALClass>();
            services.AddScoped<Application.Repository.User.UserService>();
            services.AddScoped<IAuthentication, Infrastructure.Authentication.NAuthentication.DALClass>();
            services.AddScoped<Application.Repository.Authentication.AuthenticationService>();
            services.AddScoped<ICompany, Infrastructure.Repository.Company.NCompany.DALClass>();
            services.AddScoped<CompanyService>();
            services.Configure<JWTOptionsClass>(configuration.GetSection("JWT"));
        } // AddDependency...
    } // class...
}
