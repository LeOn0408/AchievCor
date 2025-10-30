using AchievCor.Server.Data;
using AchievCor.Server.Data.Authorization;
using AchievCor.Server.Data.Entities;
using AchievCor.Server.Options;
using AchievCor.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
builder.Services.AddDbContext<AchievCorDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddTransient<IdentityService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var jwtSecret = builder.Configuration["Auth:SecurityKey"];

if (string.IsNullOrWhiteSpace(jwtSecret))
{
    jwtSecret = JwtSecretGenerator.Generate();

    builder.Configuration["Auth:SecurityKey"] = jwtSecret;

    var file = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
    if (File.Exists(file))
    {
        var json = File.ReadAllText(file);
        dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json)!;
        jsonObj["Auth"]["SecurityKey"] = jwtSecret;
        string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(file, output);
    }
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var authOption = new AuthOptions(builder.Configuration);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = authOption.Issuer,
        ValidateAudience = true,
        ValidAudience = authOption.Audience,
        ValidateLifetime = true,
        IssuerSigningKey = authOption.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true,
    };
});
builder.Services.AddAuthorization();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AchievCorDbContext>();
    var auth = new AuthOptions(builder.Configuration);
    var admin = new AdminOptions(builder.Configuration);
    await DbSeeder.InitializeAsync(db, auth, admin);
}

app.Run();
