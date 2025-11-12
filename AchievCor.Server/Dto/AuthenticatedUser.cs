using AchievCor.Server.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace AchievCor.Server.Dto;

public class AuthenticatedUser
{
    public string? JwtToken { get; private set; }
    public DateTime TokenExpiryDate { get; set; }
    public UserDto User { get; set; }

    [JsonIgnore]
    public RefreshToken RefreshToken { get; private set; } = null!;

    public AuthenticatedUser(UserDto userDto, JwtSecurityToken jwt)
    {
        SetToken(jwt);
        User = userDto;
    }

    public AuthenticatedUser(UserDto userDto)
    {
        User = userDto;
    }

    public void SetToken(JwtSecurityToken jwt)
    {
        JwtToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        TokenExpiryDate = jwt.ValidTo;
    }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(JwtToken) &&
               TokenExpiryDate > DateTime.UtcNow &&
               User != null &&
               !string.IsNullOrEmpty(RefreshToken.Token) &&
               RefreshToken.TokenExpiryDate > DateTime.UtcNow;
    }

    public void SetRefreshToken(RefreshToken token)
    {
        RefreshToken = token;
    }
}