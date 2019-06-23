namespace TaskBackEnd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubmittedOfflineandonline : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "SubmittedOnline", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "SubmittedOffline", c => c.Boolean(nullable: false));
            DropColumn("dbo.Orders", "Submitted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Submitted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Orders", "SubmittedOffline");
            DropColumn("dbo.Orders", "SubmittedOnline");
        }
    }
}
