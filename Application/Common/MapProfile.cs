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
            CreateMap<EmployeesMainDTO, EmployeesMainResponse>()
                .ForCtorParam("employeeId", opt => opt.MapFrom(src => src.EmployeeId))
                .ForCtorParam("companyId", opt => opt.MapFrom(src => src.CompanyId))
                .ForCtorParam("employeeCode", 
                    opt => opt.MapFrom(src => src.EmployeeCode))
                .ForCtorParam("employeeName",
                    opt => opt.MapFrom(src => src.EmployeeName))
                .ForCtorParam("dOB",
                    opt => opt.MapFrom(src => src.DOB))
                .ForCtorParam("gender",
                    opt => opt.MapFrom(src => src.Gender))
                .ForCtorParam("email",
                    opt => opt.MapFrom(src => src.Email))
                .ForCtorParam("phone",
                    opt => opt.MapFrom(src => src.Phone))
                .ForCtorParam("hireDate",
                    opt => opt.MapFrom(src => src.HireDate))
                .ForCtorParam("terminationDate",
                    opt => opt.MapFrom(src => src.TerminationDate))
                .ForCtorParam("employeeTypeId",
                    opt => opt.MapFrom(src => src.EmployeeTypeId))
                .ForCtorParam("employeeTypeName",
                    opt => opt.MapFrom(src => src.EmployeeTypeName))
                .ForCtorParam("departmentId",
                    opt => opt.MapFrom(src => src.DepartmentId))
                .ForCtorParam("departmentName",
                    opt => opt.MapFrom(src => src.DepartmentName))
                .ForCtorParam("designationId",
                    opt => opt.MapFrom(src => src.DesignationId))
                .ForCtorParam("designationName",
                    opt => opt.MapFrom(src => src.DesignationName))
                .ForCtorParam("managerId",
                    opt => opt.MapFrom(src => src.ManagerId))
                .ForCtorParam("managerName",
                    opt => opt.MapFrom(src => src.ManagerName))
                .ForCtorParam("addressLine1",
                    opt => opt.MapFrom(src => src.AddressLine1))
                .ForCtorParam("addressLine2",
                    opt => opt.MapFrom(src => src.AddressLine2))
                .ForCtorParam("city",
                    opt => opt.MapFrom(src => src.City))
                .ForCtorParam("state",
                    opt => opt.MapFrom(src => src.State))
                .ForCtorParam("country",
                    opt => opt.MapFrom(src => src.Country))
                .ForCtorParam("pin",
                    opt => opt.MapFrom(src => src.Pin))
                .ForCtorParam("listEmployeesBankResponse",
                    opt => opt.MapFrom(src => src.ListEmployeesBankDTO))
                .ForCtorParam("employeesSalaryStructures",
                    opt => opt.MapFrom(src => src.EmployeesSalaryStructures))
                .ForCtorParam("listEmployeeSalaryComponentsResponse",
                    opt => opt.MapFrom(src => src.ListEmployeeSalaryComponentsDTO));

            CreateMap<EmployeesBankResponse, EmployeesBankDTO>();
            CreateMap<EmployeesBankDTO, EmployeesBankResponse>();
            CreateMap<EmployeesSalaryStructuresResponse, EmployeesSalaryStructuresDTO>();
            CreateMap<EmployeesSalaryStructuresDTO, EmployeesSalaryStructuresResponse>();
            CreateMap<EmployeeSalaryComponentsResponse, EmployeeSalaryComponentsDTO>();
            CreateMap<EmployeeSalaryComponentsDTO, EmployeeSalaryComponentsResponse>();
        } // constructor...
    } // class...
}
