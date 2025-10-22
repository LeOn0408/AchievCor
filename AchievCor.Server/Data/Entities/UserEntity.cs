using System.ComponentModel.DataAnnotations.Schema;

namespace AchievCor.Server.Data.Entities;

[Table("ac_users")]
public class UserEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;

    public string? Email { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public LocalIdentity? LocalIdentity { get; set; }
}
