using Domain.CompanyAndFinYear;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.CompanyAndFinYear
{
    namespace NCompanyAndFinYear
    {
        internal sealed class DALClass : ICompanyAndFinYear
        {
            private readonly PayrollContext context;

            public DALClass(PayrollContext context)
            {
                this.context = context;
            } // constructor...

            public async Task<List<CompanyResponse>> GetCompanies()
            {
                var companies = await context.Companies.Select(x => new CompanyResponse()
                {
                    CompanyId = x.CompanyId,
                    CompanyCode = x.CompanyCode,
                    CompanyName = x.CompanyName,
                    GSTIN = x.Gstin!,
                    RegistrationNo = x.RegistrationNo!,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    City = x.City,
                    State = x.State,
                    Country = x.Country,
                    Pin = x.Pin!
                }).ToListAsync();

                return companies;
            } // GetCompanies...

            public async Task<List<FinYearResponse>> GetFinYears()
            {
                var years = await context.FinYears.Select(x => new FinYearResponse()
                {
                    YearId = x.YaarId,
                    FromDate = x.FromDate.ToString("dd/MM/yyyy"),
                    ToDate = x.ToDate.ToString("dd/MM/yyyy")
                }).ToListAsync();

                return years;
            } // GetFinYears...
        } // class...
    } // namespace NCompanyAndFinYear...
}
