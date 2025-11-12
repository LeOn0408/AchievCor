using System.ComponentModel.DataAnnotations.Schema;

namespace AchievCor.Server.Data.Entities;

[Table("ac_local_identity")]
public class LocalIdentity
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public string? PasswordHash { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;  
    public RefreshToken? RefreshToken { get; set; }
}
