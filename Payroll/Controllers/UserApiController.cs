using Application.DTO.User;
using Application.Repository.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Payroll.Controllers
{
    [Route("user")]
    [ApiController]

    public class UserApiController : ControllerBase
    {
        private readonly UserService userService;

        public UserApiController(UserService userService)
        {
            this.userService = userService;
        } // constructor...

        [HttpPost("create-new")]
        public async Task<IActionResult> CreateNewUser(CreateUserDTO dto)
        {
            await userService.CreateUser(dto);
            return Ok(new { Message = "Success" });
        } // CreateNewUser...

        [HttpGet("get-user/{username:alpha}")]
        public async Task<IActionResult> GetUserByUserName(string username)
        {
            var user = await userService.GetUserByUserName(username);
            return Ok(user);
        } // GetUserByUserName...
    } // class...
}
