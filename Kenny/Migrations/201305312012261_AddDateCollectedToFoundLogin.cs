namespace Kenny.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateCollectedToFoundLogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FoundLogins", "DateCollected", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FoundLogins", "DateCollected");
        }
    }
}
