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
            .WithColumn("Name").AsString().NotNullable();

        Create
            .Table("TodoItems")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("TodoListId").AsInt32().ForeignKey("TodoLists", "Id").Indexed()
            .WithColumn("Text").AsString().NotNullable()
            .WithColumn("Done").AsBoolean().NotNullable();
    }
}