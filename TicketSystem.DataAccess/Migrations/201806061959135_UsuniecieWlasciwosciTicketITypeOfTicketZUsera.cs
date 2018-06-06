namespace TicketSystem.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuniecieWlasciwosciTicketITypeOfTicketZUsera : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Ticket", new[] { "User_Id" });
            DropColumn("dbo.Ticket", "UserId");
            RenameColumn(table: "dbo.Ticket", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Ticket", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Ticket", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Ticket", "UserId");
            DropColumn("dbo.AspNetUsers", "TicketId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "TicketId", c => c.Int(nullable: false));
            DropIndex("dbo.Ticket", new[] { "UserId" });
            AlterColumn("dbo.Ticket", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Ticket", "UserId", c => c.String());
            RenameColumn(table: "dbo.Ticket", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Ticket", "UserId", c => c.String());
            CreateIndex("dbo.Ticket", "User_Id");
        }
    }
}
