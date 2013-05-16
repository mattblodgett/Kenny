namespace Kenny.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOwnerToSite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sites", "OwnerId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Sites", "OwnerId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            CreateIndex("dbo.Sites", "OwnerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Sites", new[] { "OwnerId" });
            DropForeignKey("dbo.Sites", "OwnerId", "dbo.UserProfile");
            DropColumn("dbo.Sites", "OwnerId");
        }
    }
}
