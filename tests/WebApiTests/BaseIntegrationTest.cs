using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Infrastructure.DataAccess;
using System.Net.Http.Json;

namespace WebApiTests;

public abstract class BaseIntegrationTest : IClassFixture<RecipeBookApplicationFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly HttpClient _httpClient;
    internal readonly RecipeBookDbContext DbContext;
    public BaseIntegrationTest(RecipeBookApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();

        _scope = factory.Services.CreateScope();

        DbContext = _scope.ServiceProvider.GetRequiredService<RecipeBookDbContext>();
    }

    protected async Task<HttpResponseMessage> Post(string requestUri, object request, string culture = "en-US")
    {
        ChangeRequestCulture(culture);        

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    private void ChangeRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);
    }

    public void Dispose()
    {
        _scope?.Dispose();
        DbContext?.Dispose();
    }
}
