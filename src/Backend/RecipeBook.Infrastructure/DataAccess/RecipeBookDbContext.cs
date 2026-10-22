using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;

namespace RecipeBook.Infrastructure.DataAccess;

internal class RecipeBookDbContext : DbContext
{
    public RecipeBookDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

    public DbSet<User> Users { get; set; }
}
