namespace Kenny.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInitialModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        BaseUrl = c.String(nullable: false),
                        AuthenticatedUrl = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FoundLogins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        IsValid = c.Boolean(),
                        SourceUrl = c.String(),
                        Site_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sites", t => t.Site_Id, cascadeDelete: true)
                .Index(t => t.Site_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.FoundLogins", new[] { "Site_Id" });
            DropForeignKey("dbo.FoundLogins", "Site_Id", "dbo.Sites");
            DropTable("dbo.FoundLogins");
            DropTable("dbo.Sites");
        }
    }
}
