namespace FinancialTracker.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPaymentPurpose : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentPurpose",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Purpose = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Payment", "PaymentPurposeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Payment", "PaymentPurposeID");
            AddForeignKey("dbo.Payment", "PaymentPurposeID", "dbo.PaymentPurpose", "ID", cascadeDelete: true);
            DropColumn("dbo.Payment", "Context");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payment", "Context", c => c.String(nullable: false, maxLength: 50));
            DropForeignKey("dbo.Payment", "PaymentPurposeID", "dbo.PaymentPurpose");
            DropIndex("dbo.Payment", new[] { "PaymentPurposeID" });
            DropColumn("dbo.Payment", "PaymentPurposeID");
            DropTable("dbo.PaymentPurpose");
        }
    }
}
