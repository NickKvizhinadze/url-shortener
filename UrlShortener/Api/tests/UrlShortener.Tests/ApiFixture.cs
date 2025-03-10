using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Api;
using UrlShortener.Core.Urls.Add;
using UrlShortener.Api.Extensions;
using UrlShortener.Tests.Extensions;


namespace UrlShortener.Tests;

public class ApiFixture: WebApplicationFactory<IAssemblyMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Remove<IUrlDatastore>();

            services.AddSingleton<IUrlDatastore>(new InMemoryUrlDataStore());

        });
        
        base.ConfigureWebHost(builder);
    }
}