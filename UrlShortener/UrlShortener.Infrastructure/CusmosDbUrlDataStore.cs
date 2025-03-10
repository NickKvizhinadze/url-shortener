using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using UrlShortener.Core.Urls;
using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Infrastructure;

public class CosmosDbUrlDataStore : IUrlDatastore
{
    private readonly Container _container;

    public CosmosDbUrlDataStore(Container container)
    {
        _container = container;
    }

    public async Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken)
    {
        var document = (ShortenedUrlCosmos)shortened;
        await _container.CreateItemAsync(document,
            new PartitionKey(document.PartitionKey),
            cancellationToken: cancellationToken);
    }
}

internal class ShortenedUrlCosmos
{
    public string LongUrl { get; }

    [JsonProperty(PropertyName = "id")] // Cosmos DB Unique Identifier
    public string ShortUrl { get; }

    public DateTimeOffset CreatedOn { get; }
    public string CreatedBy { get; }

    public string PartitionKey => ShortUrl[..1]; //Cosmos DB partition key

    public ShortenedUrlCosmos(string longUrl, string shortUrl, string createdBy, DateTimeOffset createdOn)
    {
        LongUrl = longUrl;
        ShortUrl = shortUrl;
        CreatedBy = createdBy;
        CreatedOn = createdOn;
    }

    public static implicit operator ShortenedUrl(ShortenedUrlCosmos url)
        => new(new Uri(url.LongUrl), url.ShortUrl, url.CreatedBy, url.CreatedOn);


    public static explicit operator ShortenedUrlCosmos(ShortenedUrl url)
        => new(url.LongUrl.ToString(), url.ShortUrl, url.CreatedBy, url.CreatedOn);
}