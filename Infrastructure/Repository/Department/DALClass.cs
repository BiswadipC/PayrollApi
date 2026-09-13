using Application.Repository.Department;
using Domain.Common;
using Domain.Department;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.Department
{
    namespace NDepartment
    {
        internal sealed class DALClass : IDepartment
        {
            private readonly PayrollContext context;

            public DALClass(PayrollContext context)
            {
                this.context = context;
            } // constructor...

            public async Task<List<DepartmentResponse>> GetDepartments()
            {
                var departments = await context.Departments.Select(x => new DepartmentResponse(x.IdNo, x.Name!)).ToListAsync();
                return departments;                
            } // GetDepartments...

            public async Task<DepartmentResponse> GetDepartmentById(int id)
            {
                var department = await context.Departments.FirstOrDefaultAsync(x => x.IdNo == id);
                var departmentResponse = new DepartmentResponse(department!.IdNo, department.Name ?? string.Empty);
                return departmentResponse;
            } // GetDepartmentById...

            private async Task ErrorHandling(DepartmentResponse response)
            {
                if(response.IdNo == 0 && context.Departments.Any(x => x.Name!.ToLower() == response.Name.ToLower()))
                {
                    throw new BadRequestClass(new Dictionary<string, string[]>()
                    {
                        {GlobalConstantClass.BadRequestKey, new [] {$"Duplicate Department Name found - {response.Name}"} }
                    });
                }

                if (response.IdNo > 0 && context.Departments.Any(x => x.Name!.ToLower() == response.Name.ToLower() && x.IdNo != response.IdNo))
                {
                    throw new BadRequestClass(new Dictionary<string, string[]>()
                    {
                        {GlobalConstantClass.BadRequestKey, new [] {$"Duplicate Department Name found - {response.Name}"} }
                    });
                }
            } // ErrorHandling...

            private async Task CreateDepartment(DepartmentResponse response)
            {
                await ErrorHandling(response);

                Infrastructure.Models.Department department = new Models.Department();
                department.Name = response.Name;
                await context.Departments.AddAsync(department);
                await context.SaveChangesAsync();
            }

            private async Task UpdateDepartment(DepartmentResponse response)
            {
                await ErrorHandling(response);

                var existingDepartment = await context.Departments.FirstOrDefaultAsync(x => x.IdNo == response.IdNo);
                if(existingDepartment != null)
                {
                    existingDepartment.Name = response.Name;
                    context.Update(existingDepartment);
                    await context.SaveChangesAsync();
                }                
            }

            public async Task Save(DepartmentResponse response)
            {
                var trans = await context.Database.BeginTransactionAsync();

                try
                {
                    if (response.IdNo == 0) 
                    {
                        await CreateDepartment(response);
                    }
                    else
                    {
                        await UpdateDepartment(response);
                    }

                    await trans.CommitAsync();
                }
                catch(Exception ex)
                {
                    await trans.RollbackAsync();
                    trans.Dispose();
                    throw;
                }
                finally
                {
                    trans.Dispose();
                }
            } // Save...
        } // class...
    } // namespace NDepartment...
}
