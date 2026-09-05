using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repository.Users;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;

namespace Payroll.Controllers
{
    [Route("user")]
    [ApiController]

    public class UserApiController : ControllerBase
    {
        private readonly IUser iuser;
        private readonly IOptions<JWTOptionsClass> options;

        public UserApiController(IUser iuser, IOptions<JWTOptionsClass> options)
        {
            this.iuser = iuser;
            this.options = options;
        } // constructor...

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(UserCreationClass user)
        {
            await iuser.CreateUser(user);
            return Created();
        } // CreateUser...

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser(UserAuthentication user)
        {
            bool b = await iuser.AuthenticateUser(user);
            return Ok(new {Message = b});
        } // AuthenticateUser...

        [HttpPost("CreateJWTWithCompanyFinYearSelection")]
        public async Task<IActionResult> CreateJWTWithCompanyFinYearSelection(string username, int companyId, string dateFrom, string dateTo)
        {
            string access_token = await iuser.CreateJWTWithCompanyFinYearSelection(username, companyId, dateFrom, dateTo, options);
            string refresh_token = await iuser.CreateRefreshToken(options);
            await iuser.UpdateRefreshTokenInDatabase(username, refresh_token, DateTime.UtcNow.AddDays(options.Value.RefreshTokenExpirationDays),
                        companyId, dateFrom, dateTo);

            return Ok(new { Message = "Success" });
        } // CreateJWTWithCompanyFinYearSelection...

        [HttpPost("Refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(string username, int companyId, string dateFrom, string dateTo, IOptions<JWTOptionsClass> options)
        {
            await iuser.Refresh(username, companyId, dateFrom, dateTo, options);
            return Ok(new { Message = "Success" });
        } // Refresh...

        [HttpGet("UserProfile")]
        [Authorize]
        public async Task<IActionResult> GetUserProfileAfterLogin()
        {
            var userCompanyProfile = await iuser.GetUserProfileAfterLogin();
            return Ok(userCompanyProfile);
        } // GetUserProfileAfterLogin...

        [HttpGet("IsLoggdIn")]
        [Authorize]
        public async Task<IActionResult> IsLoggedIn()
        {
            return Ok(iuser.IsLoggedIn());
        } // IsLoggedIn...

        [HttpGet("GetUserClaims/{username}")]
        [Authorize]
        public async Task<IActionResult> GetUserClaims(string username)
        {
            var userClaims = await iuser.GetUserClaims(username);
            return Ok(userClaims);
        } // GetUserClaims...

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await iuser.LogOut();
            return Ok();
        } // LogOut...
    } // class...
}
