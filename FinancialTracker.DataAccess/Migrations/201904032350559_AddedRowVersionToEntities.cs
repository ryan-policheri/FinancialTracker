namespace FinancialTracker.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRowVersionToEntities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentPurpose", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Payment", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Recipient", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipient", "RowVersion");
            DropColumn("dbo.Payment", "RowVersion");
            DropColumn("dbo.PaymentPurpose", "RowVersion");
        }
    }
}
