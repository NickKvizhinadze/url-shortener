namespace UrlShortener.Core.Urls.Add;

public class AddUrlHandler
{
    private readonly ShortUrlGenerator _shortUrlGenerator;
    private readonly IUrlDatastore _urlDatastore;
    private readonly TimeProvider _timeProvider;

    public AddUrlHandler(ShortUrlGenerator shortUrlGenerator, IUrlDatastore urlDatastore,
        TimeProvider timeProvider)
    {
        _shortUrlGenerator = shortUrlGenerator;
        _urlDatastore = urlDatastore;
        _timeProvider = timeProvider;
    }

    public async Task<AddUrlResponse> HandleAsync(AddUrlRequest request, CancellationToken cancellationToken)
    {
        var shortened = new ShortenedUrl(request.LongUrl,
            _shortUrlGenerator.GenerateUniqueUrl(),
            request.CreatedBy,
            _timeProvider.GetUtcNow());
        
        await _urlDatastore.AddAsync(shortened, cancellationToken);
        return new AddUrlResponse(request.LongUrl, shortened.ShortUrl);
    }
}