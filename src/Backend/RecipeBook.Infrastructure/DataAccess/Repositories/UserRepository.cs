using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.User;

namespace RecipeBook.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly RecipeBookDbContext _dbContext;

    public UserRepository(RecipeBookDbContext dbContext) 
        => _dbContext = dbContext;

    public async Task Add(User user) 
        => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email)
        => await _dbContext.Users.AnyAsync(user => user.Active && user.Email.Equals(email));
}
