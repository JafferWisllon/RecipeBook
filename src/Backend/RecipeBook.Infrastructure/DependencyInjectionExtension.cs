using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.PasswordHashing;
using RecipeBook.Infrastructure.DataAccess;
using RecipeBook.Infrastructure.DataAccess.Repositories;
using RecipeBook.Infrastructure.Security.PasswordHashing;
using System.Reflection;

namespace RecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection service)
    {
        public void AddInfrastructure(IConfiguration configuration)
        {
            service.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
            service.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            service.AddScoped<IUserReadOnlyRepository, UserRepository>();

            service.AddDbContext<RecipeBookDbContext>(config =>
            {
                var connectionString = configuration.GetConnectionString("DbConnection")!;
                config.UseMySQL(connectionString);
            });

            service.AddScoped<IUnitOfWork, UnitOfWork>();

            service.AddFluentMigratorCore().ConfigureRunner(config => {
                var connectionString = configuration.GetConnectionString("DbConnection")!;

                config
                .AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("RecipeBook.Infrastructure"))
                .For.All();
            });
        }
    }
}