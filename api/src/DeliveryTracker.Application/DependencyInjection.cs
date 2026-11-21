using Microsoft.Extensions.DependencyInjection;

namespace DeliveryTracker.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the Application layer services (use cases, validators, MediatR handlers).
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Later: services.AddMediatR(...) and services.AddValidatorsFromAssembly(...)
        return services;
    }
}
