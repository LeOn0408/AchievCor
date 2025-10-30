using AchievCor.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace AchievCor.Server.Services
{
    public class SetupService
    {
        private readonly AchievCorDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public SetupService(AchievCorDbContext dbContext, IConfiguration configuration) 
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<bool> IsFirstRunAsync()
        {
            return !await _dbContext.UserRoles.AnyAsync(r => r.RoleId == 1);
        }
    }
}
