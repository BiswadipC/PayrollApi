using Domain.Common;
using Domain.Department;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Department
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
                var departments = await context.Departments.Select(m => new DepartmentResponse()
                {
                    IdNo = m.IdNo,
                    Name = m.Name ?? string.Empty
                }).ToListAsync();

                return departments;
            } // GetDepartments...

            public async Task<DepartmentResponse> GetDepartmentById(int id)
            {
                var errors = new Dictionary<string, string[]>
                {
                    {GlobalConstantsClass.PageNotFoundKey, new[] {GlobalConstantsClass.PageNotFoundError } }
                };

                var department = await context.Departments.Select(m => new DepartmentResponse()
                {
                    IdNo = m.IdNo,
                    Name = m.Name ?? string.Empty
                }).FirstOrDefaultAsync(x => x.IdNo == id);

                return department ?? throw new NotFoundException(errors);
            } // GetDepartmentById...

            private async Task CreateDepartment(DepartmentResponse department)
            {
                var errors = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(department.Name))
                {
                    errors.Add(GlobalConstantsClass.BadRequestKey + "1", new[] { "Department Name cannot be blank." });                    
                }

                if(context.Departments.Any(m => m.Name!.ToUpper() == department.Name.ToUpper()))
                {
                    errors.Add(GlobalConstantsClass.BadRequestKey + "2", new[] { $"Department Name \'{department.Name}\' already exists." });
                }

                if (errors.Any())
                {
                    throw new BadRequestException(errors);
                }

                Infrastructure.Models.Department d = new Infrastructure.Models.Department();
                d.Name = department.Name;
                await context.Departments.AddAsync(d);
                await context.SaveChangesAsync();
            } // CreateDepartment...

            private async Task UpdateDepartment(DepartmentResponse department)
            {
                var errors = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(department.Name))
                {
                    errors.Add(GlobalConstantsClass.BadRequestKey + "1", new[] { "Department Name cannot be blank." });
                }

                if(context.Departments.Any(m => m.Name!.ToUpper() == department.Name.ToUpper() && m.IdNo != department.IdNo))
                {
                    errors.Add(GlobalConstantsClass.BadRequestKey + "2", new[] { "Department Name '{department.Name}' already exists." });
                }

                if(errors.Any())
                {
                    throw new BadRequestException(errors);
                }

                var existingDepartment = await context.Departments.FirstOrDefaultAsync(m => m.IdNo == department.IdNo);
                existingDepartment!.Name = department.Name;
                context.Departments.Update(existingDepartment);
                await context.SaveChangesAsync();
            } // UpdateDepartment...

            public async Task Save(DepartmentResponse response)
            {
                var trans = await context.Database.BeginTransactionAsync();

                try
                {
                    if(response.IdNo == 0)
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
