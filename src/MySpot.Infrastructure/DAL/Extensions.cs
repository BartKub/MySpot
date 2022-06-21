using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Services;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DAL.Repositories;
using MySpot.Infrastructure.Time;

namespace MySpot.Infrastructure.DAL
{
    internal static class Extensions
    {
        private const string OptionsSectionName = "postgres";

        public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration config)
        {

            services.Configure<PostgresOptions>(config.GetSection(OptionsSectionName));
            var postgresOptions = config.GetOptions<PostgresOptions>(OptionsSectionName);
            services.AddDbContext<MySpotDbContext>(x=> x.UseNpgsql(postgresOptions.ConnectionString));
            services.AddScoped<IWeeklyParkingSpotRepository, PostgresWeeklyParkingSpotRepository>();
            services.AddHostedService<DatabaseInitializer>();

            //EF core + npgsql issue
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            return services;
        }
    }
}
