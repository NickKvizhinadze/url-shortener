using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using UrlShortener.Api.Core.Tests.TestDoubles;
using UrlShortener.Core;
using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Api.Core.Tests.Urls;

public class AddUrlScenarios
{
    private readonly AddUrlHandler _handler;
    private readonly InMemoryUrlDataStore _urlDatastore = new();
    private readonly TimeProvider _timeProvider;

    public AddUrlScenarios()
    {
        var tokenProvider = new TokenProvider();
        tokenProvider.AssignRange(1, 5);
        var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);

        _timeProvider = new FakeTimeProvider();
        _handler = new AddUrlHandler(shortUrlGenerator, _urlDatastore, _timeProvider);
    }

    [Fact]
    public async Task Should_return_shortened_url()
    {
        var request = CreateAddUrlRequest();

        var response = await _handler.HandleAsync(request, CancellationToken.None);
        
        response.Succeeded.Should().BeTrue();
        response.Value!.ShortUrl.Should().NotBeEmpty();
        response.Value.ShortUrl.Should().Be("1");
    }

    [Fact]
    public async Task Should_return_shortened_url_with_created_by_and_created_on()
    {
        var request = CreateAddUrlRequest();

        var response = await _handler.HandleAsync(request, CancellationToken.None);

        response.Succeeded.Should().BeTrue();
        _urlDatastore.Should().ContainKey(response.Value!.ShortUrl);
        _urlDatastore[response.Value.ShortUrl].CreatedBy.Should().Be(request.CreatedBy);
        _urlDatastore[response.Value.ShortUrl].CreatedOn.Should().Be(_timeProvider.GetUtcNow());
    }

    [Fact]
    public async Task Should_save_short_url()
    {
        var request = CreateAddUrlRequest();

        var response = await _handler.HandleAsync(request, CancellationToken.None);

        _urlDatastore.Should().ContainKey(response.Value!.ShortUrl);
    }

    [Fact]
    public async Task Should_return_error_if_created_by_is_empty()
    {
        var request = CreateAddUrlRequest(createdBy: string.Empty);
        var response = await _handler.HandleAsync(request, CancellationToken.None);

        response.Succeeded.Should().BeFalse();
        response.Error.Code.Should().Be("missing_value");

    }

    private static AddUrlRequest CreateAddUrlRequest(string createdBy = "nick@kvizhinadze.net")
    {
        return new AddUrlRequest(new Uri("https://www.example.com"), createdBy);
    }
}