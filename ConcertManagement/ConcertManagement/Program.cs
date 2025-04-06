using ConcertManagement.Data.Entities;
using ConcertManagement.Data.Repositories;
using ConcertManagement.Infrastructure;
using ConcertManagement.Infrastructure.Middleware;
using ConcertManagement.Service;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ConcertManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.

            builder.Services.AddDbContextPool<CmDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DbContext"),
                provideroptions => provideroptions.EnableRetryOnFailure());
            });

            ConfigureLogging(builder);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IVenuesRepository, VenuesRepository>();
            builder.Services.AddScoped<IEventsRepository, EventsRepository>();
            builder.Services.AddScoped<IReservationsRepository, ReservationsRepository>();
            builder.Services.AddScoped<IConcertService, ConcertService>();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            var app = builder.Build();
            
            ConfigureMiddleware(app);

            app.Run();
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            app.UseSerilogRequestLogging(); // to log Http requests

            // Global exception handling
            //app.UseExceptionHandler("/error");
            app.UseMiddleware<ErrorHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }

        private static void ConfigureLogging(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(builder.Configuration)
                                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
