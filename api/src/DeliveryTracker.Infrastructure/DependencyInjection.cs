using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryTracker.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the Infrastructure layer services (EF Core DbContext, repositories, external services).
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Later: services.AddDbContext<AppDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("Default")))
        return services;
    }
}
