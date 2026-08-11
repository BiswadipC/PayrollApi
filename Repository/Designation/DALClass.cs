using Domain.Designation;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Designation
{
    namespace NDesignation
    {
        internal class DALClass : IDesignation
        {
            private readonly PayrollContext context;

            public DALClass(PayrollContext context)
            {
                this.context = context;
            } // constructor...

            public async Task<List<DesignationResponse>> GetDesignations()
            {
                var designations = await context.Designations.Select(m => new DesignationResponse()
                {
                    IdNo = m.IdNo,
                    Name = m.Name
                }).ToListAsync();

                return designations ?? new List<DesignationResponse>();
            } // GetDesignations...

            public async Task<DesignationResponse> GetDesignationById(int id)
            {
                var designation = await context.Designations.Select(m => new DesignationResponse()
                {
                    IdNo = m.IdNo,
                    Name = m.Name
                }).FirstOrDefaultAsync(x => x.IdNo == id);

                return designation ?? new DesignationResponse();
            } // GetDesignationById...

            public async Task<string> Save(DesignationResponse response)
            {
                string message = string.Empty;

                try
                {
                    Infrastructure.Models.Designation d = new Infrastructure.Models.Designation();

                    if (response.IdNo == 0)
                    {
                        if(context.Designations.Any(m => m.Name == response.Name))
                        {
                            message = $"Duplicate Designation found - {response.Name}";
                            return message;
                        }

                        d.Name = response.Name;
                        await context.AddAsync(d);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        if(context.Designations.Any(m => m.Name == response.Name && m.IdNo != response.IdNo))
                        {
                            message = $"Duplicate Designation found - {response.Name}";
                            return message;
                        }

                        var designation = await context.Designations.FirstOrDefaultAsync(m => m.IdNo == response.IdNo);
                        designation!.Name = response.Name;
                        context.Update(designation);
                        await context.SaveChangesAsync();
                    } // end if...

                    return "Success";
                }
                catch(Exception e)
                {
                    return e.ToString();
                }                
            } // Save...
        } // class...
    } // namespace NDesignation...
}
