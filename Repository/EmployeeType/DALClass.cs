using Domain.EmployeeType;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.EmployeeType
{
    namespace NEmployeeType
    {
        internal class DALClass : IEmployeeType
        {
            private readonly PayrollContext context;

            public DALClass(PayrollContext context)
            {
                this.context = context;
            } // constructor...

            public async Task<List<EmployeeTypeResponse>> GetEmployeeTypes()
            {
                var employeeTypes = await context.EmployeeTypes.Select(m => new EmployeeTypeResponse()
                {
                    TypeId = m.TypeId,
                    TypeName = m.TypeName
                }).ToListAsync();

                return employeeTypes;
            } // EmployeeTypeResponse...

            public async Task<EmployeeTypeResponse> GetEmployeeTypeByTypeId(int typeId)
            {
                var employeeType = await context.EmployeeTypes.Select(x => new EmployeeTypeResponse()
                {
                    TypeId = x.TypeId,
                    TypeName = x.TypeName
                }).FirstOrDefaultAsync(m => m.TypeId == typeId) ?? new EmployeeTypeResponse();

                return employeeType;
            } // GetEmployeeTypeByTypeId...

            public async Task<string> Save(EmployeeTypeResponse response)
            {
                var trans = await context.Database.BeginTransactionAsync();
                string message = string.Empty;

                try
                {
                    if (response.TypeId == 0)
                    {
                        if (context.EmployeeTypes.Any(m => m.TypeName.ToUpper() == response.TypeName.ToUpper()))
                        {
                            message = $"Duplicate EMployee Type found - {response.TypeName}";
                            return message;
                        }

                        Infrastructure.Models.EmployeeType et = new Infrastructure.Models.EmployeeType();
                        et.TypeName = response.TypeName;
                        await context.EmployeeTypes.AddAsync(et);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        if (context.EmployeeTypes.Any(x => x.TypeName.ToUpper() == response.TypeName.ToUpper() && x.TypeId != response.TypeId))
                        {
                            message = $"Duplicate Employee Type found - {response.TypeName}";
                            return message;
                        }

                        var existingEmployeeType = await context.EmployeeTypes.FirstOrDefaultAsync(x => x.TypeId == response.TypeId);
                        existingEmployeeType!.TypeName = response.TypeName;
                        context.Update(existingEmployeeType);
                        await context.SaveChangesAsync();
                    } // end if...

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
    } // namespace NEmployeeType...  
}
