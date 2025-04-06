using ConcertManagement.Data.Entities;
using ConcertManagement.Data.Repositories;
using ConcertManagement.Infrastructure;
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

            Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(builder.Configuration)
                                .Enrich.FromLogContext()
                                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                                .CreateLogger();

            builder.Host.UseSerilog();

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

            app.UseSerilogRequestLogging(); // to log Http requests

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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
