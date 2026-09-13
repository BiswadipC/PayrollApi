using Application.Repository.SalaryComponent;
using Domain.Common;
using Domain.SalaryComponent;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.SalaryComponent
{
    namespace NSalaryComponent
    {
        internal sealed class DALClass : ISalaryComponent
        {
            private readonly PayrollContext context;

            public DALClass(PayrollContext context)
            {
                this.context = context;
            } // constructor...

            public async Task<List<SalaryComponentResponse>> GetSalaryComponents()
            {
                var components = await context.SalaryComponents.Select(m => new SalaryComponentResponse(m.ComponentId, m.CompanyId,
                    m.ComponentCode, m.ComponentName, m.ComponentType, m.CalculationType, m.Taxable, m.IsActive)).ToListAsync();

                return components;
            } // GetSalaryComponentResponses...

            public async Task<SalaryComponentResponse> getSalaryComponentByComponentId(int componentId)
            {
                var errors = new Dictionary<string, string[]>
                {
                    {GlobalConstantClass.PageNotFoundKey, new[] {GlobalConstantClass.PageNotFoundError} }
                };

                var component = await context.SalaryComponents.FirstOrDefaultAsync(m => m.ComponentId == componentId);

                return new SalaryComponentResponse(component!.ComponentId,component.CompanyId, component.ComponentCode, component.ComponentName,
                    component.ComponentType, component.CalculationType, component.Taxable, component.IsActive) ?? throw new NotFoundClass(errors);
            } // GetSalaryComponentByComponentId...

            private async Task ErrorHandling(SalaryComponentResponse salary)
            {
                var badRequestErrorsDictionary = new Dictionary<string, string[]>();
                List<string> errors = new List<string>();

                if (salary.ComponentId == 0 && context.SalaryComponents.Any(m => m.ComponentCode == salary.ComponentCode && m.CompanyId == salary.CompanyId))
                {
                    errors.Add($"Duplicate component code found - {salary.ComponentCode}");
                }

                if (salary.ComponentId > 0 && context.SalaryComponents.Any(m => m.ComponentCode == salary.ComponentCode && m.CompanyId == salary.CompanyId &&
                        m.ComponentId != salary.ComponentId))
                {
                    errors.Add($"Duplicate component code found - {salary.ComponentCode}");
                }

                if (salary.ComponentId == 0 && context.SalaryComponents.Any(m => m.ComponentName == salary.ComponentName && m.CompanyId == salary.CompanyId))
                {
                    errors.Add($"Duplicate component name found - {salary.ComponentName}");
                }

                if (salary.ComponentId > 0 && context.SalaryComponents.Any(m => m.ComponentName == salary.ComponentName && m.CompanyId == salary.CompanyId &&
                        m.ComponentId != salary.ComponentId))
                {
                    errors.Add($"Duplicate component name found - {salary.ComponentName}");
                }

                if (!context.Companies.Any(x => x.CompanyId == salary.CompanyId))
                {
                    errors.Add("Invalid \'Company\' specified.");
                }

                if (errors.Any())
                {
                    badRequestErrorsDictionary.Add(GlobalConstantClass.BadRequestKey, errors.ToArray());
                    throw new BadRequestClass(badRequestErrorsDictionary);
                }
            } // ErrorHandling...

            private async Task CreateSalaryComponent(SalaryComponentResponse salary)
            {
                await ErrorHandling(salary);

                Infrastructure.Models.SalaryComponent component = new Infrastructure.Models.SalaryComponent();
                component.ComponentCode = salary.ComponentCode;
                component.ComponentName = salary.ComponentName;
                component.CalculationType = salary.CalculationType;
                component.ComponentType = salary.ComponentType;
                component.Taxable = salary.Taxable;
                component.IsActive = "Yes";
                component.CompanyId = salary.CompanyId;
                await context.SalaryComponents.AddAsync(component);
                await context.SaveChangesAsync();
            } // CreateSalaryComponent...

            private async Task UpdateSalaryComponent(SalaryComponentResponse salary)
            {
                await ErrorHandling(salary);

                var existingComponet = await context.SalaryComponents.FirstOrDefaultAsync(m => m.ComponentId == salary.ComponentId);

                existingComponet!.ComponentCode = salary.ComponentCode;
                existingComponet.ComponentName = salary.ComponentName;
                existingComponet.CalculationType = salary.CalculationType;
                existingComponet.ComponentType = salary.ComponentType;
                existingComponet.Taxable = salary.Taxable;
                context.SalaryComponents.Update(existingComponet);
                await context.SaveChangesAsync();
            } // CreateSalaryComponent...

            public async Task Save(SalaryComponentResponse response)
            {
                var trans = await context.Database.BeginTransactionAsync();

                try
                {
                    if (response.ComponentId == 0)
                    {
                        await CreateSalaryComponent(response);
                    }
                    else
                    {
                        await UpdateSalaryComponent(response);
                    } // end if...

                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    await trans.DisposeAsync();
                    throw;
                }
                finally
                {
                    await trans.DisposeAsync();
                }
            } // Save...
        } // class...
    } // namespace NSalaryComponent...
}
