namespace Shadow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThreeTicketClasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TicketPriorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tickets", "TicketStatusId", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "TicketPrioritieId", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "TicketTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "TicketStatusId");
            CreateIndex("dbo.Tickets", "TicketPrioritieId");
            CreateIndex("dbo.Tickets", "TicketTypeId");
            AddForeignKey("dbo.Tickets", "TicketPrioritieId", "dbo.TicketPriorities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tickets", "TicketStatusId", "dbo.TicketStatus", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tickets", "TicketTypeId", "dbo.TicketTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "TicketTypeId", "dbo.TicketTypes");
            DropForeignKey("dbo.Tickets", "TicketStatusId", "dbo.TicketStatus");
            DropForeignKey("dbo.Tickets", "TicketPrioritieId", "dbo.TicketPriorities");
            DropIndex("dbo.Tickets", new[] { "TicketTypeId" });
            DropIndex("dbo.Tickets", new[] { "TicketPrioritieId" });
            DropIndex("dbo.Tickets", new[] { "TicketStatusId" });
            DropColumn("dbo.Tickets", "TicketTypeId");
            DropColumn("dbo.Tickets", "TicketPrioritieId");
            DropColumn("dbo.Tickets", "TicketStatusId");
            DropTable("dbo.TicketTypes");
            DropTable("dbo.TicketStatus");
            DropTable("dbo.TicketPriorities");
        }
    }
}
