
using L3WebAPI.Business.Implementations;
using L3WebAPI.Business.Interfaces;
using L3WebAPI.Common;
using L3WebAPI.Database;
using L3WebAPI.Database.Implementations;
using L3WebAPI.LocalData.Interfaces;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

namespace L3WebAPI {
    public class Program {
        public static void Main(string[] args) {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");

            try {
                var builder = WebApplication.CreateBuilder(args);

                var rawConfig = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddEnvironmentVariables()
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets<Program>()
                    .Build();

                var appSettingsSection = rawConfig.GetSection("AppSettings");
                builder.Services.Configure<AppSettings>(appSettingsSection);

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                // Add services to the container.
                builder.Services.AddTransient<DatabaseContext>();

                // Services
                builder.Services.AddTransient<IGamesService, GamesService>();

                // Data
                builder.Services.AddTransient<IGamesDataAccess, GamesDatabaseAccess>();

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment()) {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                using (var scope = app.Services.CreateScope()) {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                    // Here is the migration executed
                    dbContext.Database.Migrate();
                }

                app.Run();
            } catch (Exception exception) {
                // NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            } finally {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }
    }
}
