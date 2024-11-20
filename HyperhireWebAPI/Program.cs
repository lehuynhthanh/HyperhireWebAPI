using HyperhireWebAPI.Infrastructure;
using HyperhireWebAPI.Swagger;
using HyperhireWebAPI.Application;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        
        builder.Services.AddControllers();
        builder.AddInfraRegistration();
        builder.AddApplication();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerExtension(builder.Configuration);
        builder.Services.AddHttpContextAccessor();

        //builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

        
        



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            //app.UseSwaggerExtension(builder.Configuration);
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MigrateAdsDb();

        app.Run();
    }
}