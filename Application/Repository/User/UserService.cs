using Application.DTO.User;
using AutoMapper;
using Domain.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.User
{
    public class UserService
    {
        private readonly IUser iuser;
        private readonly IMapper mapper;

        public UserService(IUser iuser, IMapper mapper)
        {
            this.iuser = iuser;
            this.mapper = mapper;
        } // constructor...

        public async Task CreateUser(CreateUserDTO createUserDTO)
        {
            var userResponse = mapper.Map<CreateUserResponse>(createUserDTO);
            await iuser.CreateUser(userResponse);
        } // CreateUser...

        public async Task<UserDTO> GetUserByUserName(string username)
        {
            var user = await iuser.GetUserByUserName(username);
            var dto = mapper.Map<UserDTO>(user);
            return dto;
        } // GetUserByUserName...
    } // class...
}
