using FinBeat.DAL.Repositories;
using FinBeat.DAL.Repositories.Implementation;
using FinBeat.DAL.Services.Implementation;
using FinBeat.DAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinBeat.DAL.Extensions
{
    public static class DiExtensions
    {
        /// <summary>
        /// Configures and registers the database access layer (DAL) services in the application's dependency injection container.
        /// This method sets up the connection to the PostgreSQL database and registers the DbContext and related services.
        /// </summary>
        /// <param name="services">The service collection to which the DAL services will be added.</param>
        /// <param name="configuration">The application configuration containing the connection string for the database.</param>
        /// <returns>The updated service collection with the DAL services registered.</returns>
        public static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgreSQL");
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();

            return services;
        }

        /// <summary>
        /// Registers the repository services in the application's dependency injection container.
        /// This method configures the repositories to be used in the application by adding their respective interfaces and implementations.
        /// </summary>
        /// <param name="services">The service collection to which the repository services will be added.</param>
        /// <returns>The updated service collection with the repository services registered.</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IEntityRepository, EntityRepository>();

            return services;
        }
    }
}
