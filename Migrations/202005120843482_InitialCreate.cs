namespace testowo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 100),
                        CategoryDesc = c.String(nullable: false),
                        NameFileIcon = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        TitleCourse = c.String(nullable: false, maxLength: 100),
                        AutorCourse = c.String(nullable: false, maxLength: 100),
                        DateAdd = c.DateTime(nullable: false),
                        NamePicture = c.String(maxLength: 100),
                        DescCourse = c.String(),
                        PriceCourse = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Bestseller = c.Boolean(nullable: false),
                        Hidden = c.Boolean(nullable: false),
                        ShortDesc = c.String(),
                    })
                .PrimaryKey(t => t.CourseId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Name = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Adress = c.String(nullable: false, maxLength: 100),
                        Town = c.String(nullable: false, maxLength: 100),
                        ZipCode = c.String(nullable: false, maxLength: 6),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        Commentary = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                        OrderValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderID);
            
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        OrderItemId = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        PurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderItemId)
                .ForeignKey("dbo.Course", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Order", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItem", "OrderID", "dbo.Order");
            DropForeignKey("dbo.OrderItem", "CourseId", "dbo.Course");
            DropIndex("dbo.OrderItem", new[] { "CourseId" });
            DropIndex("dbo.OrderItem", new[] { "OrderID" });
            DropTable("dbo.OrderItem");
            DropTable("dbo.Order");
            DropTable("dbo.Course");
            DropTable("dbo.Category");
        }
    }
}
