using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}
