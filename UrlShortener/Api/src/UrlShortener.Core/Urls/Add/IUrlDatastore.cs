namespace UrlShortener.Core.Urls.Add;

public interface IUrlDatastore
{
    Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken);
}