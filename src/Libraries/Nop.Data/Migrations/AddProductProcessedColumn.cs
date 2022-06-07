using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping;

namespace Nop.Data.Migrations;

[NopMigration("2022/07/06 18:00:00", "Product added Processed column", MigrationProcessType.Update)]
public class AddProductProcessedColumn : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(Product))).Column(nameof(Product.Processed)).Exists())
        {
            Alter.Table(NameCompatibilityManager.GetTableName(typeof(Product)))
                .AddColumn(nameof(Product.Processed))
                .AsBoolean()
                .NotNullable()
                .SetExistingRowsTo(false);
        }
    }
}