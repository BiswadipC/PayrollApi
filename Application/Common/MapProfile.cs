using Application.DTO.Bank;
using Application.DTO.Company;
using Application.DTO.EmployeesManagement;
using Application.DTO.SalaryComponent;
using Application.DTO.User;
using AutoMapper;
using Domain.Bank;
using Domain.Company;
using Domain.EmployeesManagement;
using Domain.SalaryComponent;
using Domain.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<BankResponse, BankDTO>();
            CreateMap<BankDTO, BankResponse>();
            
            CreateMap<BranchResponse, BranchDTO>();
            CreateMap<BranchDTO, BranchResponse>();

            CreateMap<SalaryComponentResponse, SalaryComponentDTO>();
            CreateMap<SalaryComponentDTO, SalaryComponentResponse>();

            CreateMap<UserResponse, UserDTO>();
            CreateMap<UserDTO, UserResponse>();
            CreateMap<UserClaimsResponse, UserClaimsDTO>();
            CreateMap<CreateUserDTO, CreateUserResponse>();
            CreateMap<AuthenticateUserDTO, AuthenticateUserResponse>();

            CreateMap<CompanyResponse, CompanyDTO>();
            CreateMap<CompanyDTO, CompanyResponse>();

            CreateMap<FinYearResponse, FinYearDTO>();
            CreateMap<FinYearDTO, FinYearResponse>();

            CreateMap<EmployeesMainResponse, EmployeesMainDTO>();
            CreateMap<EmployeesMainDTO, EmployeesMainResponse>();
            CreateMap<EmployeesBankResponse, EmployeesBankDTO>();
            CreateMap<EmployeesBankDTO, EmployeesBankResponse>();
            CreateMap<EmployeesSalaryStructuresResponse, EmployeesSalaryStructuresDTO>();
            CreateMap<EmployeesSalaryStructuresDTO, EmployeesSalaryStructuresResponse>();
            CreateMap<EmployeeSalaryComponentsResponse, EmployeeSalaryComponentsDTO>();
            CreateMap<EmployeeSalaryComponentsDTO, EmployeeSalaryComponentsResponse>();
        } // constructor...
    } // class...
}
