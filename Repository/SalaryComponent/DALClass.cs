using Domain.Common;
using Domain.SalaryComponent;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.SalaryComponent
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
                var components = await context.SalaryComponents.Select(m => new SalaryComponentResponse()
                {
                    ComponentId = m.ComponentId,
                    CompanyId = m.CompanyId,
                    ComponentCode = m.ComponentCode,
                    ComponentName = m.ComponentName,
                    ComponentType = m.ComponentType,
                    CalculationType = m.CalculationType,
                    Taxable = m.Taxable,
                    IsActive = m.IsActive
                }).ToListAsync();

                return components;
            } // GetSalaryComponentResponses...

            public async Task<SalaryComponentResponse> GetSalaryComponentByComponentId(int componentId)
            {
                var errors = new Dictionary<string, string[]>
                {
                    {GlobalConstantsClass.PageNotFoundKey, new[] {GlobalConstantsClass.PageNotFoundError} }
                };

                var components = await context.SalaryComponents.Select(m => new SalaryComponentResponse()
                {
                    ComponentId = m.ComponentId,
                    CompanyId = m.CompanyId,
                    ComponentCode = m.ComponentCode,
                    ComponentName = m.ComponentName,
                    ComponentType = m.ComponentType,
                    CalculationType = m.CalculationType,
                    Taxable = m.Taxable,
                    IsActive = m.IsActive
                }).FirstOrDefaultAsync(m => m.ComponentId == componentId);

                return components ?? throw new NotFoundException(errors);
            } // GetSalaryComponentByComponentId...

            private async Task ErrorHandling(SalaryComponentResponse salary)
            {
                var badRequestErrorsDictionary = new Dictionary<string, string[]>();
                List<string> errors = new List<string>();

                if (string.IsNullOrWhiteSpace(salary.ComponentCode))
                {
                    errors.Add("Component Code cannot be blank");
                }

                if (string.IsNullOrWhiteSpace(salary.ComponentName))
                {
                    errors.Add("Component Name cannot be blank.");
                }

                if(salary.ComponentId == 0 && context.SalaryComponents.Any(m => m.ComponentCode == salary.ComponentCode && m.CompanyId == salary.CompanyId))
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

                if(salary.ComponentType != "EARNING" && salary.ComponentType != "DEDUCTION" && salary.ComponentType != "TAX" && 
                            salary.ComponentType != "EMPLOYER_CONTRIBUTION")
                {
                    errors.Add("Invalid component type. Please select a valid component type");
                }

                if (salary.CalculationType != "FIXED" && salary.CalculationType != "PERCENTAGE" && salary.CalculationType != "FORMULA" &&
                            salary.ComponentType != "ATTENDANCE_BASED")
                {
                    errors.Add("Invalid calculation type. Please select a valid calculation type");
                }

                if(!context.Companies.Any(x => x.CompanyId == salary.CompanyId))
                {
                    errors.Add("Invalid \'Company\' specified.");
                }

                if (errors.Any())
                {
                    badRequestErrorsDictionary.Add(GlobalConstantsClass.BadRequestKey, errors.ToArray());
                    throw new BadRequestException(badRequestErrorsDictionary);
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
                    if(response.ComponentId == 0)
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
