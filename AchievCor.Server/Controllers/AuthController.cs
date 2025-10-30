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
            if (string.IsNullOrWhiteSpace(authenticatedUser.Token))
                return BadRequest("Token receipt error");

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
}
