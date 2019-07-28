namespace FinancialTracker.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPaymentDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payment", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payment", "Date");
        }
    }
}
