using Application.DTO.Company;
using Application.DTO.User;
using Application.Repository.User;
using AutoMapper;
using Domain.Common;
using Domain.Company;
using Domain.User;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Authentication
{
    public class AuthenticationService
    {
        private readonly IAuthentication iauth;
        private readonly IMapper mapper;

        public AuthenticationService(IAuthentication iauth, IMapper mapper)
        {
            this.iauth = iauth;
            this.mapper = mapper;
        } // constructor...

        public async Task AuthenticateUser(AuthenticateUserDTO dto)
        {
            var response = mapper.Map<AuthenticateUserResponse>(dto);
            bool b = await iauth.AuthenticateUser(response);
            if(!b)
            {
                throw new BadRequestClass(new Dictionary<string, string[]>
                {
                    {GlobalConstantClass.BadRequestKey, new [] {"Invalid credentials. Login denied."} }
                });
            }
        } // AuthenticateUser...

        public async Task GenerateJWT(CompanyDTO companyDTO, string username, IOptions<JWTOptionsClass> options)
        {
            var company = mapper.Map<CompanyResponse>(companyDTO);
            await iauth.GenerateJWT(company, username, options);
        } // GenerateJWT...

        public async Task<string> GenerateRefreshToken(IOptions<JWTOptionsClass> options)
        {
            return await iauth.GenerateRefreshToken(options);
        } // GenerateRefreshToken...

        public async Task UpdateRefreshTokenInDatabase(string username, string refreshToken, CompanyDTO company, IOptions<JWTOptionsClass> options)
        {
            var companyResponse = mapper.Map<CompanyResponse>(company);
            await iauth.UpdateRefreshTokenInDatabase(username, refreshToken, companyResponse, options);
        } // UpdateRefreshTokenInDatabase...

        public async Task Refresh(string username, CompanyDTO company, IOptions<JWTOptionsClass> options)
        {
            var companyResponse = mapper.Map<CompanyResponse>(company);
            await iauth.Refresh(username, companyResponse, options);
        } // Refresh...

        public async Task<List<UserClaimsDTO>> GetUserClaims(string username)
        {
            var userClaimsResponses = await iauth.GetUserClaims(username);
            var dtos = mapper.Map<List<UserClaimsDTO>>(userClaimsResponses);
            return dtos;
        } // GetUserClaims...
    } // class...
}
