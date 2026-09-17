using Application.DTO.Company;
using AutoMapper;
using Domain.Company;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Company
{
    public class CompanyService
    {
        private readonly ICompany ic;
        private readonly IMapper mapper;

        public CompanyService(ICompany ic, IMapper mapper)
        {
            this.ic = ic;
            this.mapper = mapper;
        } // constructor...

        public async Task<List<CompanyDTO>> GetAllCompanies()
        {
            var companies = await ic.GetAllCompanies();
            var dtos = mapper.Map<List<CompanyDTO>>(companies);
            return dtos;
        } // GetAllCompanies...

        public async Task<List<FinYearDTO>> GetFinYearsByCompanyId(int companyId)
        {
            var years = await ic.GetFinYearsByCompanyId(companyId);
            var dtos = mapper.Map<List<FinYearDTO>>(years);
            return dtos;
        } // GetFinYearsByCompanyId...

        public async Task<CompanyDTO> GetCompanyFinYearByCompanyIdFinYearId(int companyId, int finYearId)
        {
            var company = await ic.GetCompanyFinYearByCompanyIdFinYearId(companyId, finYearId);
            var dto = mapper.Map<CompanyDTO>(company);
            return dto;
        } // GetCompanyFinYearByCompanyIdFinYearId...

        public async Task<CompanyDTO> GetCompanyByCompanyId(int companyId)
        {
            var companyResponse = await ic.GetCompanyByCompanyId(companyId);
            var dto = mapper.Map<CompanyDTO>(companyResponse);
            return dto;
        } // GetCompanyByCompanyId...
    } // class...
}
