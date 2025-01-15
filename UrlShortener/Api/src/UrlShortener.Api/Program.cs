using Azure.Identity;
using Scalar.AspNetCore;
using UrlShortener.Api.Extensions;
using UrlShortener.Core;
using UrlShortener.Core.Urls.Add;

var builder = WebApplication.CreateBuilder(args);

var keyVaultName = builder.Configuration["KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{keyVaultName}.vault.azure.net/"),
        new DefaultAzureCredential());
}


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddUrlFeature();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapPost("/api/urls", async (
    AddUrlHandler handler,
    AddUrlRequest request,
    CancellationToken cancellationToken) =>
{
    var requestWithUser = request with
    {
        CreatedBy = "nick@kvizhinadze.net"
    };
    
    var result = await handler.HandleAsync(requestWithUser, cancellationToken);

    if (!result.Succeeded)
        return Results.BadRequest(result.Error);

    return Results.Created($"/api/urls/{result.Value!.ShortUrl}", result.Value);
});


app.Run();
