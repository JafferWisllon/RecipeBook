using FluentMigrator;

namespace RecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_USERS, "Creating users table")]
public class Version000001 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Email").AsString(250).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable();
    }
}
