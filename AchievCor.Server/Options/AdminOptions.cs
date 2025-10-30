namespace AchievCor.Server.Options;

public class AdminOptions
{
    public string Login { get; }
    public string Password { get; }
    public string Email { get; }

    public AdminOptions(IConfiguration configuration)
    {
        Login = configuration["Auth:Admin:Login"] ?? "admin";
        Password = configuration["Auth:Admin:Password"] ?? "Ch@ngeMeN0w";
        Email = configuration["Auth:Admin:Email"] ?? "admin@achievcor.local";
    }
}
