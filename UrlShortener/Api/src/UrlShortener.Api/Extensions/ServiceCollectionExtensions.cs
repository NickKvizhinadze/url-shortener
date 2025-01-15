﻿using UrlShortener.Core;
using UrlShortener.Core.Urls;
using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUrlFeature(this IServiceCollection services)
    {
        services.AddScoped<AddUrlHandler>();
        services.AddSingleton<TokenProvider>(_ =>
        {
            var tokenProvider = new TokenProvider();
            tokenProvider.AssignRange(1, 1000);
            return tokenProvider;
        });
        
        services.AddScoped<ShortUrlGenerator>();

        services.AddSingleton<IUrlDatastore, InMemoryUrlDataStore>();

        return services;
    }
}

public class InMemoryUrlDataStore : Dictionary<string, ShortenedUrl>, IUrlDatastore
{
    public Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken)
    {
        Add(shortened.ShortUrl, shortened);
        return Task.CompletedTask;
    }
}