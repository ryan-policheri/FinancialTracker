namespace FinancialTracker.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRecipient : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recipient",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Address = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Payment", "RecipientID", c => c.Int());
            CreateIndex("dbo.Payment", "RecipientID");
            AddForeignKey("dbo.Payment", "RecipientID", "dbo.Recipient", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payment", "RecipientID", "dbo.Recipient");
            DropIndex("dbo.Payment", new[] { "RecipientID" });
            DropColumn("dbo.Payment", "RecipientID");
            DropTable("dbo.Recipient");
        }
    }
}
