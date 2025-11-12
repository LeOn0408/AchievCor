using AchievCor.Server.Data;
using AchievCor.Server.Data.Authorization;
using AchievCor.Server.Data.Entities;
using AchievCor.Server.Dto;
using AchievCor.Server.Mappings;
using AchievCor.Server.Options;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;

namespace AchievCor.Server.Services;

public class IdentityService
{
    private readonly IConfiguration _configuration;
    private readonly AchievCorDbContext _applicationContext;

    public IdentityService(IConfiguration configuration, AchievCorDbContext applicationContext)
    {
        _configuration = configuration;
        _applicationContext = applicationContext;
    }

    public AuthenticatedUser GetAuthenticatedUser(AuthorizationData authorization)
    {
        LocalIdentity? identity = _applicationContext.LocalIdentity.FirstOrDefault(x => x.Login == authorization.Username);
            
        if (identity == null || !Password.Verify(authorization.Password, identity.PasswordHash ?? string.Empty))
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }


        UserDto user = _applicationContext.Users
            .Where(u => u.Id == identity.UserId)
            .First().ToDto();

        var token = GetJwtToken(user);
        AuthenticatedUser authenticatedUser = new(user, token);
        authenticatedUser.SetRefreshToken(CreateRefreshToken(identity));

        return authenticatedUser;
    }

    private RefreshToken CreateRefreshToken(LocalIdentity identity)
    {
        var token = new RefreshToken
        {
            TokenExpiryDate = DateTime.UtcNow.AddDays(7),
            LocalIdentity = identity,
            Token = Guid.NewGuid().ToString()
        };
        SaveTokenToDatabase(token);
        return token;
    }

    private void SaveTokenToDatabase(RefreshToken token)
    {
        var tokenDatabase = _applicationContext.RefreshToken.FirstOrDefault(t => t.LocalIdentity == token.LocalIdentity);
        if (tokenDatabase is null)
        {
            _applicationContext.RefreshToken.Add(token);
            _applicationContext.SaveChanges();
        }
        else
        {
            tokenDatabase.Token = token.Token;
            tokenDatabase.TokenExpiryDate = token.TokenExpiryDate;
            _applicationContext.SaveChanges();
        }
    }

    public AuthenticatedUser GetAuthenticatedUserByRefreshToken(string refreshToken)
    {
        var refreshTokenData = _applicationContext.RefreshToken.Include(u => u.LocalIdentity).FirstOrDefault(t => t.Token == refreshToken);

        if (refreshTokenData is null || refreshTokenData.TokenExpiryDate < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var identity = _applicationContext.LocalIdentity.FirstOrDefault(x => x.Id == refreshTokenData.LocalIdentityId);
        
        if (identity == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        UserDto user = _applicationContext.Users
            .Where(u => u.Id == identity.UserId)
            .First().ToDto();

        var token = GetJwtToken(user);
        AuthenticatedUser authenticatedUser = new(user, token);
        authenticatedUser.SetRefreshToken(CreateRefreshToken(identity));

        return authenticatedUser;
    }


    private JwtSecurityToken GetJwtToken(UserDto user)
    {
        AuthOptions authOptions = new(_configuration);
        var claims = new List<Claim> {
                new(ClaimTypes.Name, user.Login),
                //TODO: Add role
                //new(ClaimTypes.Role, '')
            };
        var jwt = new JwtSecurityToken(
                issuer: authOptions.Issuer,
        audience: authOptions.Audience,
        claims: claims,
        expires: DateTime.Now.Add(TimeSpan.FromMinutes(15)), // время действия 15 минут
        signingCredentials: new SigningCredentials(authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        return jwt;
    }
}
