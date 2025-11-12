using System.ComponentModel.DataAnnotations.Schema;

namespace AchievCor.Server.Data.Entities
{
    [Table("ac_refresh_token")]
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public int LocalIdentityId { get; set; }
        public LocalIdentity? LocalIdentity { get; set; }
        public DateTime TokenExpiryDate { get; set; }
    }
}
