namespace testowo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Order", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Order", "UserId");
            RenameColumn(table: "dbo.Order", name: "ApplicationUser_Id", newName: "UserId");
            AddColumn("dbo.AspNetUsers", "UserData_Phone", c => c.String());
            AlterColumn("dbo.Order", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Order", "UserId");
            DropColumn("dbo.AspNetUsers", "UserData_Telefon");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "UserData_Telefon", c => c.String());
            DropIndex("dbo.Order", new[] { "UserId" });
            AlterColumn("dbo.Order", "UserId", c => c.String());
            DropColumn("dbo.AspNetUsers", "UserData_Phone");
            RenameColumn(table: "dbo.Order", name: "UserId", newName: "ApplicationUser_Id");
            AddColumn("dbo.Order", "UserId", c => c.String());
            CreateIndex("dbo.Order", "ApplicationUser_Id");
        }
    }
}
