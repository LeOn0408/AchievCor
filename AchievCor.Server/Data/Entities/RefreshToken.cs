using System.ComponentModel.DataAnnotations.Schema;

namespace AchievCor.Server.Data.Entities
{
    [Table("ac_refresh_token")]
    public class RefreshToken
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public UserEntity? User { get; set; }
        public DateTime TokenExpiryDate { get; set; }
    }
}
