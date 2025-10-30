using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchievCor.Server.Data.Entities;

[Table("ac_user_role")]
public class UserRole
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public UserEntity? User { get; set; }

    [Required]
    public int RoleId { get; set; }
    public Role? Role { get; set; }

    public DateTime AppointedFrom { get; set; } = DateTime.UtcNow;

    public DateTime? AppointedUntil { get; set; }

    public DateTime AppointmentDate { get; set; } = DateTime.UtcNow;

    public int? AppointedByUserId { get; set; }

    public UserEntity? AppointedByUser { get; set; }
}
