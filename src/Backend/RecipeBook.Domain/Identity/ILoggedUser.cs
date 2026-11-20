using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Identity;

public interface ILoggedUser
{
    Task<User> Get();
    Guid GetUserId();
}
