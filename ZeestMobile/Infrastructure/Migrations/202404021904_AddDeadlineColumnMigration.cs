using FluentMigrator;

namespace ZeestMobile.Infrastructure.Migrations;

[TimestampedMigration(2024, 04, 02, 19, 04)]
public class AddDeadlineColumnMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create
            .Column("Deadline")
            .OnTable("TodoItems").AsDateTime().Nullable();
    }
}