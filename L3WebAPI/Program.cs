
using L3WebAPI.Business.Implementations;
using L3WebAPI.Business.Interfaces;
using L3WebAPI.LocalData.Implementations;
using L3WebAPI.LocalData.Interfaces;

namespace L3WebAPI {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Services
            builder.Services.AddTransient<IGamesService, GamesService>();

            // Data
            builder.Services.AddTransient<IGamesDataAccess, GamesDataAccess>();

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

            app.Run();
        }
    }
}
