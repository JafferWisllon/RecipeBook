using RecipeBook.Domain.Security.Tokens;

namespace RecipeBook.Api.Token;

internal class HttpContextTokenProvider : IAccessTokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTokenProvider(IHttpContextAccessor httpContextAccessor) 
        => _httpContextAccessor = httpContextAccessor;

    public string GetToken()
    {
        var accessToken = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
  
        return accessToken["Bearer ".Length..];
    }
}
