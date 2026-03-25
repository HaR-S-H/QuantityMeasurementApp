using System;
using Microsoft.Extensions.Configuration;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Repository;

namespace QuantityMeasurementApp.Startup
{
    internal sealed class ServiceFactory
    {
        private readonly IConfigurationRoot _configuration;

        public ServiceFactory()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
                .Build();
        }

        public IQuantityMeasurementService CreateService() => new QuantityMeasurementServiceImpl();

        public IQuantityMeasurementRepository CreateRepository()
        {
            var provider = _configuration["Persistence:Provider"];

            if (!string.Equals(provider, "SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                return QuantityMeasurementCacheRepository.Instance;
            }

            var connectionString = _configuration["Database:ConnectionString"];

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string is missing. Set Database:ConnectionString in appsettings or QMA_SQLSERVER_CONNECTION_STRING."
                );
            }

            try
            {
                return new QuantityMeasurementDatabaseRepository(connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Warning: SQL Server unavailable ({ex.Message}). Falling back to in-memory cache repository."
                );
                return QuantityMeasurementCacheRepository.Instance;
            }
        }
    }
}
