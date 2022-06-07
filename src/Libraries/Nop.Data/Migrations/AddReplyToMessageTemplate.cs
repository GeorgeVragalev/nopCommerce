using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Data.Mapping;

namespace Nop.Data.Migrations;

[NopMigration("2022/06/07 13:00:00", "Message template add ReplyTo column", MigrationProcessType.Update)]
public class AddReplyToMessageTemplate : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(MessageTemplate))).Column(nameof(MessageTemplate.ReplyTo)).Exists())
        {
            Alter.Table(NameCompatibilityManager.GetTableName(typeof(MessageTemplate)))
                .AddColumn(nameof(MessageTemplate.ReplyTo))
                .AsString(int.MaxValue)
                .Nullable();
        }
    }
}