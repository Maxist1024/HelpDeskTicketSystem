namespace TicketSystem.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdAsString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ticket", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ticket", "UserId", c => c.Int(nullable: false));
        }
    }
}
