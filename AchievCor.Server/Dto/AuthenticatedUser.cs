namespace AchievCor.Server.Dto;

public class AuthenticatedUser
{
    public required string Token { get; set; }

    public required DateTime TokenExpiryDate { get; set; }
    public required UserDto User { get; set; }
}