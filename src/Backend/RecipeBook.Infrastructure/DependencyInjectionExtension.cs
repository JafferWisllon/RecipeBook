using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Identity;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.PasswordHashing;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Infrastructure.DataAccess;
using RecipeBook.Infrastructure.DataAccess.Repositories;
using RecipeBook.Infrastructure.Identity;
using RecipeBook.Infrastructure.Security.PasswordHashing;
using RecipeBook.Infrastructure.Security.Tokens.Access;
using System.Reflection;

namespace RecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection service)
    {
        public void AddInfrastructure(IConfiguration configuration)
        {
            service.AddRepositories();

            service.AddTokensHandlers(configuration);

            service.AddScoped<ILoggedUser, LoggedUser>();

            service.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

            service.AddDbContext<RecipeBookDbContext>(config =>
            {
                var connectionString = configuration.GetConnectionString("DbConnection")!;
                config.UseMySQL(connectionString);
            });

            service.AddFluentMigratorCore().ConfigureRunner(config => 
            {
                config
                .AddMySql5()
                .WithGlobalConnectionString(_ =>
                {
                    var connectionString = configuration.GetConnectionString("DbConnection")!;
                    return connectionString;
                })
                .ScanIn(Assembly.Load("RecipeBook.Infrastructure"))
                .For.All();
            });
        }

        private void AddRepositories()
        {
            service.AddScoped<IUnitOfWork, UnitOfWork>();

            service.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            service.AddScoped<IUserReadOnlyRepository, UserRepository>();
        }

        private void AddTokensHandlers(IConfiguration configuration)
        {
            var expirationTimeInMinutes = configuration.GetValue<uint>("Jwt:ExpirationTimeMinutes");
            var signingKey = configuration.GetValue<string>("Jwt:SigningKey")!;

            service.AddScoped<IAccessTokenGenerator>(provider =>
            {
                return new JwtTokenHandler(expirationTimeInMinutes, signingKey);
            });
        }
    }
}