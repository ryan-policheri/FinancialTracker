namespace FinancialTracker.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadePaymentRecipientRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payment", "RecipientID", "dbo.Recipient");
            DropIndex("dbo.Payment", new[] { "RecipientID" });
            AlterColumn("dbo.Payment", "RecipientID", c => c.Int(nullable: false));
            CreateIndex("dbo.Payment", "RecipientID");
            AddForeignKey("dbo.Payment", "RecipientID", "dbo.Recipient", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payment", "RecipientID", "dbo.Recipient");
            DropIndex("dbo.Payment", new[] { "RecipientID" });
            AlterColumn("dbo.Payment", "RecipientID", c => c.Int());
            CreateIndex("dbo.Payment", "RecipientID");
            AddForeignKey("dbo.Payment", "RecipientID", "dbo.Recipient", "ID");
        }
    }
}
