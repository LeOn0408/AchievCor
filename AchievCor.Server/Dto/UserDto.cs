namespace AchievCor.Server.Dto;

public class UserDto
{
    public int Id { get; set; }
    public required string Login { get; set; } 
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
}
