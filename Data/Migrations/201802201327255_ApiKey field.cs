namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApiKeyfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servers", "ApiKey", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Servers", "ApiKey");
        }
    }
}
