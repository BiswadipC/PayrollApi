using Application.DTO.Company;
using Application.DTO.User;
using Application.Repository.Authentication;
using Application.Repository.Company;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Payroll.Controllers
{
    [ApiController]

    public class AuthenticateApiController : ControllerBase
    {
        private readonly AuthenticationService authenticationService;
        private readonly IOptions<JWTOptionsClass> options;
        private readonly CompanyService companyService;

        public AuthenticateApiController(AuthenticationService authenticationService, IOptions<JWTOptionsClass> options, CompanyService companyService)
        {
            this.authenticationService = authenticationService;
            this.options = options;
            this.companyService = companyService;
        } // constructor...

        [HttpPost("authenticate-user")]
        public async Task<IActionResult> AuthenticateUser(AuthenticateUserDTO dto)
        {
            await authenticationService.AuthenticateUser(dto);
            return Ok(new {Message = true});
        } // AuthenticateUser...

        [HttpPost("GenerateJWTAfterAuthentication/{companyId:int}/{finYearId:int}/{username:alpha}")]
        public async Task<IActionResult> GenerateJWTAfterAuthentication(int companyId, int finYearId, string username)
        {
            var companyDTO = await companyService.GetCompanyFinYearByCompanyIdFinYearId(companyId, finYearId);
            await authenticationService.GenerateJWT(companyDTO, username, options);
            string refreshToken = await authenticationService.GenerateRefreshToken(options);
            await authenticationService.UpdateRefreshTokenInDatabase(username, refreshToken, companyDTO, options);

            return Ok(new {Message = "Success"});
        } // GenerateJWTAfterAuthentication...

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(string username, int companyId, int finYearId)
        {
            var companyDTO = await companyService.GetCompanyFinYearByCompanyIdFinYearId(companyId, finYearId);
            await authenticationService.Refresh(username, companyDTO, options);
            return Ok(new { Message = "Success" });
        } // Refresh...

        [HttpGet("IsLoggedIn")]
        [Authorize]
        public async Task<IActionResult> IsLoggedIn()
        {
            bool b = false;
            if (User != null && User.Identity!.IsAuthenticated)
            {
                b = true;
            }

            return Ok(b);
        } // IsLoggedIn...

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("JWT", new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            HttpContext.Response.Cookies.Delete("RT", new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            });

            return Ok();
        } // Logout...

        [HttpGet("GetUserClaims/{username:alpha}")]
        public async Task<IActionResult> GetUserClaims(string username)
        {
            var userClaimsDTOs = await authenticationService.GetUserClaims(username);
            return Ok(userClaimsDTOs);
        } // GetUserClaims...
    } // class...
}
