namespace TaskBackEnd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubmittedToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Submitted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Submitted");
        }
    }
}
