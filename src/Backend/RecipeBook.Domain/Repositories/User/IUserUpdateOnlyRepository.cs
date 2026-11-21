namespace RecipeBook.Domain.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    void UpdateProdile(Entities.User user);
    Task UpdatePassword(Guid userId, string passwordHash);
}
