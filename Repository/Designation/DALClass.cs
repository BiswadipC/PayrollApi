using Domain.Common;
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
        internal sealed class DALClass : IDesignation
        {
            private readonly PayrollContext context;

            public DALClass(PayrollContext context)
            {
                this.context = context;
            } // constructor...

            public async Task<List<DesignationResponse>> GetDesignations()
            {
                var designations = await context.Designations.Select(m => new DesignationResponse
                {
                    IdNo = m.IdNo,
                    Name = m.Name
                }).ToListAsync();

                return designations;
            } // GetDesignations...

            public async Task<DesignationResponse> GetDesignationById(int id)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>
                {
                    {GlobalConstantsClass.PageNotFoundKey, new[] { GlobalConstantsClass.PageNotFoundError} }
                };
                
                var designation = await context.Designations.Select(x => new DesignationResponse
                {
                    IdNo = x.IdNo,
                    Name = x.Name
                }).FirstOrDefaultAsync(m => m.IdNo == id);

                return designation ?? throw new NotFoundException(errors);
            } // GetDesignationById...

            private async Task CreateDesignation(DesignationResponse designation)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(designation.Name))
                {
                    errors.Add(GlobalConstantsClass.BadRequestKey + "1", new[] { "Designation Name cannot be blank." });                    
                }

                if(context.Designations.Any(m => m.Name.ToUpper() == designation.Name.ToUpper()))
                {
                    errors.Add(GlobalConstantsClass.BadRequestKey + "2", new[] {$"Designation name \'{designation.Name}\' already exists."} );
                }

                if(errors.Any())
                {
                    throw new BadRequestException(errors);
                }

                Infrastructure.Models.Designation d = new Infrastructure.Models.Designation();
                d.Name = designation.Name;
                await context.Designations.AddAsync(d);
                await context.SaveChangesAsync();
            } // CreateDesignation...

            private async Task UpdateDesignation(DesignationResponse designation)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(designation.Name))
                {
                    errors.Add(GlobalConstantsClass.BadRequestKey + "1", new[] { "Designation Name cannot be blank." });
                }

                if (context.Designations.Any(m => m.Name.ToUpper() == designation.Name.ToUpper() && m.IdNo != designation.IdNo))
                {
                    errors.Add(GlobalConstantsClass.BadRequestKey + "2", new[] { $"Designation name \'{designation.Name}\' already exists." });
                }

                if (errors.Any())
                {
                    throw new BadRequestException(errors);
                }

                var existingDesignation = await context.Designations.FirstOrDefaultAsync(m => m.IdNo == designation.IdNo);
                existingDesignation!.Name = designation.Name;
                context.Designations.Update(existingDesignation);
                await context.SaveChangesAsync();
            } // UpdateDesignation...

            public async Task Save(DesignationResponse response)
            {
                var trans = await context.Database.BeginTransactionAsync();

                try
                {
                    if(response.IdNo == 0)
                    {
                        await CreateDesignation(response);
                    }
                    else
                    {
                        await UpdateDesignation(response);
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
    } // namespace NDesignation...
}   
