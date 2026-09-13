using Domain.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.User
{
    public interface IUser
    {
        Task CreateUser(CreateUserResponse userResponse);
        Task<UserResponse> GetUserByUserName(string username);
    } // interface...
}
