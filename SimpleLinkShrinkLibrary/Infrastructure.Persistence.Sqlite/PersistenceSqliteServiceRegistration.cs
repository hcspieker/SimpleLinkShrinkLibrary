using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleLinkShrinkLibrary.Infrastructure.Persistence.Exceptions;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence.Sqlite
{
    public static class PersistenceSqliteServiceRegistration
    {
        public static IServiceCollection EnableSqlitePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ShortlinkDbConnectionString")
                ?? throw new ConnectionStringNotFoundException("Connection string 'ShortlinkDbConnectionString' not found.");

            var migrationsAssembly = typeof(PersistenceSqliteServiceRegistration).Assembly;

            services.AddDbContext<ShortlinkDbContext>(o => o.UseSqlite(connectionString, x => x.MigrationsAssembly(migrationsAssembly)));

            services.EnableBasePersistenceServices();

            return services;
        }
    }
}
