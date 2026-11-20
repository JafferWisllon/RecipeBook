using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Identity;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Infrastructure.DataAccess;

namespace RecipeBook.Infrastructure.Identity;

internal class LoggedUser : ILoggedUser
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly RecipeBookDbContext _dbContext;

    public LoggedUser(IAccessTokenProvider accessTokenProvider, RecipeBookDbContext dbContext)
    {
        _accessTokenProvider = accessTokenProvider;
        _dbContext = dbContext;
    }

    public async Task<User> Get()
    {
        var userId = GetUserId();

        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.Active && user.Id == userId);
    }

    public Guid GetUserId()
    {
        var accessToken = _accessTokenProvider.GetToken();

        var handler = new JsonWebTokenHandler();

        var jsonWebToken = handler.ReadJsonWebToken(accessToken);

        var subject = jsonWebToken.Subject;

        return Guid.Parse(subject);
    }
}
