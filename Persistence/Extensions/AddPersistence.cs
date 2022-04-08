using ApplicationServices.Interfaces;
using ApplicationServices.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Persistence.Extensions
{


    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(DbConnectionStrings.AccountServiceDbConnection);

            services.AddDbContext<AccountServiceDbContext>(options =>
                        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.TryAddScoped<IAccountServiceDbContext>(provider => provider.GetService<AccountServiceDbContext>());

            return services;
        }
    }
}
