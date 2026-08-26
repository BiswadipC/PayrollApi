using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.BankAndBranches;
using Repository.Department;
using Repository.Designation;
using Repository.SalaryComponent;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Common
{
    public static class DependencyInjectionClass
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PayrollContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("SqlConnection")));

            services.AddScoped<IDesignation, Designation.NDesignation.DALClass>();
            services.AddScoped<IDepartment, Department.NDepartment.DALClass>();
            services.AddScoped<IBankAndBranches, BankAndBranches.NBankAndBranches.DALClass>();
            services.AddScoped<ISalaryComponent, SalaryComponent.NSalaryComponent.DALClass>();
        } // AddDependencies...
    } // class...
}
