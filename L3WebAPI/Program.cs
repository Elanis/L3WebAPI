
using L3WebAPI.Business.Implementations;
using L3WebAPI.Business.Interfaces;
using L3WebAPI.Common;
using L3WebAPI.Database;
using L3WebAPI.Database.Implementations;
using L3WebAPI.LocalData.Interfaces;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NLog;
using NLog.Web;
using System.Text;
using System.Text.Json;

namespace L3WebAPI {
	public class Program {
		public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

		private static Task WriteHealthCheckResponse(HttpContext context, HealthReport healthReport) {
			context.Response.ContentType = "application/json; charset=utf-8";

			var options = new JsonWriterOptions { Indented = true };

			using var memoryStream = new MemoryStream();
			using (var jsonWriter = new Utf8JsonWriter(memoryStream, options)) {
				jsonWriter.WriteStartObject();
				jsonWriter.WriteString("status", healthReport.Status.ToString());
				jsonWriter.WriteStartObject("results");

				foreach (var healthReportEntry in healthReport.Entries) {
					jsonWriter.WriteStartObject(healthReportEntry.Key);
					jsonWriter.WriteString("status",
						healthReportEntry.Value.Status.ToString());
					jsonWriter.WriteString("description",
						healthReportEntry.Value.Description);
					jsonWriter.WriteStartObject("data");

					foreach (var item in healthReportEntry.Value.Data) {
						jsonWriter.WritePropertyName(item.Key);

						JsonSerializer.Serialize(jsonWriter, item.Value,
							item.Value?.GetType() ?? typeof(object));
					}

					jsonWriter.WriteEndObject();
					jsonWriter.WriteEndObject();
				}

				jsonWriter.WriteEndObject();
				jsonWriter.WriteEndObject();
			}

			return context.Response.WriteAsync(
				Encoding.UTF8.GetString(memoryStream.ToArray()));
		}

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

				builder.Services.AddCors(options => {
					options.AddPolicy(name: MyAllowSpecificOrigins,
						policy => {
							policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
						});
				});

				builder.Services.AddHealthChecks()
						.AddNpgSql(appSettingsSection["ConnectionString"]);

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

				app.UseCors(MyAllowSpecificOrigins);

				app.UseAuthorization();

				// https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks
				app.MapHealthChecks("/health", new HealthCheckOptions {
					ResponseWriter = WriteHealthCheckResponse
				});

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
