using AchievCor.Server.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace AchievCor.Server.Dto;

public class AuthenticatedUser
{
    public string? Token { get; private set; }
    public DateTime TokenExpiryDate { get; set; }
    public UserDto User { get; set; }

    [JsonIgnore]
    public RefreshToken? RefreshToken { get; set; }


    public AuthenticatedUser(UserDto userDto, JwtSecurityToken jwt)
    {
        Token = new JwtSecurityTokenHandler().WriteToken(jwt);
        User = userDto;
    }

    public AuthenticatedUser(UserDto userDto)
    {
        User = userDto;
    }
    public void SetToken(JwtSecurityToken jwt)
    {
        Token = new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}