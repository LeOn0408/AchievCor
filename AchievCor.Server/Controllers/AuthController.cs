using AchievCor.Server.Data;
using AchievCor.Server.Dto;
using AchievCor.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AchievCor.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IdentityService _authenticateService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IdentityService authenticateService, ILogger<AuthController> logger)
    {
        _authenticateService = authenticateService;
        _logger = logger;
    }


    [HttpPost("authenticate")]
    public ActionResult<AuthenticatedUser> GetAuthentification(AuthorizationData authorization)
    {
        try
        {
            AuthenticatedUser authenticatedUser = _authenticateService.GetAuthenticatedUser(authorization);
            if (!authenticatedUser.IsValid())
                return BadRequest(new { message = "Invalid credentials" });


            HttpContext.Response.Cookies.Append(".AspCore.Refresh.Token", authenticatedUser.RefreshToken.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    MaxAge = authenticatedUser.TokenExpiryDate - DateTime.UtcNow
                });

            return Ok(authenticatedUser);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Authentication failed for user: {Username}", authorization.Username);
            return Unauthorized(new { message = "Invalid credentials" });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Authentication failed for user: {Username}", authorization.Username);
            return BadRequest(new { message = "Authentication service error" });
        }
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


    [Route("refresh-token")]
    [HttpPost]
    public ActionResult<AuthenticatedUser> AuthenticateByRefreshToken()
    {
        var token = HttpContext.Request.Cookies[".AspCore.Refresh.Token"];
        if (token is null)
            return Unauthorized(new { message = "Invalid credentials" });

        try
        {
            AuthenticatedUser authenticatedUser = _authenticateService.GetAuthenticatedUserByRefreshToken(token);
            if (authenticatedUser is null || !authenticatedUser.IsValid())
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
            HttpContext.Response.Cookies.Append(".AspCore.Refresh.Token", authenticatedUser?.RefreshToken?.Token?.ToString() ?? string.Empty,
                new CookieOptions
                {
                    MaxAge = authenticatedUser?.RefreshToken?.TokenExpiryDate - DateTime.Now
                });
            return authenticatedUser;

        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Authentication service error" });
        }
    }
}
