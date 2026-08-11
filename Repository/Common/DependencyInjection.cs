using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Department;
using Repository.Designation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Common
{
    public static class DependencyInjection
    {
        public static void AddDependency(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddScoped<IDesignation, Repository.Designation.NDesignation.DALClass>();
            service.AddScoped<IDepartment, Repository.Department.DALClass>();
        } // AddDependency...
    } // DependencyInjection...
}
