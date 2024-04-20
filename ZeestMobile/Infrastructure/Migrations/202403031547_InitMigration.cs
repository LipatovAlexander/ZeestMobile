using FluentMigrator;

namespace ZeestMobile.Infrastructure.Migrations;

[TimestampedMigration(2024, 03, 03, 15, 47)]
public class InitMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create
            .Table("TodoLists")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("ToDoItems").AsString().NotNullable();
    }
}