using AchievCor.Server.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AchievCor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {

        [HttpPost("authenticate")]
        public ActionResult<AuthenticatedUser> GetAuthentification(AuthorizationData authorization)
        {
            //AuthenticatedUser authenticatedUser = _authenticateService.GetAuthenticatedUser(authorization.Username, authorization.HashPass);

            //if (!authenticatedUser.IsUserValid)
            //{
            //    return Unauthorized(authenticatedUser?.ErrorMessage ?? "Token receipt error");
            //}
            //TODO: Заглушка. Обязательно привязать авторизацию!!!!
            AuthenticatedUser authenticatedUser = new() {
                 Token = "token",
                 User = new UserDto() { Login = "Tester" },
                 TokenExpiryDate = DateTime.UtcNow.AddDays(1),
            };

            if (string.IsNullOrWhiteSpace(authenticatedUser.Token))
                return Unauthorized("Token receipt error");

            HttpContext.Response.Cookies.Append(".AspCore.Refresh.Token", authenticatedUser.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    MaxAge = authenticatedUser.TokenExpiryDate - DateTime.UtcNow
                });
            return Ok(authenticatedUser);
        }

        [Authorize]
        [HttpGet]
        [Route("validate")]
        public IActionResult Validate()
        {
            var userName = User.Identity?.Name;
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            return Ok(new
            {
                message = "Token is valid",
                user = userName ?? "Unknown",
                id = userId
            });
        }
    }
}
