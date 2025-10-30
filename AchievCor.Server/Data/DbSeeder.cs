using AchievCor.Server.Data.Authorization;
using AchievCor.Server.Data.Entities;
using AchievCor.Server.Options;
using Microsoft.EntityFrameworkCore;

namespace AchievCor.Server.Data;

public class DbSeeder
{
    public static async Task InitializeAsync(AchievCorDbContext context, AuthOptions authOptions, AdminOptions adminOptions)
    {
        await context.Database.MigrateAsync();

        string adminLogin = adminOptions.Login;

        LocalIdentity? admin = await context.LocalIdentity.FirstOrDefaultAsync(u => u.Login == adminLogin);

        if (admin != null)
        {
            return;
        }

        var adminUser = new UserEntity
        {
            Email = adminOptions.Email,
            FirstName = adminLogin
        };

        LocalIdentity adminIdentity = new()
        {
            Login = adminLogin,
            PasswordHash = Password.Hash(adminOptions.Password),
            User = adminUser
        };
        context.LocalIdentity.Add(adminIdentity);
        await context.SaveChangesAsync();

        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        if (adminRole == null)
        {
            adminRole = new Role { Name = "Admin", Description = "System administrator" };
            context.Roles.Add(adminRole);
            await context.SaveChangesAsync();
        }

        context.UserRoles.Add(new UserRole
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id,
            AppointedFrom = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
    }
}
