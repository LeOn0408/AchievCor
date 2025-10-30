using AchievCor.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AchievCor.Server.Data;

public class AchievCorDbContext : DbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<LocalIdentity> LocalIdentity => Set<LocalIdentity>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();


    private IConfiguration _configuration;

    public AchievCorDbContext(DbContextOptions<AchievCorDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }
}
