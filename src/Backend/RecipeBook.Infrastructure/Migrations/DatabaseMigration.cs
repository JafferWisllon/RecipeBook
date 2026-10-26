using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Infrastructure.DataAccess;

namespace RecipeBook.Infrastructure.Migrations;

public class DatabaseMigration
{
    public static void ExecuteMigrations(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();
        
        runner.MigrateUp();
    }
}
