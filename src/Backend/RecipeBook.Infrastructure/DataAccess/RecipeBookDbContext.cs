using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WebApiTests")]
namespace RecipeBook.Infrastructure.DataAccess;

internal class RecipeBookDbContext : DbContext
{
    public RecipeBookDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

    public DbSet<User> Users { get; set; }
}
