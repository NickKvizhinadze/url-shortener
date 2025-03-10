using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.Tests.Extensions;

public static class ServiceCollectionExtensions
{
    public static void Remove<T>(this IServiceCollection serviceCollection)
    {
        var descriptor = serviceCollection.SingleOrDefault(descriptor => descriptor.ServiceType == typeof(T));
        
        if(descriptor != null)
            serviceCollection.Remove(descriptor);
    }
}