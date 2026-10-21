using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Security.PasswordHashing;
using RecipeBook.Infrastructure.Security.PasswordHashing;

namespace RecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection service)
    {
        public void AddInfrastructure()
        {
            service.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
        }
    }
}