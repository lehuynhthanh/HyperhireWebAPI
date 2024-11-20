using HyperhireWebAPI.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using MediatR;
using HyperhireWebAPI.Domain.Repositories;

namespace HyperhireWebAPI.Infrastructure;

public static class InfraRegistration
{
    public static void AddInfraRegistration(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<JwtService>();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("612446ebd0ed897b5b96431b94b6162e9e1d7efa8f39ba6db267e9839200584a")),
                ValidIssuer = "your-app",
                ValidAudience = "your-app"
            };
        });

        builder.Services.AddDbContext<HyperhireDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("HyperhireWebAPI")));
        
        
        builder.Services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        
        builder.Services.AddControllers();
    }

    public static void MigrateAdsDb(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var services = serviceScope.ServiceProvider;
        var dbcontext = services.GetRequiredService<HyperhireDbContext>(); // => Should change to ApplicationDbContext when intergrate with PMS api.
        dbcontext.Database.Migrate();
    }
}
