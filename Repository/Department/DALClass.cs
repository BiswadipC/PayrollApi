using Domain.Department;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Department
{
    internal class DALClass : IDepartment
    {
        private readonly PayrollContext context;

        public DALClass(PayrollContext context)
        {
            this.context = context;
        } // constructor...

        public async Task<List<DepartmentResponse>> GetDepartments()
        {
            var depatments = await context.Departments.Select(x => new DepartmentResponse()
            {
                IdNo = x.IdNo,
                Name = x.Name ?? string.Empty
            }).ToListAsync();

            return depatments;
        } // GetDepartments...

        public async Task<DepartmentResponse> GetDepartmentById(int id)
        {
            var department = await context.Departments.Select(x => new DepartmentResponse()
            {
                IdNo = x.IdNo,
                Name = x.Name ?? string.Empty
            }).FirstOrDefaultAsync(m =>  m.IdNo == id);

            return department ?? new DepartmentResponse();
        } // GetDepartmentById...

        public async Task<string> Save(DepartmentResponse response)
        {
            string message = string.Empty;
            var trans = await context.Database.BeginTransactionAsync();

            try
            {
                if(response.IdNo == 0)
                {
                    if(context.Departments.Any(x => x.Name!.ToUpper() == response.Name.ToUpper()))
                    {
                        message = $"Duplicate Department found - {response.Name}";
                        return message;
                    }

                    Infrastructure.Models.Department d = new Infrastructure.Models.Department();
                    d.Name = response.Name;
                    await context.AddAsync(d);
                    await context.SaveChangesAsync();
                }
                else
                {
                    if(context.Departments.Any(m => m.Name!.ToUpper() == response.Name.ToUpper() && m.IdNo != response.IdNo))
                    {
                        message = $"Duplicate Department found - {response.Name}";
                        return message;
                    }

                    var department = await context.Departments.FirstOrDefaultAsync(x => x.IdNo == response.IdNo);
                    department!.Name = response.Name;
                    context.Update(department);
                    await context.SaveChangesAsync();
                } // end of if...

                await trans.CommitAsync();
                message = "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                message = ex.ToString();
            }
            finally
            {
                trans.Dispose();
            }

            return message;
        } // Save...
    } // class...
}
