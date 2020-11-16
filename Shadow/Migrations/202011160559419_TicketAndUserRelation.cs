namespace Shadow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketAndUserRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Tickets", "OwnerId", c => c.String(maxLength: 128));
            AddColumn("dbo.Tickets", "AssignedToUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tickets", "OwnerId");
            CreateIndex("dbo.Tickets", "AssignedToUserId");
            AddForeignKey("dbo.Tickets", "AssignedToUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Tickets", "OwnerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tickets", "AssignedToUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Tickets", new[] { "AssignedToUserId" });
            DropIndex("dbo.Tickets", new[] { "OwnerId" });
            DropColumn("dbo.Tickets", "AssignedToUserId");
            DropColumn("dbo.Tickets", "OwnerId");
            DropColumn("dbo.AspNetUsers", "Discriminator");
        }
    }
}
