namespace Shadow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNewColumnInNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketNotifications", "NotificationDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketNotifications", "NotificationDescription");
        }
    }
}
