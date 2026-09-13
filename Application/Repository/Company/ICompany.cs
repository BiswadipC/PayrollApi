using Domain.Company;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Company
{
    public interface ICompany
    {
        Task<List<CompanyResponse>> GetAllCompanies();
        Task<List<FinYearResponse>> GetFinYearsByCompanyId(int companyId);
        Task<CompanyResponse> GetCompanyFinYearByCompanyIdFinYearId(int companyId, int finYearId);
    } // interface...
}
