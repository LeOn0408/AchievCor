using AchievCor.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AchievCor.Server.Data;

public class AchievCorDbContext : DbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();

    private IConfiguration _configuration;

    public AchievCorDbContext(DbContextOptions<AchievCorDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }
}
