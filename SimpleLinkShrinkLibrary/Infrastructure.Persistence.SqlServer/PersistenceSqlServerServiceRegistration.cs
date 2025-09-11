using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleLinkShrinkLibrary.Infrastructure.Persistence.Exceptions;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence.SqlServer
{
    public static class PersistenceSqlServerServiceRegistration
    {
        public static IServiceCollection EnableSqlServerPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ShortlinkDbConnectionString");

            if (connectionString == null)
                throw new ConnectionStringNotFoundException("Connection string 'ShortlinkDbConnectionString' not found.");

            var migrationsAssembly = typeof(PersistenceSqlServerServiceRegistration).Assembly;


            services.AddDbContext<ShortlinkDbContext>(o => o.UseSqlServer(connectionString, x => x.MigrationsAssembly(migrationsAssembly)));

            services.EnableBasePersistenceServices();


            return services;
        }
    }
}
