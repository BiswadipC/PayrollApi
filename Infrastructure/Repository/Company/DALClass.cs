using Application.Repository.Company;
using Domain.Company;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository.Company
{
    namespace NCompany
    {
        internal sealed class DALClass : ICompany
        {
            private readonly PayrollContext context;

            public DALClass(PayrollContext context)
            {
                this.context = context;
            } // constructor...

            public async Task<List<CompanyResponse>> GetAllCompanies()
            {
                var companies = await context.Companies.Select(m => new CompanyResponse(m.CompanyId, m.CompanyCode, m.CompanyName,
                        m.Gstin ?? string.Empty, m.RegistrationNo ?? string.Empty, m.Address1 ?? string.Empty, m.Address2 ?? string.Empty, m.City ?? string.Empty,
                        m.State ?? string.Empty, m.Country ?? string.Empty, m.Pin ?? string.Empty, m.CurrencyCode ?? string.Empty, new List<FinYearResponse>(), "GET")).ToListAsync();
                return companies;
            } // GetAllCompanies...

            public async Task<List<FinYearResponse>> GetFinYearsByCompanyId(int companyId)
            {
                var years = await context.FinYears.Where(x => x.CompanyId == companyId)
                            .Select(x => new FinYearResponse(x.YearId, x.FromDate.ToString("dd/MM/yyyy"), x.ToDate.ToString("dd/MM/yyyy"))).ToListAsync();
                return years;                                                            
            } // GetFinYearsByCompanyId...

            public async Task<CompanyResponse> GetCompanyFinYearByCompanyIdFinYearId(int companyId, int finYearId)
            {
                var company = await context.Companies.FirstOrDefaultAsync(x => x.CompanyId == companyId);
                var years = await context.FinYears.Where(m => m.YearId == finYearId)
                            .Select(x => new FinYearResponse(x.YearId, x.FromDate.ToString("dd/MM/yyyy"), x.ToDate.ToString("dd/MM/yyyy"))).ToListAsync();

                var companyResponse = new CompanyResponse(company!.CompanyId, company.CompanyCode, company.CompanyName, company.Gstin ?? string.Empty, 
                    company.RegistrationNo ?? string.Empty, company.Address1 ?? string.Empty, company.Address2 ?? string.Empty, company.City ?? string.Empty, 
                    company.State ?? string.Empty, company.Country ?? string.Empty, company.Pin ?? string.Empty, company.CurrencyCode ?? string.Empty, years, "GET");

                return companyResponse;
            } // GetCompanyFinYearByCompanyIdFinYearId...

            public async Task<CompanyResponse> GetCompanyByCompanyId(int companyId)
            {
                var years = await GetFinYearsByCompanyId(companyId);
                var company = await context.Companies.Where(m => m.CompanyId == companyId).Select(x => new
                {
                    CompanyId = x.CompanyId,
                    CompanyCode = x.CompanyCode,
                    CompanyName = x.CompanyName,
                    GSTIN = x.Gstin ?? string.Empty,
                    RegNo = x.RegistrationNo,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    City = x.City,
                    State = x.State,
                    Country = x.Country,
                    Pin = x.Pin,
                    CurrencyCode = x.CurrencyCode
                }).FirstOrDefaultAsync();

                var CompanyResponse = new CompanyResponse(company!.CompanyId, company!.CompanyCode, company!.CompanyName, company!.GSTIN, company!.RegNo ?? string.Empty, 
                    company!.Address1 ?? string.Empty, company!.Address2 ?? string.Empty, company!.City ?? string.Empty, company!.State ?? string.Empty, 
                    company!.Country ?? string.Empty, company!.Pin ?? string.Empty, company!.CurrencyCode ?? string.Empty, years, "GET");

                return CompanyResponse;
            } // GetCompanyByCompanyId...
        } // class...
    } // namespace NCompany..
}
