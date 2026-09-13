using Domain.CompanyAndFinYear;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.CompanyAndFinYear
{
    public interface ICompanyAndFinYear
    {
        Task<List<CompanyResponse>> GetCompanies();
        Task<List<FinYearResponse>> GetFinYears();
    } // interface...
}
