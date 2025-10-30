using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AchievCor.Server.Options;

public class AuthOptions
{
    private string? _issuer = "AchievCorAsp"; // издатель токена
    private string? _audience = "AchievCorVue"; // потребитель токена

    private readonly string _securityKey;

    public string? Issuer { get => _issuer; private set => _issuer = value; }
    public string? Audience { get => _audience; private set => _audience = value; }

    public AuthOptions(IConfiguration configuration)
    {
        _securityKey = configuration["Auth:SecurityKey"]
                ?? throw new SecurityTokenValidationException("Security key not found");

        Issuer = configuration["Auth:Issuer"];
        Audience = configuration["Auth:Audience"];
    }

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new(Encoding.UTF8.GetBytes(_securityKey));
    }
}
