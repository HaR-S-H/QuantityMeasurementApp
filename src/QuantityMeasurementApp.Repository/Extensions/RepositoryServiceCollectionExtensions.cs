using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace QuantityMeasurementApp.Repository
{
    /// <summary>
    /// Registers repository services, including SQL Server persistence and optional Redis caching.
    /// </summary>
    public static class RepositoryServiceCollectionExtensions
    {
        /// <summary>
        /// Adds repository dependencies using connection strings from configuration.
        /// </summary>
        public static IServiceCollection AddQuantityMeasurementRepository(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var sqlConnectionString = configuration.GetConnectionString("SqlServer");
            if (string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                throw new InvalidOperationException(
                    "Missing SQL Server connection string. Configure ConnectionStrings:SqlServer."
                );
            }

            services.AddDbContext<QuantityMeasurementDbContext>(options =>
                options.UseSqlServer(sqlConnectionString)
            );

            services.AddScoped<IUserRepository, UserDatabaseRepository>();
            services.AddScoped<IRevokedTokenRepository, RevokedTokenDatabaseRepository>();

            // Base repository always writes/reads from SQL Server.
            services.AddScoped<QuantityMeasurementDatabaseRepository>();

            var redisConnectionString = configuration.GetConnectionString("Redis");
            if (!string.IsNullOrWhiteSpace(redisConnectionString))
            {
                try
                {
                    // Keep API usable even when Redis is temporarily unavailable.
                    var options = ConfigurationOptions.Parse(redisConnectionString);
                    options.AbortOnConnectFail = true;
                    options.ConnectRetry = 0;
                    options.ConnectTimeout = 1000;
                    options.SyncTimeout = 1000;

                    var multiplexer = ConnectionMultiplexer.Connect(options);
                    services.AddSingleton<IConnectionMultiplexer>(multiplexer);

                    services.AddScoped<IQuantityMeasurementRepository>(serviceProvider =>
                        new QuantityMeasurementRedisRepository(
                            serviceProvider.GetRequiredService<QuantityMeasurementDatabaseRepository>(),
                            serviceProvider.GetRequiredService<IConnectionMultiplexer>()
                        )
                    );

                    return services;
                }
                catch
                {
                    // Fall back to SQL-only repository when Redis connection cannot be established.
                }
            }

            services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>();
            return services;
        }
    }
}