namespace testowo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addShortDesc2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Course", "ShortDesc2", c => c.String());
            CreateIndex("dbo.Course", "CategoryId");
            AddForeignKey("dbo.Course", "CategoryId", "dbo.Category", "CategoryId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Course", "CategoryId", "dbo.Category");
            DropIndex("dbo.Course", new[] { "CategoryId" });
            DropColumn("dbo.Course", "ShortDesc2");
        }
    }
}
