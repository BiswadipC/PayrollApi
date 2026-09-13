using Application.Repository.Designation;
using Domain.Common;
using Domain.Designation;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.Designation
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
                var designation = await context.Designations.Select(m => new DesignationResponse(m.IdNo, m.Name)).ToListAsync();
                return designation;
            } // GetDesignations...

            public async Task<DesignationResponse> GetDesignationById(int id)
            {
                var designation = await context.Designations.FirstOrDefaultAsync(m => m.IdNo == id);
                var designationResponse = new DesignationResponse(designation!.IdNo, designation.Name);
                return designationResponse;
            } // GetDesignationById...

            private async Task ErrorHandling(DesignationResponse response)
            {
                if(response.IdNo == 0 && context.Designations.Any(m => m.Name.ToLower() == response.Name.ToLower()))
                {
                    throw new BadRequestClass(new Dictionary<string, string[]>()
                    {
                        {GlobalConstantClass.BadRequestKey, new[] {$"Duplication designation name found - {response.Name}"} }
                    });
                }

                if(response.IdNo > 0 && context.Designations.Any(m => m.Name.ToLower() == response.Name.ToLower() && m.IdNo != response.IdNo))
                {
                    throw new BadRequestClass(new Dictionary<string, string[]>()
                    {
                        {GlobalConstantClass.BadRequestKey, new []{$"Duplication designation name found - {response.Name}" } }
                    });
                }                
            } // ErrorHandling...

            public async Task CreateDesignation(DesignationResponse response)
            {
                await ErrorHandling(response);

                Infrastructure.Models.Designation designation = new Infrastructure.Models.Designation();
                designation.Name = response.Name;

                await context.Designations.AddAsync(designation);
                await context.SaveChangesAsync();
            } // CreateDesignation...

            public async Task UpdateDesignation(DesignationResponse response)
            {
                await ErrorHandling(response);

                var existingDesignation = await context.Designations.FirstOrDefaultAsync(m => m.IdNo == response.IdNo);
                if(existingDesignation != null)
                {
                    existingDesignation.Name = response.Name;
                    context.Update(existingDesignation);
                    await context.SaveChangesAsync();
                }
            } // CreateDesignation...

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
